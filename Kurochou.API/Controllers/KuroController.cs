using Kurochou.App.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Kurochou.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("UserPolicy")]
public class KuroController : ControllerBase
{
    protected new static IResult Response<T>(T data, int status = 200)
        => Results.Json(data, statusCode: status);

    protected new static IResult NoContent() => Results.NoContent();

    protected static IResult Failure(string error, int status = 400)
        => Results.Json(Result<object>.Fail(error), statusCode: status);

    protected static IResult Failure(IEnumerable<string> errors, int status = 400)
        => Results.Json(Result<object>.Fail(errors), statusCode: status);

    protected static IResult NotFound(string message = "Resource not found.")
        => Results.Json(Result<object>.Fail(message), statusCode: 404);

    protected static IResult Unauthorized(string message = "Unauthorized.")
        => Results.Json(Result<object>.Fail(message), statusCode: 401);

    protected static IResult Forbidden(string message = "Forbidden.")
        => Results.Json(Result<object>.Fail(message), statusCode: 403);

    protected static IResult ValidationFailure(ModelStateDictionary modelState)
    {
        var errors = modelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);

        return Results.Json(Result<object>.Fail(errors, "Validation failed."), statusCode: 422);
    }
}