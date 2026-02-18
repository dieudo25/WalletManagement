using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WalletManagement.Domain.Common;
using WalletManagement.Domain.Entities;

namespace WalletManagement.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Budget> Budgets => Set<Budget>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var entities = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // On clear avant le dispatch pour éviter les boucles infinies
        entities.ForEach(e => e.ClearDomainEvents());

        // En MVP : on loggue simplement les events
        // En V2 : on injectera IPublisher de MediatR ici
        foreach (var domainEvent in domainEvents)
        {
            Console.WriteLine($"[DomainEvent] {domainEvent.GetType().Name} raised at {domainEvent.OccurredOn}");
        }

        await Task.CompletedTask;
    }
}
