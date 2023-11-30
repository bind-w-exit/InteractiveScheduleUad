using Microsoft.EntityFrameworkCore;

namespace InteractiveScheduleUad.Api.Utilities;

public static class Utls
{
    public static bool IsForeignKeyOrphaned(int? key, object? resolvedEntity)
    {
        return (key is not null && resolvedEntity is null);
    }

    // checks whether entity exists and returns it. If the entity doesn't exist, creates and returns it.
    public static EntityT EnsureExists<EntityT>
        (DbSet<EntityT> dbSet, EntityT entity, Func<EntityT, bool> compareF)
         where EntityT : class
    {
        var matchingEntityFromDb = dbSet.Where(compareF).FirstOrDefault();

        EntityT entityToReturn;

        if (matchingEntityFromDb is null)
        {
            var addedEntity = dbSet.Add(entity);
            return addedEntity.Entity;
        }
        else
        {
            entityToReturn = matchingEntityFromDb;
        }

        return entityToReturn;
    }

    // gets object property name and returns that property value
    public static ReturnT? GetPropertyValue<ReturnT>(object obj, string propertyName)
    {
        ReturnT? value = (ReturnT)(obj.GetType().GetProperty(propertyName)?.GetValue(obj, null));
        return value;
    }
}