using Domain.Models;
using Shared.Kernel.Repositories;

namespace Domain.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<List<User>> GetUsersByIds(IEnumerable<Guid> ids);
}