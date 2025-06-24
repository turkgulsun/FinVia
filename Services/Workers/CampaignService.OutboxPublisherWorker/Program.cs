using CampaignService.OutboxPublisherWorker;
using CampaignService.OutboxPublisherWorker.Persistence;
using Finvia.Shared.Outbox.Abstractions;
using Finvia.Shared.Messaging;
using Finvia.Shared.Messaging.Abstractions;
using Finvia.Shared.Outbox;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

#region DbContext

builder.Services.AddDbContext<CampaignDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("CampaignDb"));
});
builder.Services.AddScoped<IOutboxDbContext>(sp => sp.GetRequiredService<CampaignDbContext>());

#endregion

#region RabbitMQ - EventBus

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

#region Outbox Publisher

builder.Services.AddScoped<IOutboxMessagePublisher, OutboxMessagePublisher>();

#endregion

builder.Services.AddHostedService<Worker>();

builder.Build().Run();