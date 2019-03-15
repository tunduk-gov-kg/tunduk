using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SimpleSOAPClient;
using SimpleSOAPClient.Handlers;
using SimpleSOAPClient.Helpers;
using SimpleSOAPClient.Models;
using XRoad.Domain;
using XRoad.Domain.Header;
using XRoad.GlobalConfiguration.Domain.SOAP;
using XRoad.GlobalConfiguration.Metadata;

namespace XRoad.GlobalConfiguration
{
    public class ServiceMetadataManager : IServiceMetadataManager
    {
        private static readonly UserIdHeader UserIdHeader = new UserIdHeader() { Value = ".NetCore_ServiceMetadataManager" };

        public async Task<SharedParams> GetSharedParamsAsync(Uri securityServerUri)
        {
            var httpClient = new HttpClient();
            var verificationConfUri = new Uri(securityServerUri, "verificationconf");
            using (var httpStream = await httpClient.GetStreamAsync(verificationConfUri))
            {
                using (ZipArchive zipArchive = new ZipArchive(httpStream))
                {
                    string instanceIdentifier = await GetInstanceIdentifierAsync(zipArchive);
                    return GetSharedParams(instanceIdentifier, zipArchive);
                }
            }
        }

        public async Task<byte[]> GetWsdlAsync(Uri securityServerUri, SubSystemIdentifier subSystemId, ServiceIdentifier targetService)
        {
            byte[] wsdlFileBytes = {};
            
            var client = SoapClient.Prepare()
                .WithHandler(new DelegatingSoapHandler
                {
                    OnHttpResponseAsyncAction = async (soapClient, httpContext, cancellationToken) =>
                    {
                        if (httpContext.Response.Content.IsMimeMultipartContent())
                        {
                            var streamProvider = await httpContext.Response.Content.ReadAsMultipartAsync(cancellationToken);
                            var contentCursor = streamProvider.Contents.GetEnumerator();

                            Debug.Assert(contentCursor.MoveNext());
                            var soapResponse = contentCursor.Current;

                            Debug.Assert(contentCursor.MoveNext());
                            var wsdlFile = contentCursor.Current;

                            contentCursor.Dispose();

                            wsdlFileBytes = await wsdlFile.ReadAsByteArrayAsync();
                            httpContext.Response.Content = soapResponse;
                        }
                    }
                });

            var body = SoapEnvelope.Prepare().Body(new GetWsdlRequest
            {
                ServiceCode = targetService.ServiceCode,
                ServiceVersion = targetService.ServiceVersion
            }).WithHeaders(new List<SoapHeader>
            {
                IdHeader.Random,
                UserIdHeader,
                ProtocolVersionHeader.Version40,
                (XRoadClient)subSystemId,
                new XRoadService
                {
                    Instance = targetService.Instance,
                    MemberClass = targetService.MemberClass,
                    MemberCode =targetService.MemberCode,
                    SubSystemCode = targetService.SubSystemCode,
                    ServiceCode = "getWsdl",
                    ServiceVersion = "v1"
                }
            });

            var result = await client.SendAsync(securityServerUri.ToString(), String.Empty, body);
            result.ThrowIfFaulted();
            
            return wsdlFileBytes;
        }

        public async Task<IList<ServiceIdentifier>> GetServicesAsync(Uri securityServerUri, SubSystemIdentifier client, SubSystemIdentifier source)
        {
            var soapEnvelope = SoapEnvelope.Prepare()
                .Body(new ListMethodsRequest())
                .WithHeaders(
                new List<SoapHeader>()
                {
                    IdHeader.Random,
                    UserIdHeader,
                    ProtocolVersionHeader.Version40,
                    (XRoadClient) client,
                    new XRoadService()
                    {
                        Instance = source.Instance,
                        MemberClass = source.MemberClass,
                        MemberCode = source.MemberCode,
                        SubSystemCode = source.SubSystemCode,
                        ServiceCode = "listMethods"
                    }
                });
            var envelope = await SoapClient.Prepare().SendAsync(securityServerUri.ToString(), String.Empty, soapEnvelope);
            return envelope.Body<ListMethodsResponse>().Services.Select(o => (ServiceIdentifier)o).ToList();
        }


        private async Task<string> GetInstanceIdentifierAsync(ZipArchive zipArchive)
        {
            const string zipEntryName = "verificationconf/instance-identifier";
            ZipArchiveEntry zipEntry = zipArchive.GetEntry(zipEntryName);

            Debug.Assert(zipEntry != null, nameof(zipEntry) + " != null");

            using (Stream instanceIdentifierStream = zipEntry.Open())
            {
                var reader = new StreamReader(instanceIdentifierStream);
                return await reader.ReadToEndAsync();
            }
        }

        private SharedParams GetSharedParams(string instanceIdentifier, ZipArchive zipArchive)
        {
            string zipEntryName = $"verificationconf/{instanceIdentifier}/shared-params.xml";
            ZipArchiveEntry zipEntry = zipArchive.GetEntry(zipEntryName);

            Debug.Assert(zipEntry != null, nameof(zipEntry) + " != null");

            using (Stream sharedParamsStream = zipEntry.Open())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SharedParams));
                return (SharedParams)xmlSerializer.Deserialize(sharedParamsStream);
            }
        }
    }
}
