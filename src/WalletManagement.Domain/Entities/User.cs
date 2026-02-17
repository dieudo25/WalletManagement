using WalletManagement.Domain.Common;
using WalletManagement.Domain.Events.Users;
using WalletManagement.Domain.ValueObjects;

namespace WalletManagement.Domain.Entities;

public class User : BaseEntity
{
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public bool IsEmailVerified { get; private set; }
    public string? EmailVerificationToken { get; private set; }
    public DateTimeOffset? EmailVerifiedAt { get; private set; }

    // EF Core
    protected User() { }

    public static User Create(Email email, string passwordHash, string firstName, string lastName)
    {
        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            IsEmailVerified = false,
            EmailVerificationToken = Guid.CreateVersion7().ToString("N")
        };

        user.RaiseDomainEvent(new UserCreatedEvent(user.ExternalId, email.Value));

        return user;
    }

    public void VerifyEmail(string token)
    {
        if (IsEmailVerified) return;

        if (EmailVerificationToken != token)
            throw new InvalidOperationException("Invalid verification token.");

        IsEmailVerified = true;
        EmailVerificationToken = null;
        EmailVerifiedAt = DateTimeOffset.UtcNow;
        SetUpdatedAt();

        RaiseDomainEvent(new UserEmailVerifiedEvent(ExternalId));
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        SetUpdatedAt();
    }

    public string FullName => $"{FirstName} {LastName}";
}