using System.Text;
using System.Text.Json;
using Finvia.Shared.Messaging.Abstractions;
using RabbitMQ.Client;


namespace Finvia.Shared.Messaging;

public class RabbitMqEventBus : IEventBus
{
    private readonly IModel _channel;

    public RabbitMqEventBus(IModel channel)
    {
        _channel = channel;
        _channel.ExchangeDeclare("event_bus", ExchangeType.Topic, durable: true);
    }

    public Task PublishAsync<T>(T @event, string routingKey, Guid correlationId) where T : class
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
        var props = _channel.CreateBasicProperties();
        props.Headers = new Dictionary<string, object>
        {
            ["correlationId"] = correlationId.ToString()
        };
        
        props.ContentType = "application/json";
        props.DeliveryMode = 2;

        _channel.BasicPublish(
            exchange: "event_bus",
            routingKey: routingKey,
            basicProperties: props,
            body: body);

        Console.WriteLine($"[RABBITMQ] Event published: {typeof(T).Name} - {routingKey}");

        return Task.CompletedTask;
    }
}
