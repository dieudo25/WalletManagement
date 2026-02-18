using Microsoft.EntityFrameworkCore;
using WalletManagement.Application.Common.Interfaces;
using WalletManagement.Domain.Entities;
using WalletManagement.Domain.Enums;

namespace WalletManagement.Infrastructure.Persistence.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(
        int accountId,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        TransactionType? type = null,
        int page = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Where(t => t.AccountId == accountId && t.Status != TransactionStatus.Cancelled)
            .AsQueryable();

        if (from.HasValue)
            query = query.Where(t => t.TransactionDate >= from.Value);

        if (to.HasValue)
            query = query.Where(t => t.TransactionDate <= to.Value);

        if (type.HasValue)
            query = query.Where(t => t.Type == type.Value);

        return await query
            .OrderByDescending(t => t.TransactionDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(t => t.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetSumByAccountIdAsync(
        int accountId,
        TransactionType type,
        DateTimeOffset from,
        DateTimeOffset to,
        CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(t => t.AccountId == accountId
                     && t.Type == type
                     && t.Status == TransactionStatus.Cleared
                     && t.TransactionDate >= from
                     && t.TransactionDate <= to)
            .SumAsync(t => t.Amount, cancellationToken);
}
