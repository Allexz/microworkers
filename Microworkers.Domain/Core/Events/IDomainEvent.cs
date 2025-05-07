using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microworkers.Domain.Core.Events;
public interface IDomainEvent
{
    DateTime OccurredOn { get; }  // Timestamp do evento
    string EventType { get; }
}
