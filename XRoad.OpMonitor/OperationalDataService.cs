using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using SimpleSOAPClient;
using SimpleSOAPClient.Handlers;
using SimpleSOAPClient.Helpers;
using SimpleSOAPClient.Models;
using XRoad.Domain;
using XRoad.Domain.Header;
using XRoad.OpMonitor.Domain;
using XRoad.OpMonitor.Domain.SOAP;

namespace XRoad.OpMonitor
{
    public class OperationalDataService : IOperationalDataService
    {
        private static readonly UserIdHeader UserIdHeader = new UserIdHeader
            {Value = ".NetCore_ServiceMetadataManager"};

        public OperationalData GetOperationalData(XRoadExchangeParameters xRoadExchangeParameters,
            SecurityServerIdentifier securityServerIdentifier,
            SearchCriteria searchCriteria)
        {
            byte[] attachmentBytes = { };

            var client = SoapClient.Prepare()
                .WithHandler(new DelegatingSoapHandler
                {
                    OnHttpResponseAsyncAction = async (soapClient, httpContext, cancellationToken) =>
                    {
                        if (httpContext.Response.Content.IsMimeMultipartContent())
                        {
                            var streamProvider =
                                await httpContext.Response.Content.ReadAsMultipartAsync(cancellationToken);
                            var contentCursor = streamProvider.Contents.GetEnumerator();

                            Debug.Assert(contentCursor.MoveNext());
                            var soapResponse = contentCursor.Current;

                            Debug.Assert(contentCursor.MoveNext());
                            var attachment = contentCursor.Current;

                            contentCursor.Dispose();

                            attachmentBytes = await attachment.ReadAsByteArrayAsync();
                            httpContext.Response.Content = soapResponse;
                        }
                    }
                });

            var body = SoapEnvelope.Prepare().Body(new GetSecurityServerOperationalData
            {
                SearchCriteria = searchCriteria
            }).WithHeaders(new List<SoapHeader>
            {
                IdHeader.Random,
                UserIdHeader,
                ProtocolVersionHeader.Version40,
                (XRoadClient) xRoadExchangeParameters.ClientSubSystem,
                new XRoadService
                {
                    Instance = securityServerIdentifier.Instance,
                    MemberClass = securityServerIdentifier.MemberClass,
                    MemberCode = securityServerIdentifier.MemberCode,
                    ServiceCode = "getSecurityServerOperationalData"
                },
                (XRoadSecurityServer) securityServerIdentifier
            });

            var result = client.Send(xRoadExchangeParameters.SecurityServerUri.ToString(), string.Empty, body);
            result.ThrowIfFaulted();

            var operationalData = result.Body<OperationalData>();
            operationalData.Attachment = attachmentBytes;
            return operationalData;
        }
    }
}