using FluentValidation;
using KycService.Application.Abstractions;
using KycService.Application.Commands.Kyc;
using KycService.Application.Common;
using KycService.Domain.Abstractions;
using KycService.Infrastructure.EventHandlers;
using KycService.Infrastructure.Persistence;
using KycService.Infrastructure.Providers;
using KycService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Kyc Service API", Version = "v1" });
});

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(SubmitKycCommandHandler).Assembly,
        typeof(KycRejectedEventHandler).Assembly
    ));

builder.Services.AddValidatorsFromAssemblyContaining<SubmitKycCommandValidator>();

builder.Services.AddDbContext<KycDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("KycDb")));

builder.Services.AddScoped<IKycRepository, KycRepository>();
builder.Services.AddScoped<IKycVerificationProvider, KycVerificationProvider>();

builder.Services.AddScoped<DomainEventDispatcher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();