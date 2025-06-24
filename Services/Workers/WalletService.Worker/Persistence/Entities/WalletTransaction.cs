using WalletService.Worker.Persistence.Enums;

namespace WalletService.Worker.Persistence.Entities;

public class WalletTransaction
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public TransactionType Type { get; set; } = TransactionType.Unknown;
    public string Description { get; set; } = null!;
    public Guid CorrelationId { get; set; }
    public DateTime CreatedAt { get; set; }
}