using WalletManagement.Domain.Common;
using WalletManagement.Domain.Enums;
using WalletManagement.Domain.Exceptions;

namespace WalletManagement.Domain.Entities;

public class Category : BaseEntity
{
    public int? UserId { get; private set; }          // null = catégorie système
    public string Name { get; private set; } = null!;
    public CategoryType Type { get; private set; }
    public int? ParentCategoryId { get; private set; }
    public bool IsSystem { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation
    public Category? ParentCategory { get; private set; }
    private readonly List<Category> _subCategories = [];
    public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();

    // EF Core
    protected Category() { }

    public static Category CreateSystem(string name, CategoryType type, int? parentId = null)
        => new()
        {
            Name = name.Trim(),
            Type = type,
            ParentCategoryId = parentId,
            IsSystem = true,
            IsDeleted = false
        };

    public static Category CreateUserDefined(
        int userId, string name, CategoryType type, int? parentId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        return new Category
        {
            UserId = userId,
            Name = name.Trim(),
            Type = type,
            ParentCategoryId = parentId,
            IsSystem = false,
            IsDeleted = false
        };
    }

    public void SoftDelete()
    {
        if (IsSystem)
            throw new InvalidOperationException("System categories cannot be deleted.");

        IsDeleted = true;
        SetUpdatedAt();
    }

    public void Update(string name)
    {
        if (IsSystem)
            throw new InvalidOperationException("System categories cannot be modified.");

        Name = name.Trim();
        SetUpdatedAt();
    }

    public bool IsUserOwned => UserId.HasValue;
}