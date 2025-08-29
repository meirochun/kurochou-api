using Kurochou.API.Helpers;
using Kurochou.Domain.DTO.Auth;
using Kurochou.Domain.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kurochou.API.Controllers;

public class AuthController(IAuthenticationService service) : KuroController
{
        [HttpPost("Register")]
        [Authorize("AdminPolicy")]
        public async Task<IResult> RegisterAsync([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
                var result = await service.Register(request, cancellationToken);

                if (result is null)
                {
                        return ApiResult.Failure("Unable to register the user");
                }

                return ApiResult.Success(result, "Registered successfully");
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
                var result = await service.Login(request, cancellationToken);

                if (result is null || string.IsNullOrWhiteSpace(result.Token))
                {
                        return ApiResult.Failure("Invalid username or password", 401);
                }

                return ApiResult.Success(result, "Logged in successfully");
        }
}