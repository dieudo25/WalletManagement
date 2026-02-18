using WalletManagement.Domain.Enums;

namespace WalletManagement.Infrastructure.Persistence.Seeds;

public static class CategorySeed
{
    public static IEnumerable<object> GetCategories()
    {
        var categories = new List<object>();
        int id = 1;

        // === EXPENSE CATEGORIES ===
        var housing = Add(id++, "Housing", CategoryType.Expense, null);
        var food = Add(id++, "Food", CategoryType.Expense, null);
        var transport = Add(id++, "Transport", CategoryType.Expense, null);
        var health = Add(id++, "Health", CategoryType.Expense, null);
        var leisure = Add(id++, "Leisure", CategoryType.Expense, null);
        var shopping = Add(id++, "Shopping", CategoryType.Expense, null);
        var education = Add(id++, "Education", CategoryType.Expense, null);
        var other_exp = Add(id++, "Other", CategoryType.Expense, null);

        categories.AddRange([housing, food, transport, health, leisure, shopping, education, other_exp]);

        // Sub-categories Expense
        categories.AddRange([
            Add(id++, "Rent / Mortgage",  CategoryType.Expense, 1),
            Add(id++, "Electricity",      CategoryType.Expense, 1),
            Add(id++, "Internet",         CategoryType.Expense, 1),
            Add(id++, "Groceries",        CategoryType.Expense, 2),
            Add(id++, "Restaurants",      CategoryType.Expense, 2),
            Add(id++, "Takeaway",         CategoryType.Expense, 2),
            Add(id++, "Fuel",             CategoryType.Expense, 3),
            Add(id++, "Public Transport", CategoryType.Expense, 3),
            Add(id++, "Car Insurance",    CategoryType.Expense, 3),
            Add(id++, "Doctor",           CategoryType.Expense, 4),
            Add(id++, "Pharmacy",         CategoryType.Expense, 4),
            Add(id++, "Gym",              CategoryType.Expense, 5),
            Add(id++, "Streaming",        CategoryType.Expense, 5),
            Add(id++, "Hobbies",          CategoryType.Expense, 5),
            Add(id++, "Clothes",          CategoryType.Expense, 6),
            Add(id++, "Electronics",      CategoryType.Expense, 6),
        ]);

        // === INCOME CATEGORIES ===
        categories.AddRange([
            Add(id++, "Salary",           CategoryType.Income, null),
            Add(id++, "Freelance",        CategoryType.Income, null),
            Add(id++, "Investments",      CategoryType.Income, null),
            Add(id++, "Rental Income",    CategoryType.Income, null),
            Add(id++, "Other Income",     CategoryType.Income, null),
        ]);

        return categories;
    }

    private static object Add(int id, string name, CategoryType type, int? parentId) => new
    {
        Id = id,
        ExternalId = Guid.NewGuid(),
        Name = name,
        Type = type.ToString(),
        ParentCategoryId = parentId,
        IsSystem = true,
        IsDeleted = false,
        UserId = (int?)null,
        CreatedAt = DateTimeOffset.UtcNow,
        UpdatedAt = (DateTimeOffset?)null
    };
}
