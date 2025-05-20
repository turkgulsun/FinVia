using Finvia.Shared.Domain;
using WalletService.Domain.Enums;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Entities;

public class Wallet : IAggregateRoot
{
    public WalletId Id { get; private set; } = WalletId.Create();
    public Guid UserId { get; private set; }
    public Balance Balance { get; private set; }
    public Currency Currency { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private Wallet() { } // EF Core

    public Wallet(Guid userId, Currency currency)
    {
        Id = WalletId.Create();
        UserId = userId;
        Currency = currency;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
        Balance = new Balance(0, currency);
    }

    public void Credit(decimal amount)
    {
        EnsureActive();

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        Balance = Balance.Add(amount);
    }

    public void Debit(decimal amount)
    {
        EnsureActive();

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        if (Balance.Amount < amount)
            throw new InvalidOperationException("Insufficient balance.");

        Balance = Balance.Subtract(amount);
    }

    public void Deactivate() => IsActive = false;

    private void EnsureActive()
    {
        if (!IsActive)
            throw new InvalidOperationException("Wallet is inactive.");
    }
}