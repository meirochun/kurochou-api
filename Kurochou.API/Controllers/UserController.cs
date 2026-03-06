using Kurochou.App.DTO.User.Request;
using Kurochou.App.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Kurochou.API.Controllers;

public class UserController(IUserService service) : KuroController
{
    [HttpGet]
    [Authorize("AdminPolicy")]
    public async Task<IResult> GetUserAsync([FromQuery] GetUserRequest request, CancellationToken cancellationToken)
    {
        var result = await service.GetUsersAsync(request, cancellationToken);
        return Response(result);
    }

    [HttpPost]
    [Authorize("AdminPolicy")]
    public async Task<IResult> CreateUserAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateUserAsync(request, cancellationToken);

        if (!result.Success)
            return Failure(result.Errors!, (int)HttpStatusCode.BadRequest);

        return Response(result, (int)HttpStatusCode.Created);
    }

    [HttpPut("{id:guid}")]
    [Authorize("AdminPolicy")]
    public async Task<IResult> UpdateUserAsync([FromRoute] Guid id, [FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateUserAsync(id, request, cancellationToken);

        if (!result.Success)
            return Failure(result.Errors!, (int)HttpStatusCode.BadRequest);

        return Response(result, (int)HttpStatusCode.OK);
    }

    [HttpDelete]
    [Authorize("AdminPolicy")]
    public async Task<IResult> DeleteUserAsync([FromQuery] DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var result = await service.DeleteUserAsync(request, cancellationToken);

        if (!result.Success)
            return Failure(result.Errors!, (int)HttpStatusCode.BadRequest);

        return Response(result, (int)HttpStatusCode.OK);
    }
}