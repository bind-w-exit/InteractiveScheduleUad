﻿using System.Linq.Expressions;

namespace InteractiveScheduleUad.Api.Repositories.Contracts;

public interface IRepositoryBase<T>
    where T : class
{
    void Delete(T entity);

    Task DeleteAll();

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    Task<IEnumerable<T>> GetAllAsync(bool includeNestedObjects = false);

    Task<T?> GetByIdAsync(object? id);

    Task InsertAsync(T entity);

    Task SaveChangesAsync();

    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

    void Update(T entity);
}