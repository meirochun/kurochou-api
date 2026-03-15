using Kurochou.App.DTO;
using Kurochou.App.DTO.User.Response;
using System.Security.Claims;

namespace Kurochou.App.Interfaces.Service.Auth;

public interface IGoogleAuthService
{
    Task<Result<AuthResponseDTO>> GetJwtToken(ClaimsPrincipal cp, CancellationToken ct);
}
