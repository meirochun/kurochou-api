using Kurochou.App.DTO.Clips.Request;
using Kurochou.App.Interfaces.Service;
using Kurochou.Domain.Entities;
using Kurochou.Domain.Interface.Repository;

namespace Kurochou.App.Service;

public class ClipService(IClipRepository clipRepository, IAuthenticationService authService) : IClipService
{
    public async Task<IEnumerable<Clip>> GetClipsAsync(CancellationToken cancellationToken)
    {
        var clips = await clipRepository.GetAllAsync(cancellationToken);
        return clips;
    }

    public async Task<Guid> UploadAsync(UploadRequest request, CancellationToken cancellationToken)
    {
        var userId = authService.GetUserId();

        var clip = new Clip
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            UserId = userId,
            Upvote = 0,
            IsPublic = request.IsPublic,
            Link = request.Link,
            CreatedAt = DateTime.Now,
        };

        await clipRepository.InsertAsync(clip, cancellationToken);

        return clip.Id;
    }
}