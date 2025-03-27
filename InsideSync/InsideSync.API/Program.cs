using InsideSync.Application.Interfaces;
using InsideSync.Application.Services;
using InsideSync.Infrastructure.Repositories;
using InsideSync.Infrastructure.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddScoped<IOtpRepository, OtpRepository>();
builder.Services.AddScoped<IEmailService, SendGridEmailSender>();
builder.Services.AddScoped<OtpManager>();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
