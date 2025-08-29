using Kurochou.Domain.DTO.User;
using Kurochou.Domain.Interface.Repository;
using Kurochou.Domain.Interface.Service;
using Kurochou.Domain.Model;

namespace Kurochou.Domain.Service;

public class UserService(IUserRepository repository) : IUserService
{
        private readonly IUserRepository _repository = repository;

        public async Task<IEnumerable<User?>> GetUsersAsync(GetUserRequest request, CancellationToken cancellationToken)
        {
                var result = await _repository.GetUsersAsync(request.Search, cancellationToken);

                return result;
        }
}