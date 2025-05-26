using System.Reflection;
using FluentValidation;
using KycService.Application.Abstractions;
using KycService.Application.Commands.Kyc;
using KycService.Application.Common;
using KycService.Domain.Abstractions;
using KycService.Infrastructure.EventHandlers;
using KycService.Infrastructure.Persistence;
using KycService.Infrastructure.Providers;
using KycService.Infrastructure.Repositories;
using KycService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Finvia.Shared.Middlewares;
using Finvia.Shared.Messaging;
using Finvia.Shared.Messaging.Abstractions;
using Finvia.Shared.Outbox;
using Finvia.Shared.Outbox.Abstractions;

var builder = WebApplication.CreateBuilder(args);

#region Serilog

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "kyc-logs-{0:yyyy.MM.dd}"
    })
    .CreateLogger();

builder.Host.UseSerilog();

#endregion

#region Core Services

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kyc Service API", Version = "v1" });
});

#endregion

#region Database

builder.Services.AddDbContext<KycDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("KycDb")));

builder.Services.AddScoped<IOutboxDbContext>(sp => sp.GetRequiredService<KycDbContext>());

#endregion

#region Dependency Injection

builder.Services.AddScoped<IKycRepository, KycRepository>();
builder.Services.AddScoped<IKycVerificationProvider, KycVerificationProvider>();
builder.Services.AddScoped<IKycAuditService, KycAuditService>();
builder.Services.AddScoped<IIntegrationEventDispatcher, IntegrationEventDispatcher>();
builder.Services.AddScoped<DomainEventDispatcher>();
builder.Services.AddScoped<IOutboxWriter, OutboxWriter>();

#endregion

#region FluentValidation & MediatR

builder.Services.AddValidatorsFromAssemblyContaining<SubmitKycCommandValidator>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(SubmitKycCommandHandler).Assembly,
        typeof(KycRejectedEventHandler).Assembly
    ));

#endregion

#region RabbitMQ

builder.Services.AddSingleton<IConnection>(_ =>
{
    var factory = new ConnectionFactory
    {
        HostName = "rabbitmq",
        Port = 5672
    };
    return factory.CreateConnection();
});

builder.Services.AddSingleton<IModel>(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    return connection.CreateModel();
});

builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();

#endregion

#region OpenTelemetry

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("KycService"))
            .AddOtlpExporter(opt =>
            {
                opt.Endpoint = new Uri("http://localhost:4317");
                opt.Protocol = OtlpExportProtocol.Grpc;
            }));

#endregion

#region Application Build

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#endregion

#region Middlewares

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ActivityEnricherMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>(); 

app.UseHttpsRedirection();

#endregion

// #region Migrations (Auto Apply)
//
// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<KycDbContext>();
//     db.Database.Migrate();
// }
//
// #endregion

app.MapControllers();
app.Run();
