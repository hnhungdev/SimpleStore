using MediatR;

namespace BuildingBlocks.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }

    DateTimeOffset OccurredAt { get; }

    string EventType { get; }
}