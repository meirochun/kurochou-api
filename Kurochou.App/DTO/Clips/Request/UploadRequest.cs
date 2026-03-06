namespace Kurochou.App.DTO.Clips.Request;

public record UploadRequest(string Title, string Description, bool IsPublic, string Link);