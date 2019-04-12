using Catalog.Domain.Entity;
using FluentValidation;

namespace Catalog.Domain.Validation
{
    public class OperationalDataRecordValidator : AbstractValidator<OperationalDataRecord>
    {
        public OperationalDataRecordValidator()
        {
            RuleFor(model => model.Succeeded).NotNull();
            
            RuleFor(model => model.SecurityServerType).NotEmpty();
            
            RuleFor(model => model.MessageId).NotEmpty();
            RuleFor(model => model.MessageProtocolVersion).NotEmpty();

            RuleFor(model => model.ClientXRoadInstance).NotEmpty();
            RuleFor(model => model.ClientMemberClass).NotEmpty();
            RuleFor(model => model.ClientMemberCode).NotEmpty();

            RuleFor(model => model.ServiceXRoadInstance).NotEmpty();
            RuleFor(model => model.ServiceMemberClass).NotEmpty();
            RuleFor(model => model.ServiceMemberCode).NotEmpty();
            RuleFor(model => model.ServiceCode).NotEmpty();
        }
    }
}