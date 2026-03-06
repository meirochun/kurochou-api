using Kurochou.App.DTO.Clips.Request;
using Kurochou.Domain.Entities;

namespace Kurochou.App.Interfaces.Service;

public interface IClipService
{
    Task<IEnumerable<Clip>> GetClipsAsync(CancellationToken cancellationToken);
    Task<Guid> UploadAsync(UploadRequest request, CancellationToken cancellationToken);
}