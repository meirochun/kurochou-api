using Kurochou.Domain.Enum;

namespace Kurochou.Domain.DTO;

public class UserWithClipCount
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Uploads { get; set; }
}
