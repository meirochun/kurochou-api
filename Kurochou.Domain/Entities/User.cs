using Kurochou.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kurochou.Domain.Entities;

[Table("users")]
public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string? GoogleId { get; set; }
    public UserRole Role { get; set; }

    public void Update(string username, string passwordHash, UserRole role)
    {
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
        UpdatedAt = DateTime.Now;
    }
}