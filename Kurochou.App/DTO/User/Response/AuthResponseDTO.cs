namespace Kurochou.App.DTO.User.Response;

public record AuthResponseDTO(string Token, int Expires, string Role);
