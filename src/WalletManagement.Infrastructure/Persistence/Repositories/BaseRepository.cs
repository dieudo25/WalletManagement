using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WalletManagement.Application.Common.Interfaces;
using WalletManagement.Domain.Common;

namespace WalletManagement.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync([id], cancellationToken);

    public async Task<TEntity?> GetByExternalIdAsync(Guid externalId, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(e => e.ExternalId == externalId, cancellationToken);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbSet.ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await _dbSet.Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public void Update(TEntity entity)
        => _dbSet.Update(entity);

    public void Remove(TEntity entity)
        => _dbSet.Remove(entity);
}
