using Kurochou.Domain.DTO.User;
using Kurochou.Domain.Model;

namespace Kurochou.Domain.Interface.Service;

public interface IUserService
{
        Task<IEnumerable<User?>> GetUsersAsync(GetUserRequest request, CancellationToken cancellationToken);
}