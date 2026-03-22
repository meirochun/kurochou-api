using Kurochou.App.DTO;
using Kurochou.App.DTO.User.Request;
using Kurochou.Domain.DTO;

namespace Kurochou.App.Interfaces.Service;

public interface IUserService
{
    Task<Result<IEnumerable<UserWithClipCount?>>> GetUsersAsync(GetUserRequest request, CancellationToken cancellationToken);
    Task<Result<Guid>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
    Task<Result<Guid>> UpdateUserAsync(Guid id, CreateUserRequest request, CancellationToken cancellationToken);
    Task<Result<Guid>> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken);
}