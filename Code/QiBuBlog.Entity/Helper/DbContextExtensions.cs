using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq.Expressions;
using System.Reflection;

namespace QiBuBlog.Entity.Helper
{
    public static class DbContextExtensions
    {
        public static void Update<TEntity, TKey>(this DbContext dbContext, params TEntity[] entities) where TEntity : class
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                var dbSet = dbContext.Set<TEntity>();
                try
                {
                    var entry = dbContext.Entry(entity);

                    if (entry.State == EntityState.Detached)
                    {
                        dbSet.Attach(entity);
                        entry.State = EntityState.Modified;
                    }
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
            }
        }

        public static void Update<TEntity, TKey>(this DbContext dbContext, Expression<Func<TEntity, object>> propertyExpression, params TEntity[] entities)
            where TEntity : class 
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            ReadOnlyCollection<MemberInfo> memberInfos = ((dynamic)propertyExpression.Body).Members;
            foreach (var entity in entities)
            {
                var dbSet = dbContext.Set<TEntity>();
                try
                {
                    var entry = dbContext.Entry(entity);
                    entry.State = EntityState.Unchanged;
                    foreach (var memberInfo in memberInfos)
                    {
                        entry.Property(memberInfo.Name).IsModified = true;
                    }
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
            }
        }

        public static int SaveChanges(this DbContext dbContext, bool validateOnSaveEnabled)
        {
            var isReturn = dbContext.Configuration.ValidateOnSaveEnabled != validateOnSaveEnabled;
            try
            {
                dbContext.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                return dbContext.SaveChanges();
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            finally
            {
                if (isReturn)
                {
                    dbContext.Configuration.ValidateOnSaveEnabled = !validateOnSaveEnabled;
                }
            }
        }
    }
}
