using Microworkers.Domain.Core.Exceptions;

namespace Microworkers.Domain.Core.ValueObjects;
public readonly record struct TaskiUpdate
{
    public Guid Id { get; }
    public Guid TaskiId { get; }
    public Guid UserId { get; }
    public string Message { get; }
    public DateTime Timestamp { get; }

    public TaskiUpdate(Guid taskiId,Guid userId, string message)
    {
        Id = Guid.NewGuid();
        TaskiId = taskiId != Guid.Empty ? taskiId: throw new DomainException("Task should be informed", nameof(taskiId));
        UserId = userId != Guid.Empty ? userId : throw new ArgumentNullException(nameof(userId));
        Message = ValidateMessage(message);
        Timestamp = DateTime.UtcNow;
    }

    private string ValidateMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new DomainException("Message cannot be empty", nameof(message));

        if (message.Length > 255)
            throw new DomainException("Message cannot exceed 255 characters", nameof(message));

        return message;
    }
}
