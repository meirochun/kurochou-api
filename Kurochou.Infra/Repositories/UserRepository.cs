using Dapper;
using Kurochou.Domain.Entities;
using Kurochou.Domain.Interface.Repository;
using System.Data;

namespace Kurochou.Infra.Repositories;

public class UserRepository(IDbConnection conn) : Repository<User>(conn), IUserRepository
{
    private readonly IDbConnection _conn = conn;

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT
                id,
                username,
                password_hash,
                role
            FROM users
            WHERE username = @Username";

        return await _conn.QueryFirstOrDefaultAsync<User?>(sql, new { Username = username });
    }

    public async Task<IEnumerable<User?>> GetUsersAsync(string? search, CancellationToken cancellationToken)
    {
        var sql = "SELECT username, role FROM users";

        if (!string.IsNullOrWhiteSpace(search))
            sql += " WHERE username LIKE '%' || @Username || '%'";

        return await _conn.QueryAsync<User?>(sql, new { Username = search });
    }
}