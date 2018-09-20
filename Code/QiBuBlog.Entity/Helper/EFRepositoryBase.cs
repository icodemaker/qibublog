using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace QiBuBlog.Entity.Helper
{

    public sealed class EFRepositoryBase<TEntity, TKey> where TEntity : class
    {
        #region 属性

        public EFRepositoryBase()
        {
            //TODO:
        }

        private DbContext _efContext = new EFDbContext<TEntity>().Instance;

        private bool IsCommitted { get; set; }

        private int Commit(bool validateOnSaveEnabled = true)
        {
            if (IsCommitted)
            {
                return 0;
            }
            try
            {
                var result = _efContext.SaveChanges(validateOnSaveEnabled);

                _efContext = new EFDbContext<TEntity>().Instance;
                IsCommitted = true;
                return result;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException?.InnerException is SqlException)
                {
                    var sqlEx = (SqlException) e.InnerException.InnerException;
                    var msg = DataHelper.GetSqlExceptionMessage(sqlEx.Number);
                    throw DataHelper.ThrowDataAccessException("提交数据更新时发生异常：" + msg, sqlEx);
                }
                else
                {
                    throw;
                }
            }
        }

        public void Rollback()
        {
            IsCommitted = false;
        }

        public void Dispose()
        {
            if (!IsCommitted)
            {
                Commit();
            }
            _efContext.Dispose();
        }

        public IQueryable<TEntity> Entities => _efContext.Set<TEntity>().AsNoTracking();

        #endregion

        #region 公共方法

        public int ExecuteSql(string sql)
        {
            return _efContext.Database.ExecuteSqlCommand(sql);
        }

        public IEnumerable<T> ExcSql<T>(string sql)
        {
            return _efContext.Database.SqlQuery<T>(sql);
        }

        public int Insert(TEntity entity, bool isSave = true)
        {
            DataHelper.CheckArgument(entity, "entity");
            var state = _efContext.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                _efContext.Set<TEntity>().Add(entity);
                _efContext.Entry(entity).State = EntityState.Added;
            }
            IsCommitted = false;
            return isSave ? Commit() : 0;
        }

        public int Insert(IEnumerable<TEntity> entities, bool isSave = true)
        {
            DataHelper.CheckArgument(entities, "entities");

            try
            {
                _efContext.Configuration.AutoDetectChangesEnabled = false;

                foreach (var entity in entities)
                {
                    var state = _efContext.Entry(entity).State;
                    if (state == EntityState.Detached)
                    {
                        _efContext.Set<TEntity>().Add(entity);
                        _efContext.Entry(entity).State = EntityState.Added;
                    }
                    IsCommitted = false;
                }
            }
            finally
            {
                _efContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return isSave ? Commit() : 0;
        }

        public int Delete(TKey id, bool isSave = true)
        {
            DataHelper.CheckArgument(id, "id");
            var entity = _efContext.Set<TEntity>().Find(id);
            return entity != null ? Delete(entity, isSave) : 0;
        }

        private int Delete(TEntity entity, bool isSave = true)
        {
            DataHelper.CheckArgument(entity, "entity");
            _efContext.Entry(entity).State = EntityState.Deleted;
            IsCommitted = false;
            return isSave ? Commit() : 0;
        }

        private int Delete(IEnumerable<TEntity> entities, bool isSave = true)
        {
            DataHelper.CheckArgument(entities, "entities");
            try
            {
                _efContext.Configuration.AutoDetectChangesEnabled = false;
                foreach (var entity in entities)
                {
                    _efContext.Entry(entity).State = EntityState.Deleted;
                    IsCommitted = false;
                }
            }
            finally
            {
                _efContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return isSave ? Commit() : 0;
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave = true)
        {
            DataHelper.CheckArgument(predicate, "predicate");
            var entities = _efContext.Set<TEntity>().Where(predicate).ToList();
            return entities.Count > 0 ? Delete(entities, isSave) : 0;
        }

        public int Update(TEntity entity, bool isSave = true)
        {
            DataHelper.CheckArgument(entity, "entity");
            _efContext.Update<TEntity, TKey>(entity);
            IsCommitted = false;
            return isSave ? Commit() : 0;
        }

        public int Update(IEnumerable<TEntity> entities, bool isSave = true)
        {
            DataHelper.CheckArgument(entities, "entities");
            try
            {
                _efContext.Configuration.AutoDetectChangesEnabled = false;
                foreach (var entity in entities)
                {
                    _efContext.Entry(entity).State = EntityState.Modified;
                    IsCommitted = false;
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            finally
            {
                _efContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return isSave ? Commit() : 0;
        }

        public int Update(Expression<Func<TEntity, object>> propertyExpression, TEntity entity, bool isSave = true)
        {
            DataHelper.CheckArgument(propertyExpression, "propertyExpression");
            DataHelper.CheckArgument(entity, "entity");

            _efContext.Update<TEntity, TKey>(propertyExpression, entity);
            IsCommitted = false;

            if (!isSave) return 0;
            var dbSet = _efContext.Set<TEntity>();
            dbSet.Local.Clear();
            var entry = _efContext.Entry(entity);
            return Commit(false);
        }

        public TEntity GetByKey(TKey key)
        {
            DataHelper.CheckArgument(key, "key");
            using (var efContext = new EFDbContext<TEntity>().Instance)
            {
                return efContext.Set<TEntity>().Find(key);
            }
        }

        public TEntity GetByKey(params object[] keyValues)
        {
            DataHelper.CheckArgument(keyValues, "keyValues");
            using (var efContext = new EFDbContext<TEntity>().Instance)
            {
                return efContext.Set<TEntity>().Find(keyValues);
            }
        }

        public IQueryable<TT> ExtensionEntitys<TT>(string sql, object[] param) where TT : class
        {
            DataHelper.CheckArgument(sql, "sql");
            if (param != null)
            {
                DataHelper.CheckArgument(param, "param");
                return _efContext.Database.SqlQuery<TT>(sql, param).AsQueryable();
            }
            else
            {
                return _efContext.Database.SqlQuery<TT>(sql).AsQueryable();
            }
        }

        public TEntity Find(Expression<Func<TEntity, bool>> exp)
        {
            DataHelper.CheckArgument(exp, "exp");
            using (var efContext = new EFDbContext<TEntity>().Instance)
            {
                return efContext.Set<TEntity>().AsNoTracking().FirstOrDefault(exp);
            }
        }

        public IQueryable<TEntity> BatchFind(Expression<Func<TEntity, bool>> exp)
        {
            DataHelper.CheckArgument(exp, "exp");
            var efContext = new EFDbContext<TEntity>().Instance;
            return efContext.Set<TEntity>().Where(exp).AsNoTracking();
        }
        #endregion
    }
}
