using Kurochou.Domain.Enum;

namespace Kurochou.App.DTO.User.Request;

public sealed class CreateUserRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required UserRole Role { get; set; }
}
