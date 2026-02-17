using WalletManagement.Domain.Common;
using WalletManagement.Domain.Enums;

namespace WalletManagement.Domain.Entities;

public class Budget : BaseEntity
{
    public int UserId { get; private set; }
    public int CategoryId { get; private set; }
    public decimal TargetAmount { get; private set; }
    public BudgetPeriod Period { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset? EndDate { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation
    public Category Category { get; private set; } = null!;

    // EF Core
    protected Budget() { }

    public static Budget Create(
        int userId,
        int categoryId,
        decimal targetAmount,
        BudgetPeriod period,
        DateTimeOffset startDate,
        DateTimeOffset? endDate = null)
    {
        if (targetAmount <= 0)
            throw new ArgumentException("Budget target must be greater than zero.", nameof(targetAmount));

        if (endDate.HasValue && endDate <= startDate)
            throw new ArgumentException("End date must be after start date.", nameof(endDate));

        return new Budget
        {
            UserId = userId,
            CategoryId = categoryId,
            TargetAmount = targetAmount,
            Period = period,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = true
        };
    }

    public void UpdateTarget(decimal newAmount)
    {
        if (newAmount <= 0)
            throw new ArgumentException("Budget target must be greater than zero.", nameof(newAmount));

        TargetAmount = newAmount;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }
}