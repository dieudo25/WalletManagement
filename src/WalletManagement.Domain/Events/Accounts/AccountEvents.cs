using WalletManagement.Domain.Common;

namespace WalletManagement.Domain.Events.Accounts;

public record AccountCreatedEvent(Guid AccountId, int UserId, string Currency) : DomainEvent;
public record AccountArchivedEvent(Guid AccountId) : DomainEvent;