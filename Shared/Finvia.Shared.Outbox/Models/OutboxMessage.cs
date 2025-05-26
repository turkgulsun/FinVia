using Finvia.Shared.Outbox.Enums;

namespace Finvia.Shared.Outbox.Models;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string EventType { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; set; }
    public OutboxStatus Status { get; set; } = OutboxStatus.Pending;
    public Guid CorrelationId { get; set; }
    public DateTime? FailedAt { get; set; }
    public int RetryCount { get; set; } = 0;
    public string? ErrorDetails { get; set; }
}