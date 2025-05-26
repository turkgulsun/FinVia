using Finvia.Shared.Outbox;
using Finvia.Shared.Outbox.Abstractions;
using KycService.Worker;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<OutboxDbContext>(options =>
            options.UseNpgsql(context.Configuration.GetConnectionString("KycDb")));

        services.AddSingleton<IOutboxDbContext>(sp => sp.GetRequiredService<OutboxDbContext>());

        services.AddSingleton<IConnection>(_ =>
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5672
            };
            return factory.CreateConnection();
        });

        services.AddSingleton<IModel>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            return connection.CreateModel();
        });

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();