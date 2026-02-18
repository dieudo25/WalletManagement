using Microsoft.EntityFrameworkCore;
using WalletManagement.Application.Common.Interfaces;
using WalletManagement.Domain.Entities;

namespace WalletManagement.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _dbSet
            .FirstOrDefaultAsync(u => u.Email.Value == email.ToLowerInvariant(), cancellationToken);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => await _dbSet
            .AnyAsync(u => u.Email.Value == email.ToLowerInvariant(), cancellationToken);
}
