using Dapper;
using Kurochou.Domain.Interface.Repository;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace Kurochou.Data.Repositories;

public class Repository<T>(IDbConnection conn) : IRepository<T> where T : class
{
    #region Repository setup

    private readonly string _table = GetTableName();

    private static string GetTableName()
    {
        var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
        return tableAttr?.Name ?? typeof(T).Name;
    }

    private static string GetKeyName()
    {
        var keyProp = typeof(T).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<TableAttribute>() != null);

        return keyProp?.Name ?? "Id";
    }

    #endregion

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = $"SELECT * FROM {_table}";
        return await conn.QueryAsync<T>(new CommandDefinition(sql, cancellationToken: cancellationToken));
    }

    public async Task<T?> GetByIdAsync(Guid? id, CancellationToken cancellationToken)
    {
        var sql = $"SELECT * FROM {_table} WHERE id = @Id";
        return await conn.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
    }

    public async Task<int> InsertAsync(T entity, CancellationToken cancellationToken = default)
    {
        var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id")
                .ToList();

        var columnNames = string.Join(", ", properties.Select(p => ToSnakeCase(p.Name)));
        var paramNames = string.Join(", ", properties.Select(p => "@" + p.Name));

        var sql = $"INSERT INTO {_table} ({columnNames}) VALUES ({paramNames})";

        return await conn.ExecuteAsync(sql, entity);
    }

    public async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id")
                .ToList();

        var setClause = string.Join(", ", properties.Select(p => $"{ToSnakeCase(p.Name)} = @{p.Name}"));
        var sql = $"UPDATE {_table} SET {setClause} where id = @Id";

        return await conn.ExecuteAsync(sql, entity);
    }

    public async Task<int> DeleteAsync(object id, CancellationToken cancellationToken = default)
    {
        var sql = $"DELETE FROM {_table} WHERE id = @Id";
        return await conn.ExecuteAsync(sql, new { Id = id });
    }

    private static string ToSnakeCase(string name)
    {
        return string.Concat(name.Select((ch, i) =>
                i > 0 && char.IsUpper(ch) ? "_" + char.ToLower(ch) : char.ToLower(ch).ToString()
        ));
    }
}