using WalletManagement.Domain.Common;

namespace WalletManagement.Domain.Events.Users;

public record UserCreatedEvent(Guid UserId, string Email) : DomainEvent;
public record UserEmailVerifiedEvent(Guid UserId) : DomainEvent;