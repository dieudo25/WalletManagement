using WalletManagement.Domain.Entities;

namespace WalletManagement.Application.Common.Interfaces;

public interface IAccountRepository : IRepository<Account>
{
    Task<IReadOnlyList<Account>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Account?> GetByExternalIdWithTransactionsAsync(Guid externalId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int userId, string name, CancellationToken cancellationToken = default);
}
