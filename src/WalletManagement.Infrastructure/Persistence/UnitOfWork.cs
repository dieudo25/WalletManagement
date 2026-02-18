using Microsoft.EntityFrameworkCore.Storage;
using WalletManagement.Application.Common.Interfaces;

namespace WalletManagement.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
        => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task ExecuteInTransactionAsync(
        Func<Task> action,
        CancellationToken cancellationToken = default)
    {
        IDbContextTransaction? transaction = null;

        try
        {
            transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            await action();
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            if (transaction != null)
                await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (transaction != null)
                await transaction.DisposeAsync();
        }
    }
}
