using Microsoft.EntityFrameworkCore;
using WalletManagement.Application.Common.Interfaces;
using WalletManagement.Domain.Entities;

namespace WalletManagement.Infrastructure.Persistence.Repositories;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Account>> GetByUserIdAsync(
        int userId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.Name)
            .ToListAsync(cancellationToken);

    public async Task<Account?> GetByExternalIdWithTransactionsAsync(
        Guid externalId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Include(a => a.Transactions)
            .FirstOrDefaultAsync(a => a.ExternalId == externalId, cancellationToken);

    public async Task<bool> ExistsAsync(
        int userId, string name, CancellationToken cancellationToken = default)
        => await _dbSet
            .AnyAsync(a => a.UserId == userId && a.Name == name, cancellationToken);
}
