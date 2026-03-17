using Kurochou.App.DTO;
using Kurochou.App.DTO.User.Response;
using Kurochou.App.Interfaces.Service;
using Kurochou.App.Interfaces.Service.Auth;
using Kurochou.Domain.Entities;
using Kurochou.Domain.Enum;
using Kurochou.Domain.Interface.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Kurochou.App.Service.Auth;

public class GoogleAuthService(
    ITokenService tokenService,
    IUserRepository userRepository,
    IHttpContextAccessor accessor
    ) : 
    IGoogleAuthService
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IHttpContextAccessor _accessor = accessor;

    public async Task<Result<AuthResponseDTO>> GetJwtToken(CancellationToken ct)
    {
        var result = await _accessor.HttpContext!.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var claims = result.Principal?.Claims;
        var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (email is null || googleId is null)
            return Result<AuthResponseDTO>.Fail("Failed to integrate with Google");

        var user = await _userRepository.GetByGoogleIdAsync(googleId, ct);
        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                GoogleId = googleId,
                Username = email.Split("@")[0],
                CreatedAt = DateTime.Now,
                Role = UserRole.User
            };

            var inserted = await _userRepository.InsertAsync(user, ct);
            if (inserted != 1)
                return Result<AuthResponseDTO>.Fail("Failed to create user");
        }

        var token = _tokenService.GenerateToken(user);

        return Result<AuthResponseDTO>.Ok(new AuthResponseDTO(token, 60, user.Role.ToString()));
    }
}
