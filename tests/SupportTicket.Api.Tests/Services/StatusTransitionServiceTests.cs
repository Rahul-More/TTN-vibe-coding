using SupportTicket.Api.Models.Enums;
using SupportTicket.Api.Services;
using Xunit;

namespace SupportTicket.Api.Tests.Services;

public class StatusTransitionServiceTests
{
    private readonly StatusTransitionService _service = new();

    public static TheoryData<TicketStatus, TicketStatus, bool> TransitionMatrix =>
        new()
        {
            // Valid transitions (5)
            { TicketStatus.Open, TicketStatus.InProgress, true },
            { TicketStatus.Open, TicketStatus.Cancelled, true },
            { TicketStatus.InProgress, TicketStatus.Resolved, true },
            { TicketStatus.InProgress, TicketStatus.Cancelled, true },
            { TicketStatus.Resolved, TicketStatus.Closed, true },

            // Invalid from Open (3)
            { TicketStatus.Open, TicketStatus.Open, false },
            { TicketStatus.Open, TicketStatus.Resolved, false },
            { TicketStatus.Open, TicketStatus.Closed, false },

            // Invalid from InProgress (3)
            { TicketStatus.InProgress, TicketStatus.Open, false },
            { TicketStatus.InProgress, TicketStatus.InProgress, false },
            { TicketStatus.InProgress, TicketStatus.Closed, false },

            // Invalid from Resolved (4)
            { TicketStatus.Resolved, TicketStatus.Open, false },
            { TicketStatus.Resolved, TicketStatus.InProgress, false },
            { TicketStatus.Resolved, TicketStatus.Resolved, false },
            { TicketStatus.Resolved, TicketStatus.Cancelled, false },

            // Invalid from Closed (5)
            { TicketStatus.Closed, TicketStatus.Open, false },
            { TicketStatus.Closed, TicketStatus.InProgress, false },
            { TicketStatus.Closed, TicketStatus.Resolved, false },
            { TicketStatus.Closed, TicketStatus.Closed, false },
            { TicketStatus.Closed, TicketStatus.Cancelled, false },

            // Invalid from Cancelled (5)
            { TicketStatus.Cancelled, TicketStatus.Open, false },
            { TicketStatus.Cancelled, TicketStatus.InProgress, false },
            { TicketStatus.Cancelled, TicketStatus.Resolved, false },
            { TicketStatus.Cancelled, TicketStatus.Closed, false },
            { TicketStatus.Cancelled, TicketStatus.Cancelled, false },
        };

    [Theory]
    [MemberData(nameof(TransitionMatrix))]
    public void IsValidTransition_ReturnsExpected(TicketStatus current, TicketStatus target, bool expected)
    {
        var result = _service.IsValidTransition(current, target);

        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(TransitionMatrix))]
    public void ValidateTransition_ReturnsExpectedResult(TicketStatus current, TicketStatus target, bool expectedValid)
    {
        var result = _service.ValidateTransition(current, target);

        if (expectedValid)
        {
            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
            Assert.Null(result.Code);
        }
        else
        {
            Assert.False(result.IsSuccess);
            Assert.Equal($"Cannot transition from {current} to {target}", result.Error);
            Assert.Equal("INVALID_TRANSITION", result.Code);
        }
    }

    [Theory]
    [InlineData(TicketStatus.Open, new[] { "InProgress", "Cancelled" })]
    [InlineData(TicketStatus.InProgress, new[] { "Resolved", "Cancelled" })]
    [InlineData(TicketStatus.Resolved, new[] { "Closed" })]
    [InlineData(TicketStatus.Closed, new string[] { })]
    [InlineData(TicketStatus.Cancelled, new string[] { })]
    public void GetValidNextStatuses_ReturnsExpectedStatuses(TicketStatus current, string[] expected)
    {
        var result = _service.GetValidNextStatuses(current);

        Assert.Equal(expected, result);
    }
}
