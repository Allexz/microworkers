using MediatR;
using Microsoft.EntityFrameworkCore;
using Microworkers.Application.Common;
using Microworkers.Infrastructure.Data;

namespace Microworkers.Application.Audit;
public record GetEventsByAggregateIdQuery(Guid AggregateId) : IRequest<List<DomainEvent>>;

public class GetEventsByAggregateIdHandler
    : IRequestHandler<GetEventsByAggregateIdQuery, List<DomainEvent>>
{
    private readonly EventStoreContext _context;
    private readonly ISerializer _serializer;

    public GetEventsByAggregateIdHandler(EventStoreContext context, ISerializer serializer)
    {
        _context = context;
        _serializer = serializer;
    }

    public async Task<List<DomainEvent>> Handle(
        GetEventsByAggregateIdQuery request,
        CancellationToken cancellationToken)
    {
        var storedEvents = await _context.Events
            .Where(e => e.AggregateId == request.AggregateId.ToString())
            .OrderBy(e => e.OccurredOn)
            .ToListAsync(cancellationToken);

        var events = new List<DomainEvent>();

        //foreach (var e in storedEvents)
        //{
        //    var eventType = Type.GetType($"Microworkers.Domain.Core.Events.{e.EventType}");
        //    var domainEvent = (DomainEvent)_serializer.Deserialize((eventType.DeclaringType)e.Payload);
        //    events.Add(domainEvent);
        //}

        return events;
    }
}
