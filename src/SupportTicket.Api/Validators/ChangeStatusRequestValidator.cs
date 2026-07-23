using FluentValidation;
using SupportTicket.Api.DTOs.Requests;
using SupportTicket.Api.Helpers;

namespace SupportTicket.Api.Validators;

public class ChangeStatusRequestValidator : AbstractValidator<ChangeStatusRequest>
{
    public ChangeStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .Must(status => EnumParsing.TryParseStatus(status, out _))
            .WithMessage(x => EnumParsing.InvalidStatusMessage(x.Status));
    }
}
