namespace WalletManagement.Domain.Common;

public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.CreateVersion7();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}