namespace Finvia.Shared.Domain;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
