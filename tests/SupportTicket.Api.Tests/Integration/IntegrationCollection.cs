using SupportTicket.Api.Tests.Infrastructure;
using Xunit;

namespace SupportTicket.Api.Tests.Integration;

[CollectionDefinition("Integration", DisableParallelization = true)]
public class IntegrationCollection : ICollectionFixture<CustomWebApplicationFactory>;
