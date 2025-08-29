using Kurochou.Domain.Model;

namespace Kurochou.Domain.Interface.Repository;

public interface IUserRepository : IRepository<User>
{
        Task<User?> GetByUsernameAsync(string username);
        Task<IEnumerable<User?>> GetUsersAsync(string search, CancellationToken cancellationToken);
}