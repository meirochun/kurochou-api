namespace Kurochou.App.DTO;

public class Result<T>
{
    public bool Success { get; set; }
    public T? Data { get; init; }
    public string? Message { get; init; }
    public IEnumerable<string>? Errors { get; set; }

    public static Result<T> Ok(T data, string? message = null) => new()
    {
        Success = true,
        Data = data,
        Message = message
    };

    public static Result<T> Fail(IEnumerable<string> errors, string? message = null) => new()
    {
        Success = false,
        Errors = errors,
        Message = message
    };

    public static Result<T> Fail(string error, string? message = null) =>
        Fail([error], message);
}