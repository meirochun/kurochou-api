namespace Kurochou.Domain.Interface.Repository;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid? id, CancellationToken cancellationToken = default);
    Task<int> InsertAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(object id, CancellationToken cancellationToken = default);
}