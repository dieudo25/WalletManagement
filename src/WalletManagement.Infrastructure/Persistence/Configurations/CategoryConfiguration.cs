using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletManagement.Domain.Entities;

namespace WalletManagement.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasIndex(c => c.ExternalId).IsUnique();
        builder.HasIndex(c => new { c.UserId, c.IsDeleted });

        builder.Property(c => c.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        // Self-referencing : catégorie parent → sous-catégories
        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Filtre global : les soft-deleted sont invisibles par défaut
        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.ToTable("Categories");
    }
}
