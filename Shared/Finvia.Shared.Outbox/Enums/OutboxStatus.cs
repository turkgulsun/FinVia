namespace Finvia.Shared.Outbox.Enums;

public enum OutboxStatus
{
    Pending = 0,
    Processed,
    Failed,
    Retrying,
    DeadLetter
}