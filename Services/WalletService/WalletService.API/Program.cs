using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using WalletService.API.Validators;
using WalletService.Application.Commands.CreateWallet;
using WalletService.Application.Mappings;
using WalletService.Infrastructure.Persistence;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WalletService.API.Middlewares;
using Serilog.Enrichers.Span;
using WalletService.Domain.Abstractions;
using WalletService.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// 🔧 Serilog yapılandırması
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "WalletService")
    .Enrich.With<WalletService.API.Enrichers.ActivityEnricher>() // TraceId ve SpanId için enrich
    .Enrich.WithThreadId()
    .Enrich.WithSpan()               // Serilog.Sinks.OpenTelemetry varsa
    .Enrich.WithProcessId()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// ✅ OpenTelemetry Tracing
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation()
        .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService("WalletService"))
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri("http://localhost:4317");
            opt.Protocol = OtlpExportProtocol.Grpc;
        })
    );

// 🛠 Uygulama servisleri
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WalletDb")));

builder.Services.AddScoped<IWalletRepository, WalletRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateWalletCommandHandler).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<CreateWalletRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();
MapsterConfiguration.RegisterMappings();

var app = builder.Build();

Log.Information("✅ WalletService is starting...");

// 🔄 Middleware'ler
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ActivityEnricherMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
