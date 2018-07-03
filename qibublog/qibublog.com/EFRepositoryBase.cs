using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QiBuBlog.Com
{

    public class EFRepositoryBase<TEntity, TKey> where TEntity : class
    {
        #region 属性

        public EFRepositoryBase()
        {
            //TODO:
        }

        protected DbContext EFContext = new EFDbContext<TEntity>().Instance;

        public bool IsCommitted { get; private set; }

        public int Commit(bool validateOnSaveEnabled = true)
        {
            if (IsCommitted)
            {
                return 0;
            }
            try
            {
                int result = EFContext.SaveChanges(validateOnSaveEnabled);
                //为了防止CRD操作时，上下文已存了对象出现的错误
                EFContext = new EFDbContext<TEntity>().Instance;
                IsCommitted = true;
                return result;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null && e.InnerException.InnerException is SqlException)
                {
                    SqlException sqlEx = e.InnerException.InnerException as SqlException;
                    string msg = DataHelper.GetSqlExceptionMessage(sqlEx.Number);
                    throw CommonHelper.ThrowDataAccessException("提交数据更新时发生异常：" + msg, sqlEx);
                }
                else
                {
                    throw e;
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
            EFContext.Dispose();
        }

        public virtual IQueryable<TEntity> Entities
        {
            get { return EFContext.Set<TEntity>().AsNoTracking(); }
        }

        #endregion

        #region 公共方法

        public virtual int ExecuteSql(string sql)
        {
            return EFContext.Database.ExecuteSqlCommand(sql);
        }

        public virtual IEnumerable<T> ExcSql<T>(string sql)
        {
            return EFContext.Database.SqlQuery<T>(sql);
        }

        public virtual int Insert(TEntity entity, bool isSave = true)
        {
            CommonHelper.CheckArgument(entity, "entity");
            EntityState state = EFContext.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                EFContext.Set<TEntity>().Add(entity);
                EFContext.Entry(entity).State = EntityState.Added;
            }
            IsCommitted = false;
            return isSave ? Commit() : 0;
        }

        public virtual int Insert(IEnumerable<TEntity> entities, bool isSave = true)
        {
            CommonHelper.CheckArgument(entities, "entities");

            try
            {
                EFContext.Configuration.AutoDetectChangesEnabled = false;

                foreach (TEntity entity in entities)
                {
                    EntityState state = EFContext.Entry(entity).State;
                    if (state == EntityState.Detached)
                    {
                        EFContext.Set<TEntity>().Add(entity);
                        EFContext.Entry(entity).State = EntityState.Added;
                    }
                    IsCommitted = false;
                }
            }
            finally
            {
                EFContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return isSave ? Commit() : 0;
        }

        public virtual int Delete(TKey id, bool isSave = true)
        {
            CommonHelper.CheckArgument(id, "id");
            TEntity entity = EFContext.Set<TEntity>().Find(id);
            return entity != null ? Delete(entity, isSave) : 0;
        }

        public virtual int Delete(TEntity entity, bool isSave = true)
        {
            CommonHelper.CheckArgument(entity, "entity");
            EFContext.Entry(entity).State = EntityState.Deleted;
            IsCommitted = false;
            return isSave ? Commit() : 0;
        }

        public virtual int Delete(IEnumerable<TEntity> entities, bool isSave = true)
        {
            CommonHelper.CheckArgument(entities, "entities");
            try
            {
                EFContext.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    EFContext.Entry(entity).State = EntityState.Deleted;
                    IsCommitted = false;
                }
            }
            finally
            {
                EFContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return isSave ? Commit() : 0;
        }

        public virtual int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave = true)
        {
            CommonHelper.CheckArgument(predicate, "predicate");
            List<TEntity> entities = EFContext.Set<TEntity>().Where(predicate).ToList();
            return entities.Count > 0 ? Delete(entities, isSave) : 0;
        }

        public virtual int Update(TEntity entity, bool isSave = true)
        {
            CommonHelper.CheckArgument(entity, "entity");
            EFContext.Update<TEntity, TKey>(entity);
            IsCommitted = false;
            return isSave ? Commit() : 0;
        }

        public virtual int Update(IEnumerable<TEntity> entities, bool isSave = true)
        {
            CommonHelper.CheckArgument(entities, "entities");
            try
            {
                EFContext.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    EFContext.Entry(entity).State = EntityState.Modified;
                    IsCommitted = false;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                throw dbEx;
            }
            finally
            {
                EFContext.Configuration.AutoDetectChangesEnabled = true;
            }
            return isSave ? Commit() : 0;
        }

        public int Update(Expression<Func<TEntity, object>> propertyExpression, TEntity entity, bool isSave = true)
        {
            CommonHelper.CheckArgument(propertyExpression, "propertyExpression");
            CommonHelper.CheckArgument(entity, "entity");

            EFContext.Update<TEntity, TKey>(propertyExpression, entity);
            IsCommitted = false;

            if (isSave)
            {
                var dbSet = EFContext.Set<TEntity>();
                dbSet.Local.Clear();
                var entry = EFContext.Entry(entity);
                return Commit(false);
            }
            return 0;
        }

        public virtual TEntity GetByKey(TKey key)
        {
            CommonHelper.CheckArgument(key, "key");
            using (DbContext EFContext1 = new EFDbContext<TEntity>().Instance)
            {
                return EFContext1.Set<TEntity>().Find(key);
            }
        }

        public virtual TEntity GetByKey(params object[] keyValues)
        {
            CommonHelper.CheckArgument(keyValues, "keyValues");
            using (DbContext EFContext1 = new EFDbContext<TEntity>().Instance)
            {
                return EFContext1.Set<TEntity>().Find(keyValues);
            }
        }

        public virtual IQueryable<TT> ExtensionEntitys<TT>(string sql, object[] param) where TT : class
        {
            CommonHelper.CheckArgument(sql, "sql");
            if (param != null)
            {
                CommonHelper.CheckArgument(param, "param");
                return EFContext.Database.SqlQuery<TT>(sql, param).AsQueryable();
            }
            else
            {
                return EFContext.Database.SqlQuery<TT>(sql).AsQueryable();
            }
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> exp)
        {
            CommonHelper.CheckArgument(exp, "exp");
            using (DbContext EFContext1 = new EFDbContext<TEntity>().Instance)
            {
                return EFContext1.Set<TEntity>().AsNoTracking().FirstOrDefault(exp);
            }
        }

        public virtual IQueryable<TEntity> BatchFind(Expression<Func<TEntity, bool>> exp)
        {
            CommonHelper.CheckArgument(exp, "exp");
            DbContext EFContext1 = new EFDbContext<TEntity>().Instance;
            return EFContext1.Set<TEntity>().Where(exp).AsNoTracking();
        }
        #endregion
    }
}
