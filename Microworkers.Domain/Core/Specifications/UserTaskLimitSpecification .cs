using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Enums;
using Microworkers.Domain.Core.Interfaces;

namespace Microworkers.Domain.Core.Specifications;
public class UserTaskLimitSpecification : ISpecification<User>
{
    public string ErrorMessage => "User cannot have more than 3 tasks in progress";
    public bool IsSatisfiedBy(User user)
    {
        return user.Taskis.Count(t => t.Status == TaskiStatus.InProgress) < 3;
    }
}
