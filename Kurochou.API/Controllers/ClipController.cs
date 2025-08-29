using Kurochou.API.Helpers;
using Kurochou.Domain.DTO.Clips;
using Kurochou.Domain.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace Kurochou.API.Controllers;

public class ClipController(IUploadService uploadService) : KuroController
{
        [HttpPost("Upload")]
        public async Task<IResult> Upload([FromForm] UploadRequest request, CancellationToken cancellationToken)
        {
                var clipId = await uploadService.Upload(request, request.UserId, cancellationToken);
                return ApiResult.Success(clipId);
        }
}