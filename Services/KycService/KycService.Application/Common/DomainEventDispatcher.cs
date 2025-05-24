using Finvia.Shared.Domain;
using MediatR;

namespace KycService.Application.Common;

public class DomainEventDispatcher(IMediator mediator)
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> events)
    {
        foreach (var domainEvent in events)
        {
            await mediator.Publish(domainEvent);
        }
    }
}