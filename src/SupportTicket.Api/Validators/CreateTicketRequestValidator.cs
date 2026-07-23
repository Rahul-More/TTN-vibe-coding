using System.Text.Json;
using FluentValidation;
using SupportTicket.Api.DTOs.Requests;
using SupportTicket.Api.Helpers;
using SupportTicket.Api.Repositories;

namespace SupportTicket.Api.Validators;

public class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketRequestValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Title)
            .Must(title => !string.IsNullOrWhiteSpace(title?.Trim()))
            .WithMessage("Title is required")
            .DependentRules(() =>
            {
                RuleFor(x => x.Title)
                    .Must(title => title.Trim().Length <= 200)
                    .WithMessage("Title must not exceed 200 characters");
            });

        RuleFor(x => x.Description)
            .Must(desc => desc is null || desc.Trim().Length <= 2000)
            .WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.Priority)
            .NotEmpty()
            .WithMessage("Priority is required")
            .Must(priority => EnumParsing.TryParsePriority(priority, out _))
            .WithMessage(x => EnumParsing.InvalidPriorityMessage(x.Priority));

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy is required");

        RuleFor(x => x.CreatedBy)
            .MustAsync(async (id, cancellation) => await userRepository.ExistsAsync(id))
            .WithMessage(x => $"User with id {x.CreatedBy} does not exist");

        RuleFor(x => x.AssignedTo)
            .MustAsync(async (assignedTo, cancellation) =>
                !assignedTo.HasValue || await userRepository.ExistsAsync(assignedTo.Value))
            .WithMessage(x => $"User with id {x.AssignedTo} does not exist")
            .When(x => x.AssignedTo.HasValue);

        RuleFor(x => x.ExtensionData)
            .Must(data => !HasStatusProperty(data))
            .WithMessage("Status cannot be set on create. Use PATCH /api/tickets/{id}/status.");
    }

    private static bool HasStatusProperty(Dictionary<string, JsonElement>? data) =>
        data is not null && data.Keys.Any(k => k.Equals("status", StringComparison.OrdinalIgnoreCase));
}
