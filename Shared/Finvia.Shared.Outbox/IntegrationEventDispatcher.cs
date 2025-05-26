using Finvia.Shared.Outbox.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Finvia.Shared.Outbox;

public class IntegrationEventDispatcher : IIntegrationEventDispatcher
{
    private readonly IOutboxWriter _writer;
    private readonly IHttpContextAccessor _httpContext;

    public IntegrationEventDispatcher(IOutboxWriter writer, IHttpContextAccessor httpContext)
    {
        _writer = writer;
        _httpContext = httpContext;
    }

    public async Task DispatchAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        var correlationId = _httpContext.HttpContext?.Items["CorrelationId"]?.ToString()
                            ?? Guid.NewGuid().ToString();

        await _writer.SaveAsync(
            @event,
            eventType: typeof(T).Name,
            correlationId: Guid.Parse(correlationId),
            cancellationToken
        );
    }
}