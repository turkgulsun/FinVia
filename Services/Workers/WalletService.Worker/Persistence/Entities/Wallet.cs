using WalletService.Worker.Persistence.ValueObjects;

namespace WalletService.Worker.Persistence.Entities;

public class Wallet
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Balance Balance { get; private set; }
    public string Currency { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    public void Credit(decimal amount)
    {
        EnsureActive();

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        Balance = Balance.Add(amount);
    }

    private void EnsureActive()
    {
        if (!IsActive)
            throw new InvalidOperationException("Wallet is inactive.");
    }
}