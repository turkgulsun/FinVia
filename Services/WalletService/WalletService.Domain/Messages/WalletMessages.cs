namespace WalletService.Domain.Messages;

public static class WalletMessages
{
    public const string InvalidCurrency = "Invalid currency. Supported: TRY, USD, EUR";
    public const string WalletCreated = "Wallet successfully created.";
    public const string WalletInactive = "This wallet is inactive.";
    public const string NotFound = "Wallet not found for the given user.";
    public const string InsufficientFunds = "Insufficient wallet balance.";
}