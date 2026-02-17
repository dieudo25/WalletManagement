using System.Text.RegularExpressions;
using WalletManagement.Domain.Common;

namespace WalletManagement.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    private static readonly Regex _emailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Of(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        var normalized = email.Trim().ToLowerInvariant();

        if (!_emailRegex.IsMatch(normalized))
            throw new ArgumentException($"'{email}' is not a valid email address.", nameof(email));

        return new Email(normalized);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}