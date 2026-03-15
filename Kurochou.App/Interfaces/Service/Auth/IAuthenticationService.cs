using Kurochou.App.DTO;
using Kurochou.App.DTO.User.Request;
using Kurochou.App.DTO.User.Response;

namespace Kurochou.App.Interfaces.Service.Auth;

public interface IAuthenticationService
{
    Task<Result<AuthResponseDTO?>> Login(LoginRequest request, CancellationToken cancellationToken);
    Guid GetUserId();
    Task Logout(CancellationToken cancellationToken);
}