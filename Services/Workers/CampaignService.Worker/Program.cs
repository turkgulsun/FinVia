using CampaignService.Worker;
using CampaignService.Worker.Persistence;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // PostgreSQL – CampaignDbContext
        services.AddDbContext<CampaignDbContext>(options =>
            options.UseNpgsql(context.Configuration.GetConnectionString("CampaignDb")));
        
        services.AddScoped<CampaignDbContext>();

        // RabbitMQ – Connection & Channel
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

        // Hosted Worker
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();