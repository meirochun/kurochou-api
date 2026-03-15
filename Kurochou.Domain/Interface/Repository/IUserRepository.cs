using Kurochou.Domain.Entities;

namespace Kurochou.Domain.Interface.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken cancellation);
    Task<IEnumerable<User?>> GetUsersAsync(string? username, CancellationToken cancellationToken);
}