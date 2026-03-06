using System.ComponentModel.DataAnnotations.Schema;

namespace Kurochou.Domain.Entities;

[Table("clips")]
public class Clip : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public int Upvote { get; set; } = 0;
    public string Link { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
}