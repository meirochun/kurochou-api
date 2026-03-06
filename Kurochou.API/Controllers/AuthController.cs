using Kurochou.App.DTO.User.Request;
using Kurochou.App.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kurochou.API.Controllers;

public class AuthController(IAuthenticationService authService) : KuroController
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.Login(request, cancellationToken);

        if (!result.Success)
        {
            return Failure("Invalid username or password", 401);
        }

        return Response(result);
    }

    [HttpDelete("logout")]
    [AllowAnonymous]
    public async Task<IResult> LogoutAsync(CancellationToken cancellationToken)
    {
        await authService.Logout(cancellationToken);
        return NoContent();
    }
}