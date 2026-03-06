using Kurochou.App.DTO.Clips.Request;
using Kurochou.App.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace Kurochou.API.Controllers;

public class ClipController(IClipService uploadService) : KuroController
{
    [HttpGet]
    public async Task<IResult> GetClips(CancellationToken cancellationToken)
    {
        var clips = await uploadService.GetClipsAsync(cancellationToken);
        return Response(clips);
    }

    [HttpPost("upload")]
    public async Task<IResult> UploadClip([FromBody] UploadRequest request, CancellationToken cancellationToken)
    {
        var clipId = await uploadService.UploadAsync(request, cancellationToken);
        return Response(clipId);
    }
}