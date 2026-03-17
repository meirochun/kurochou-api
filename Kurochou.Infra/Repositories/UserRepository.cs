using Dapper;
using Kurochou.Domain.Entities;
using Kurochou.Domain.Interface.Repository;
using System.Data;

namespace Kurochou.Infra.Repositories;

public class UserRepository(IDbConnection conn) : Repository<User>(conn), IUserRepository
{
    private readonly IDbConnection _conn = conn;

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken _)
    {
        const string sql = @"
            SELECT
                id,
                username,
                password_hash,
                role,
                created_at,
                updated_at
            FROM users
            WHERE username = @Username";

        return await _conn.QueryFirstOrDefaultAsync<User?>(sql, new { Username = username });
    }

    public async Task<User?> GetByGoogleIdAsync(string googleId, CancellationToken _)
    {
        const string sql = @"
            SELECT
                id,
                username,
                role,
                google_id
            FROM users
            WHERE google_id = @GoogleId";

        return await _conn.QueryFirstOrDefaultAsync<User?>(sql, new { GoogleId = googleId });
    }

    public async Task<IEnumerable<User?>> GetUsersAsync(string? search, CancellationToken _)
    {
        var sql = "SELECT id, username, role FROM users";

        if (!string.IsNullOrWhiteSpace(search))
            sql += " WHERE username LIKE '%' || @Username || '%'";

        return await _conn.QueryAsync<User?>(sql, new { Username = search });
    }
}