namespace WalletManagement.Domain.Exceptions;

public static class DomainExceptions
{
    public static class Account
    {
        public static DomainException NotFound(Guid id)
            => new("ACCOUNT_NOT_FOUND", $"Account '{id}' was not found.");

        public static DomainException Archived()
            => new("ACCOUNT_ARCHIVED", "Cannot perform operations on an archived account.");

        public static DomainException InsufficientFunds(decimal balance, decimal amount)
            => new("ACCOUNT_INSUFFICIENT_FUNDS",
                $"Insufficient funds. Balance: {balance:C}, Required: {amount:C}");

        public static DomainException CurrencyMismatch(string source, string target)
            => new("ACCOUNT_CURRENCY_MISMATCH",
                $"Cannot transfer between accounts with different currencies: {source} → {target}");
    }

    public static class Transaction
    {
        public static DomainException NotFound(Guid id)
            => new("TRANSACTION_NOT_FOUND", $"Transaction '{id}' was not found.");

        public static DomainException AlreadyCancelled()
            => new("TRANSACTION_ALREADY_CANCELLED", "Transaction is already cancelled.");

        public static DomainException InvalidAmount()
            => new("TRANSACTION_INVALID_AMOUNT", "Transaction amount must be greater than zero.");
    }

    public static class Category
    {
        public static DomainException NotFound(Guid id)
            => new("CATEGORY_NOT_FOUND", $"Category '{id}' was not found.");

        public static DomainException TypeMismatch(string categoryType, string transactionType)
            => new("CATEGORY_TYPE_MISMATCH",
                $"Cannot use a '{categoryType}' category for a '{transactionType}' transaction.");

        public static DomainException CannotDeleteWithTransactions()
            => new("CATEGORY_HAS_TRANSACTIONS",
                "Cannot delete a category that has associated transactions.");
    }

    public static class User
    {
        public static DomainException NotFound(Guid id)
            => new("USER_NOT_FOUND", $"User '{id}' was not found.");

        public static DomainException EmailAlreadyExists(string email)
            => new("USER_EMAIL_EXISTS", $"A user with email '{email}' already exists.");

        public static DomainException InvalidCredentials()
            => new("USER_INVALID_CREDENTIALS", "Invalid email or password.");

        public static DomainException EmailNotVerified()
            => new("USER_EMAIL_NOT_VERIFIED", "Email address must be verified before logging in.");
    }
}