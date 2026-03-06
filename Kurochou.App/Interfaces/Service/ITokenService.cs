using Kurochou.Domain.Entities;

namespace Kurochou.App.Interfaces.Service;

public interface ITokenService
{
    string GenerateToken(User user);
}