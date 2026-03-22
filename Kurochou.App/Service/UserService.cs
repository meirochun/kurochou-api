using Kurochou.App.DTO;
using Kurochou.App.DTO.User.Request;
using Kurochou.App.Helper;
using Kurochou.App.Interfaces.Service;
using Kurochou.Domain.DTO;
using Kurochou.Domain.Entities;
using Kurochou.Domain.Interface.Repository;

namespace Kurochou.App.Service;

public class UserService(IUserRepository repository) : IUserService
{
    private readonly IUserRepository _repository = repository;

    public async Task<Result<IEnumerable<UserWithClipCount?>>> GetUsersAsync(GetUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetUsersAsync(request.Search, cancellationToken);
        return Result<IEnumerable<UserWithClipCount?>>.Ok(result);
    }

    public async Task<Result<Guid>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByUsernameAsync(request.Username, cancellationToken);

        if (user is not null)
            return Result<Guid>.Fail("The username is already in use");

        var passwordHash = StringHelper.EncryptPassword(request.Password);

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = passwordHash,
            Role = request.Role,
            CreatedAt = DateTime.Now
        };

        await _repository.InsertAsync(newUser, cancellationToken);

        return Result<Guid>.Ok(newUser.Id, "User created");
    }

    public async Task<Result<Guid>> UpdateUserAsync(Guid id, CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(id, cancellationToken);

        if (user is null)
            return Result<Guid>.Fail("User not found");

        var passwordHash = StringHelper.EncryptPassword(request.Password);
        user.Update(request.Username, passwordHash, request.Role);

        var result = await _repository.UpdateAsync(user, cancellationToken);

        return result > 0
            ? Result<Guid>.Ok(user.Id, "User updated")
            : Result<Guid>.Fail("Failed to update user.");
    }

    public async Task<Result<Guid>> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteAsync(request.Id, cancellationToken);

        return result > 0
            ? Result<Guid>.Ok(request.Id, "User deleted")
            : Result<Guid>.Fail(["Failed to delete user."]);
    }
}