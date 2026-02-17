using WalletManagement.Domain.Common;

namespace WalletManagement.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    private Money(decimal amount, Currency currency)
    {
        Amount = decimal.Round(amount, 4, MidpointRounding.AwayFromZero);
        Currency = currency;
    }

    public static Money Of(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new ArgumentException("Money amount cannot be negative.", nameof(amount));

        return new Money(amount, currency);
    }

    public static Money Of(decimal amount, string currencyCode)
        => Of(amount, Currency.Of(currencyCode));

    public static Money Zero(Currency currency) => new(0, currency);

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount - other.Amount, Currency);
    }

    public bool IsGreaterThan(Money other)
    {
        EnsureSameCurrency(other);
        return Amount > other.Amount;
    }

    public bool IsZero() => Amount == 0;

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException(
                $"Cannot operate on Money with different currencies: {Currency} vs {other.Currency}");
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:0.00} {Currency}";
}