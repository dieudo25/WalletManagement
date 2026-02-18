using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletManagement.Domain.Entities;
using WalletManagement.Domain.ValueObjects;

namespace WalletManagement.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.ExternalId).IsUnique();

        builder.Property(u => u.ExternalId)
            .IsRequired();

        // Value Object Email → colonne simple
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(256)
                .IsRequired();

            email.HasIndex(e => e.Value).IsUnique();
        });

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.EmailVerificationToken)
            .HasMaxLength(64);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.ToTable("Users");
    }
}
