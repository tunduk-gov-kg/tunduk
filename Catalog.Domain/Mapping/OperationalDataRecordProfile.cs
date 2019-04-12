using AutoMapper;
using Catalog.Domain.Entity;
using Catalog.Domain.Enum;
using Catalog.Domain.Mapping.Resolvers;
using XRoad.OpMonitor.Domain;

namespace Catalog.Domain.Mapping
{
    public class OperationalDataRecordProfile : Profile
    {
        public OperationalDataRecordProfile()
        {
            CreateMap<OperationalDataRecordDto, OperationalDataRecord>();

            CreateMap<OperationalDataRecord, Message>()
                .ForMember(message => message.MessageState,
                    options => options.MapFrom(expression =>
                        expression.IsConsumer ? MessageState.MergedConsumer : MessageState.MergedProducer))
                .ForMember(message => message.ConsumerMessageLifecycle,
                    options => options.MapFrom<ConsumerMessageLifecycleResolver>())
                .ForMember(message => message.ProducerMessageLifecycle,
                    options => options.MapFrom<ProducerMessageLifecycleResolver>())
                .ForMember(message => message.ConsumerInstance,
                    options => options.MapFrom(expression => expression.ClientXRoadInstance))
                .ForMember(message => message.ConsumerMemberClass,
                    options => options.MapFrom(expression => expression.ClientMemberClass))
                .ForMember(message => message.ConsumerMemberCode,
                    options => options.MapFrom(expression => expression.ClientMemberCode))
                .ForMember(message => message.ConsumerSubSystemCode,
                    options => options.MapFrom(expression => expression.ClientSubsystemCode))
                .ForMember(message => message.ConsumerSecurityServerAddress,
                    options => options.MapFrom(expression => expression.ClientSecurityServerAddress))
                .ForMember(message => message.ConsumerSecurityServerInternalIpAddress,
                    options => options.MapFrom(expression =>
                        expression.IsConsumer ? expression.SecurityServerInternalIp : null))
                .ForMember(message => message.ProducerInstance,
                    options => options.MapFrom(expression => expression.ServiceXRoadInstance))
                .ForMember(message => message.ProducerMemberClass,
                    options => options.MapFrom(expression => expression.ServiceMemberClass))
                .ForMember(message => message.ProducerMemberCode,
                    options => options.MapFrom(expression => expression.ServiceMemberCode))
                .ForMember(message => message.ProducerSubSystemCode,
                    options => options.MapFrom(expression => expression.ServiceSubsystemCode))
                .ForMember(message => message.ProducerServiceCode,
                    options => options.MapFrom(expression => expression.ServiceCode))
                .ForMember(message => message.ProducerServiceVersion,
                    options => options.MapFrom(expression => expression.ServiceVersion))
                .ForMember(message => message.ProducerSecurityServerAddress,
                    options => options.MapFrom(expression => expression.ServiceSecurityServerAddress))
                .ForMember(message => message.ProducerSecurityServerInternalIpAddress,
                    options => options.MapFrom(expression =>
                        expression.IsProducer ? expression.SecurityServerInternalIp : null))
                .ForMember(message => message.IsSucceeded,
                    options => options.MapFrom(expression => expression.Succeeded))
                .ForMember(message => message.FaultCode,
                    options => options.MapFrom(expression => expression.SoapFaultCode))
                .ForMember(message => message.FaultString,
                    options => options.MapFrom(expression => expression.SoapFaultString))
                .ForMember(message => message.RequestAttachmentsCount,
                    options => options.MapFrom(expression => expression.RequestAttachmentCount))
                .ForMember(message => message.ResponseAttachmentsCount,
                    options => options.MapFrom(expression => expression.ResponseAttachmentCount));
        }
    }
}