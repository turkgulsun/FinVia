namespace WalletService.Domain.Messages;

public static class ValidationMessages
{
    public const string UserIdRequired = "UserId must not be empty.";
    public const string CurrencyRequired = "Currency is required.";
    public const string CurrencyInvalid = "Supported currencies are: TRY, USD, EUR.";
}