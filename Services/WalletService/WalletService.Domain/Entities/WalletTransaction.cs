using Finvia.Shared.Domain;
using WalletService.Domain.Enums;

namespace WalletService.Domain.Entities;

public class WalletTransaction : Entity
{
    public Guid WalletId { get; private set; }
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }
    public string Description { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }

    private WalletTransaction() { }

    public WalletTransaction(Guid walletId, decimal amount, Currency currency, string description)
    {
        WalletId = walletId;
        Amount = amount;
        Currency = currency;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }
}