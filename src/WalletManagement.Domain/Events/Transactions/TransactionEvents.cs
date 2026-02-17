using WalletManagement.Domain.Common;
using WalletManagement.Domain.Enums;

namespace WalletManagement.Domain.Events.Transactions;

public record TransactionCreatedEvent(
    Guid TransactionId,
    int AccountId,
    decimal Amount,
    TransactionType Type) : DomainEvent;

public record TransactionCancelledEvent(
    Guid TransactionId,
    int AccountId,
    decimal Amount,
    TransactionType Type) : DomainEvent;
