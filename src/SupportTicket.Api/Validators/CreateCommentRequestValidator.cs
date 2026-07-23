using FluentValidation;
using SupportTicket.Api.DTOs.Requests;
using SupportTicket.Api.Repositories;

namespace SupportTicket.Api.Validators;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Message)
            .Must(message => !string.IsNullOrWhiteSpace(message?.Trim()))
            .WithMessage("Message is required")
            .DependentRules(() =>
            {
                RuleFor(x => x.Message)
                    .Must(message => message.Trim().Length <= 1000)
                    .WithMessage("Message must not exceed 1000 characters");
            });

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy is required");

        RuleFor(x => x.CreatedBy)
            .MustAsync(async (id, cancellation) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"User with id {x.CreatedBy} does not exist");
    }
}
