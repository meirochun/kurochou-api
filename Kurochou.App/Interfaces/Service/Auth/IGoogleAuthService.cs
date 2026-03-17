using Kurochou.App.DTO;
using Kurochou.App.DTO.User.Response;

namespace Kurochou.App.Interfaces.Service.Auth;

public interface IGoogleAuthService
{
    Task<Result<AuthResponseDTO>> GetJwtToken(CancellationToken ct);
}
