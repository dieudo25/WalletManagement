using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletManagement.Domain.Entities;

namespace WalletManagement.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasIndex(a => a.ExternalId).IsUnique();

        // Index composé pour les requêtes fréquentes
        builder.HasIndex(a => new { a.UserId, a.Status });

        builder.Property(a => a.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(500);

        // Enum → string en base (lisible, pas de magic numbers)
        builder.Property(a => a.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        // Value Object Currency → colonne simple
        builder.OwnsOne(a => a.Currency, currency =>
        {
            currency.Property(c => c.Code)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        // decimal(19,4) pour les montants financiers
        builder.Property(a => a.Balance)
            .HasColumnType("decimal(19,4)")
            .IsRequired();

        // Relation User → Account
        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);  // Jamais de cascade delete sur les comptes

        // Relation Account → Transactions
        builder.HasMany(a => a.Transactions)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Accounts");
    }
}
