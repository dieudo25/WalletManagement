using WalletManagement.Domain.Entities;
using WalletManagement.Domain.Enums;

namespace WalletManagement.Application.Common.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(
        int accountId,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        TransactionType? type = null,
        int page = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default);

    Task<decimal> GetSumByAccountIdAsync(
        int accountId,
        TransactionType type,
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken = default);
}
