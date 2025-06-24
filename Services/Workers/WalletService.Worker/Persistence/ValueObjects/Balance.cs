using Finvia.Shared.Domain;
using WalletService.Worker.Persistence.Enums;

namespace WalletService.Worker.Persistence.ValueObjects;

public class Balance : ValueObject
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public Balance(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public Balance Add(decimal amount) => new(Amount + amount, Currency);

    public Balance Subtract(decimal amount)
    {
        if (Amount - amount < 0)
            throw new InvalidOperationException("Insufficient balance.");

        return new Balance(Amount - amount, Currency);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount} {Currency}";
}