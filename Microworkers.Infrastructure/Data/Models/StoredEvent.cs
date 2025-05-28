namespace Microworkers.Infrastructure.Data.Models;
public class StoredEvent
{
    public Guid Id { get; set; }
    public string EventType { get; set; }
    public string Payload { get; set; }
    public DateTime OccurredOn { get; set; }
    public string AggregateId { get; set; } // ID do agregado (ex: UserId)
}
