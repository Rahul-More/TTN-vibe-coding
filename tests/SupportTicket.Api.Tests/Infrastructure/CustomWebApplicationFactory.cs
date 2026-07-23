using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SupportTicket.Api.Data;
using SupportTicket.Api.Data.Seed;

namespace SupportTicket.Api.Tests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestConnectionString =
        "Host=localhost;Port=5432;Database=ticketdb_test;Username=postgres;Password=password";

    private HttpClient? _client;
    private bool _initialized;

    public HttpClient Client => _client ??= CreateInitializedClient();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = TestConnectionString
            });
        });
    }

    private HttpClient CreateInitializedClient()
    {
        if (!_initialized)
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            db.Database.EnsureDeleted();
            db.Database.Migrate();
            DbSeeder.SeedAsync(db, env).GetAwaiter().GetResult();
            db.Database.ExecuteSqlRaw(
                """
                SELECT setval(
                    pg_get_serial_sequence('"Tickets"', 'Id'),
                    COALESCE((SELECT MAX("Id") FROM "Tickets"), 1));
                """);
            _initialized = true;
        }

        return CreateClient();
    }
}
