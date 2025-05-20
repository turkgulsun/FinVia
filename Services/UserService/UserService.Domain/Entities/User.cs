using Finvia.Shared.Domain;
using UserService.Domain.Enums;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Entities;

public class User : IAggregateRoot
{
    public UserId Id { get; private set; } = UserId.Create();
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }         // ✅ Şifre hash olarak tutulur
    public string PhoneNumber { get; private set; }

    public bool IsEmailVerified { get; private set; }
    public bool IsPhoneVerified { get; private set; }
    public bool IsKycVerified { get; private set; }

    public UserStatus Status { get; private set; }            // Active, Blocked
    public DateTime CreatedAt { get; private set; }

    private User() { } // EF için

    public User(string fullName, string email, string passwordHash, string phoneNumber)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;

        IsEmailVerified = false;
        IsPhoneVerified = false;
        IsKycVerified = false;
        Status = UserStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void Block() => Status = UserStatus.Blocked;
    public void VerifyEmail() => IsEmailVerified = true;
    public void VerifyPhone() => IsPhoneVerified = true;
    public void VerifyKyc() => IsKycVerified = true;
}