using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletManagement.Domain.Entities;

namespace WalletManagement.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => t.ExternalId).IsUnique();

        // Index pour les requêtes de reporting fréquentes
        builder.HasIndex(t => new { t.AccountId, t.TransactionDate });
        builder.HasIndex(t => new { t.AccountId, t.Type, t.Status });

        builder.Property(t => t.Amount)
            .HasColumnType("decimal(19,4)")
            .IsRequired();

        builder.Property(t => t.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Note)
            .HasMaxLength(500);

        builder.Property(t => t.TransactionDate)
            .IsRequired();

        // Relation vers Category (nullable — une transaction peut ne pas avoir de catégorie)
        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);

        builder.ToTable("Transactions");
    }
}
