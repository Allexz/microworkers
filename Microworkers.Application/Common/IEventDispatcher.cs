namespace Microworkers.Application.Common;
public interface IEventDispatcher
{
    Task Dispatch(DomainEvent @event);
}
