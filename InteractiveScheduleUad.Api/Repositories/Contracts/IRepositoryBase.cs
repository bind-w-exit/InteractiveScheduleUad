using System.Linq.Expressions;

namespace InteractiveScheduleUad.Api.Repositories.Contracts;

public interface IRepositoryBase<T> : IDisposable
    where T : class
{
    void Delete(T entity);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(object id);

    Task InsertAsync(T entity);

    Task SaveChangesAsync();

    void Update(T entity);
}