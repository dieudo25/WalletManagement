using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletManagement.Domain.Entities;

namespace WalletManagement.Infrastructure.Persistence.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.Id);

        builder.HasIndex(b => b.ExternalId).IsUnique();
        builder.HasIndex(b => new { b.UserId, b.IsActive });

        builder.Property(b => b.TargetAmount)
            .HasColumnType("decimal(19,4)")
            .IsRequired();

        builder.Property(b => b.Period)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Budgets");
    }
}
