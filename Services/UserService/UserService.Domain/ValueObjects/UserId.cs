using Finvia.Shared.Domain;

namespace UserService.Domain.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId(Guid value)
    {
        Value = value;
    }

    public static UserId Create() => new(Guid.NewGuid());
    public static UserId From(Guid id) => new(id);
    public static implicit operator Guid(UserId id) => id.Value;
    
    public static implicit operator UserId(Guid value) => new UserId(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}