namespace WalletService.Application.DTOs;

public record WalletDto(
    Guid Id,
    Guid UserId,
    string Currency,
    decimal Balance,
    DateTime CreatedAt,
    bool IsActive
);