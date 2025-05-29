using Microworkers.Domain.Core.Aggregates;
using Microworkers.Domain.Core.Interfaces;
using Microworkers.Domain.Core.Specifications;
using Microworkers.Domain.Interfaces;
using Microworkers.Domain.Shared;

namespace Microworkers.Application.Taskis.Services;
public class TaskiAssignmentService
{
    private readonly IUserRepository _userRepository;
    private readonly ISpecification<User> _taskLimitSpec;

    public TaskiAssignmentService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _taskLimitSpec = new UserTaskLimitSpecification();
    }

    public async Task<Result> AssignTaskToUser(Guid userId, Taski taski)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) return Result.Fail("User not found");

        // Uso direto da especificação (opcional)
        if (!_taskLimitSpec.IsSatisfiedBy(user))
        {
            return Result.Fail(_taskLimitSpec.ErrorMessage);
        }

        // Ou via método do agregado
        var result = user.AddTaski(taski);
        if (result.IsFailure) return result;

        await _userRepository.UpdateAsync(user);
        return Result.Ok();
    }
}
