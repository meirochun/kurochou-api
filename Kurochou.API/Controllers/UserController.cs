using Kurochou.API.Helpers;
using Kurochou.Domain.DTO.User;
using Kurochou.Domain.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kurochou.API.Controllers;

public class UserController(IUserService service) : KuroController
{
        private readonly IUserService _service = service;

        [HttpGet]
        [Authorize("AdminPolicy")]
        public async Task<IResult> GetUserAsync([FromQuery] GetUserRequest request, CancellationToken cancellationToken)
        {
                var result = await _service.GetUsersAsync(request, cancellationToken);
                return ApiResult.Success(result);
        }
}