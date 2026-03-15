using Kurochou.App.DTO.Auth.GoogleAuth;
using Kurochou.App.Interfaces.Service.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kurochou.API.Controllers.Auth;

public class GoogleAuthController(IGoogleAuthService googleAuthService) : KuroController
{
    private readonly IGoogleAuthService _googleAuthService = googleAuthService;

    [HttpGet("google-auth-request")]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleLogin([FromQuery] GoogleAuthRequest auth)
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleResponse), new { auth.ReturnUrl })
        };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-auth-response")]
    [AllowAnonymous]
    public async Task<IResult> GoogleResponse(CancellationToken cancellationToken)
    {
        var response = await _googleAuthService.GetJwtToken(User, cancellationToken);
        return Response(response);
    }
}
