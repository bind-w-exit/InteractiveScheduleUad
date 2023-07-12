using InteractiveScheduleUad.Api.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InteractiveScheduleUad.Api.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    protected DbContext Context { get; private set; }

    public RepositoryBase(DbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<T>> GetAllAsync(bool includeNestedObjects = false)
    {
        if (includeNestedObjects)
        {
            // For returning nested objects instead of null
            var entityType = Context.Model.FindEntityType(typeof(T));
            var navigations = entityType?.GetNavigations();

            IQueryable<T> query = Context.Set<T>();

            if (navigations is not null)
            {
                foreach (var navigation in navigations)
                {
                    query = query.Include(navigation.Name);
                }
            }

            return await query.ToListAsync();
        }
        else
            return await Context.Set<T>().ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().SingleOrDefaultAsync(predicate);
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<T?> GetByIdAsync(object? id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public virtual async Task InsertAsync(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public virtual async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }

    // Implement IDisposable
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Context.Dispose();
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}