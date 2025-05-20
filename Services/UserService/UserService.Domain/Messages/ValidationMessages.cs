namespace UserService.Domain.Messages;

public static class ValidationMessages
{
    public const string FullNameRequired = "Full name is required.";
    public const string EmailRequired = "Email is required.";
    public const string EmailInvalid = "Email format is invalid.";
    public const string PasswordRequired = "Password is required.";
    public const string PasswordTooShort = "Password must be at least 6 characters.";
    public const string PhoneRequired = "Phone number is required.";
    public const string PhoneInvalid = "Phone number must contain digits only.";
    public const string EmailAlreadyExists = "Email is already exists";
    
}