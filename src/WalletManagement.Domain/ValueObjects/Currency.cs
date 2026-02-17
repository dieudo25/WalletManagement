using WalletManagement.Domain.Common;

namespace WalletManagement.Domain.ValueObjects;

public sealed class Currency : ValueObject
{
    public static readonly Currency EUR = new("EUR");
    public static readonly Currency USD = new("USD");
    public static readonly Currency GBP = new("GBP");
    public static readonly Currency CAD = new("CAD");
    public static readonly Currency CHF = new("CHF");

    private static readonly HashSet<string> _supportedCodes =
    [
        "EUR", "USD", "GBP", "CAD", "CHF"
    ];

    public string Code { get; }

    private Currency(string code) => Code = code;

    public static Currency Of(string code)
    {
        var upper = code?.Trim().ToUpperInvariant()
            ?? throw new ArgumentNullException(nameof(code));

        if (!_supportedCodes.Contains(upper))
            throw new ArgumentException($"Currency '{code}' is not supported.", nameof(code));

        return new Currency(upper);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Code;
    }

    public override string ToString() => Code;
}