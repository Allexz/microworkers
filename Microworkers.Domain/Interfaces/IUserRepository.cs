using Microworkers.Domain.Core.Aggregates;

namespace Microworkers.Domain.Interfaces;
public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> UpdateAsync(User user);  
}
