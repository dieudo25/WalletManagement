using WalletManagement.Domain.Common;
using WalletManagement.Domain.Enums;
using WalletManagement.Domain.Events.Accounts;
using WalletManagement.Domain.Exceptions;
using WalletManagement.Domain.ValueObjects;

namespace WalletManagement.Domain.Entities;

public class Account : BaseEntity
{
    public int UserId { get; private set; }
    public string Name { get; private set; } = null!;
    public AccountType Type { get; private set; }
    public Currency Currency { get; private set; } = null!;
    public decimal Balance { get; private set; }
    public AccountStatus Status { get; private set; }
    public string? Description { get; private set; }

    // Navigation
    public User User { get; private set; } = null!;
    private readonly List<Transaction> _transactions = [];
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    // EF Core
    protected Account() { }

    public static Account Create(
        int userId,
        string name,
        AccountType type,
        Currency currency,
        decimal initialBalance = 0,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Account name cannot be empty.", nameof(name));

        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.", nameof(initialBalance));

        var account = new Account
        {
            UserId = userId,
            Name = name.Trim(),
            Type = type,
            Currency = currency,
            Balance = initialBalance,
            Status = AccountStatus.Active,
            Description = description?.Trim()
        };

        account.RaiseDomainEvent(new AccountCreatedEvent(account.ExternalId, userId, currency.Code));

        return account;
    }

    public void Credit(decimal amount)
    {
        EnsureActive();
        if (amount <= 0) throw DomainExceptions.Transaction.InvalidAmount();

        Balance += amount;
        SetUpdatedAt();
    }

    public void Debit(decimal amount)
    {
        EnsureActive();
        if (amount <= 0) throw DomainExceptions.Transaction.InvalidAmount();

        Balance -= amount;
        SetUpdatedAt();
    }

    public void Archive()
    {
        if (Status == AccountStatus.Archived) return;

        Status = AccountStatus.Archived;
        SetUpdatedAt();

        RaiseDomainEvent(new AccountArchivedEvent(ExternalId));
    }

    public void UpdateDetails(string name, string? description)
    {
        EnsureActive();
        Name = name.Trim();
        Description = description?.Trim();
        SetUpdatedAt();
    }

    private void EnsureActive()
    {
        if (Status == AccountStatus.Archived)
            throw DomainExceptions.Account.Archived();
    }
}