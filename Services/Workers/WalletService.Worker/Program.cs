using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WalletService.Worker;
using WalletService.Worker.Consumers;
using WalletService.Worker.Persistence;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WalletDb")));

builder.Services.AddHostedService<RewardWalletIntegrationEventConsumer>();

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

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();