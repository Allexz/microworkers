using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.Exceptions;
using Microworkers.Domain.Core.ValueObjects;
using Microworkers.Domain.Shared;

namespace Microworkers.Domain.Core.Aggregates;
public class Taski : AggregateRoot
{
    internal  Taski(
    Guid id,
    Guid customer,
    Guid serviceProvider,
    Guid requiredSkill,
    DateTime requestDate,
    TaskDescription description,
    TaskiStatus status)
    {
        Id = id;
        Customer = customer;
        ServiceProvider = serviceProvider;
        RequiredSkill = requiredSkill;
        RequestDate = requestDate;
        Description = description;
        Status = status;
    }

    public Guid Id { get; private set; }
    public Guid Customer { get; private set; }
    public Guid ServiceProvider { get; private set; }
    public Guid RequiredSkill { get; private set; }
    public DateTime RequestDate { get; private set; }
    public TaskDescription Description { get; private set; } // Value object
    public TaskiStatus Status { get; private set; }
    public TaskiResult Result { get; private set; }
    public DateTime? CompletionDate { get; private set; }

    private readonly List<TaskiUpdate> _updates = new();
    public IReadOnlyCollection<TaskiUpdate> Updates => _updates.AsReadOnly();
    private Taski() { }

    #region Comportamentos do domínio
    public void StartTask()
    {
        if (Status != TaskiStatus.Opened)
            throw new DomainException("Task can only be started from Opened status", nameof(TaskiStatus));

        Status = TaskiStatus.InProgress;
        AddUpdate("Task started by service provider");
    }

    public void CompleteTask(TaskiResult result, string completionNotes)
    {
        if (Status != TaskiStatus.InProgress)
            throw new DomainException("Task must be in progress to be completed", nameof(TaskiResult));

        Status = TaskiStatus.Completed;
        Result = result;
        CompletionDate = DateTime.UtcNow;
        AddUpdate($"Task completed with result: {result}. Notes: {completionNotes}");
    }
    public void CancelTask(string reason)
    {
        if (Status == TaskiStatus.Completed || Status == TaskiStatus.Cancelled)
            throw new DomainException("Cannot cancel already completed or cancelled task", nameof(reason));

        Status = TaskiStatus.Cancelled;
        AddUpdate($"Task cancelled. Reason: {reason}");
    }
  
    private void AddUpdate(string message)
    {
        //_updates.Add(new TaskiUpdate(Id, message));
    }
    public static Result<bool> EnsureCustomerCanCreateTask(Guid customerId, IEnumerable<Taski> existingTasks)
    {
        int inProgress = existingTasks.Count(t => t.Customer == customerId && t.Status == TaskiStatus.InProgress);

        if (inProgress == 3)
            Result<Taski>.Fail("Customer cannot have more than 3 tasks in progress");

        return Result<Taski>.Ok<bool>(true);
    }

    #endregion

}
