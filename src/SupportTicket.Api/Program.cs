using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupportTicket.Api.Data;
using SupportTicket.Api.Data.Seed;
using SupportTicket.Api.DTOs.Requests;
using SupportTicket.Api.DTOs.Responses;
using SupportTicket.Api.Filters;
using SupportTicket.Api.Middleware;
using SupportTicket.Api.Repositories;
using SupportTicket.Api.Services;
using SupportTicket.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<FluentValidationAsyncActionFilter>();

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<FluentValidationAsyncActionFilter>();
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var firstError = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault() ?? "Validation failed";

            return new BadRequestObjectResult(new ErrorResponse(firstError));
        };
    });

builder.Services.AddValidatorsFromAssemblyContaining<CreateTicketRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Support Ticket Management API",
        Version = "v1",
        Description = "REST API for Core scope — see api-contract.md"
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStatusTransitionService, StatusTransitionService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ICommentService, CommentService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(db, env);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support Ticket API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseCors();
app.MapControllers();
app.Run();

public partial class Program;
