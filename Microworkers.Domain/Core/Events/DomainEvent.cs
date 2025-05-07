using Microworkers.Domain.Core.Events;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow; // Timestamp automático
    public abstract string EventType { get; }              // Nome do evento (ex: "UserCreated")
    public Guid EventId { get; } = Guid.NewGuid();         // Identificador único do evento
}