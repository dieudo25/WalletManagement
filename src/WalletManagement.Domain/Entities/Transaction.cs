using WalletManagement.Domain.Common;
using WalletManagement.Domain.Enums;
using WalletManagement.Domain.Events.Transactions;
using WalletManagement.Domain.Exceptions;
using static WalletManagement.Domain.Exceptions.DomainExceptions;

namespace WalletManagement.Domain.Entities;

public class Transaction : BaseEntity
{
    public int AccountId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public TransactionStatus Status { get; private set; }
    public int? CategoryId { get; private set; }
    public DateTimeOffset TransactionDate { get; private set; }
    public string? Note { get; private set; }

    // Pour les transfers : lie les deux côtés de l'opération
    public int? LinkedTransactionId { get; private set; }

    // Navigation
    public Account Account { get; private set; } = null!;
    public Category? Category { get; private set; }

    // EF Core
    protected Transaction() { }

    public static Transaction Create(
        int accountId,
        decimal amount,
        TransactionType type,
        DateTimeOffset transactionDate,
        int? categoryId = null,
        string? note = null)
    {
        if (amount <= 0)
            throw DomainExceptions.Transaction.InvalidAmount();

        var transaction = new Transaction
        {
            AccountId = accountId,
            Amount = amount,
            Type = type,
            Status = TransactionStatus.Cleared,
            CategoryId = categoryId,
            TransactionDate = transactionDate,
            Note = note?.Trim()
        };

        transaction.RaiseDomainEvent(
            new TransactionCreatedEvent(transaction.ExternalId, accountId, amount, type));

        return transaction;
    }

    public void Cancel()
    {
        if (Status == TransactionStatus.Cancelled)
            throw DomainExceptions.Transaction.AlreadyCancelled();

        Status = TransactionStatus.Cancelled;
        SetUpdatedAt();

        RaiseDomainEvent(new TransactionCancelledEvent(ExternalId, AccountId, Amount, Type));
    }

    public void LinkTo(int linkedTransactionId)
    {
        LinkedTransactionId = linkedTransactionId;
        SetUpdatedAt();
    }

    public bool IsTransfer => Type == TransactionType.Transfer;
    public bool IsCancelled => Status == TransactionStatus.Cancelled;
}