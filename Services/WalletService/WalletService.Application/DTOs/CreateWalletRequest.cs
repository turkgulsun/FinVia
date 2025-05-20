namespace WalletService.Application.DTOs;

public record CreateWalletRequest(Guid UserId, string Currency);