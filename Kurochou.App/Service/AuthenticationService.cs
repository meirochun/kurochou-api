using Kurochou.App.DTO;
using Kurochou.App.DTO.User.Request;
using Kurochou.App.DTO.User.Response;
using Kurochou.App.Helper;
using Kurochou.App.Interfaces.Service;
using Kurochou.Domain.Interface.Repository;
using Microsoft.AspNetCore.Http;

namespace Kurochou.App.Service;

public class AuthenticationService(
        ITokenService tokenService,
        IUserRepository repository,
        IHttpContextAccessor httpContextAccessor) : IAuthenticationService
{
    public async Task<Result<AuthResponseDTO?>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByUsernameAsync(request.Username, cancellationToken);

        if (user is null)
            return Result<AuthResponseDTO?>.Fail("Username or Password invalid");

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
            return Result<AuthResponseDTO?>.Fail("Username or Password invalid");

        var token = tokenService.GenerateToken(user);

        var response = new AuthResponseDTO(token, 60, user.Role.ToString());

        return Result<AuthResponseDTO?>.Ok(response);
    }

    public async Task Logout(CancellationToken cancellationToken)
    {
    }

    public Guid GetUserId()
    {
        var userId = httpContextAccessor
            .HttpContext?
            .User
            .Claims
            .FirstOrDefault(c => c.Type == "userId")?
            .Value;

        return userId.ToGuid();
    }
}