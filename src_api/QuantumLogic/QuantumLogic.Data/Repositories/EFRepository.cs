﻿using Microsoft.EntityFrameworkCore;
using QuantumLogic.Core.Domain.Entities;
using QuantumLogic.Core.Domain.Repositories;
using QuantumLogic.Data.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QuantumLogic.Data.Repositories
{
    public class EFRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Injected dependencies

        private DbContextManager DbContextManager { get; set; }

        #endregion

        public bool OnSystemFilters { get; set; }
        public IQueryable<TEntity> Query
        {
            get
            {
                return DbContextManager.CurrentContext.Set<TEntity>().AsQueryable();
            }
        }

        #region Ctors

        public EFRepository(DbContextManager dbContextManager)
            : this(dbContextManager, true)
        { }

        public EFRepository(DbContextManager dbContextManager, bool onSystemFilters)
            
        {
            DbContextManager = dbContextManager;
            OnSystemFilters = true;
        }

        #endregion

        public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryBuilder, params Expression<Func<TEntity, object>>[] includes)
        {
            bool createdNew;
            DbSet<TEntity> set = DbContextManager.BuildOrCurrentContext(out createdNew).Set<TEntity>();
            IList<TEntity> entities = await queryBuilder(ApplyIncludes(set, includes)).ToListAsync();
            if (createdNew)
            {
                DbContextManager.DisposeContext();
            }
            return entities;
        }
        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            bool createdNew = false;
            try
            {
                DbSet<TEntity> set = DbContextManager.BuildOrCurrentContext(out createdNew).Set<TEntity>();
                IQueryable<TEntity> temp = OnSystemFilters ?
                    await ApplySystemFilters(Queryable.Where(ApplyIncludes(set, includes), filter)) :
                    Queryable.Where(ApplyIncludes(set, includes), filter);
                TEntity entity = await temp.FirstOrDefaultAsync();
                return entity;
            }
            finally
            {
                if (createdNew)
                {
                    DbContextManager.DisposeContext();
                }
            }
        }
        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            bool createdNew = false;
            try
            {
                DbSet<TEntity> set = DbContextManager.BuildOrCurrentContext(out createdNew).Set<TEntity>();
                IQueryable<TEntity> temp = OnSystemFilters ?
                    await ApplySystemFilters(Queryable.Where(ApplyIncludes(set, includes), filter)) :
                    Queryable.Where(ApplyIncludes(set, includes), filter);
                TEntity entity = await temp.SingleOrDefaultAsync();
                return entity;
            }
            finally
            {
                if (createdNew)
                {
                    DbContextManager.DisposeContext();
                }
            }
        }
        public virtual async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            bool createdNew = false;
            try
            {
                DbSet<TEntity> set = DbContextManager.BuildOrCurrentContext(out createdNew).Set<TEntity>();
                IQueryable<TEntity> temp = OnSystemFilters ?
                    await ApplySystemFilters(Queryable.Where(ApplyIncludes(set, includes), filter)) :
                    Queryable.Where(ApplyIncludes(set, includes), filter);
                TEntity entity = await temp.FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new Exception($"Entity {typeof(TEntity).Name} with predicate {filter.ToString()} not found.");
                }
                return entity;
            }
            finally
            {
                if (createdNew)
                {
                    DbContextManager.DisposeContext();
                }
            }
        }
        public virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            bool createdNew = false;
            try
            {
                DbSet<TEntity> set = DbContextManager.BuildOrCurrentContext(out createdNew).Set<TEntity>();
                IQueryable<TEntity> query = OnSystemFilters ?
                    await ApplySystemFilters(Queryable.Where(ApplyIncludes(set, includes), filter)) :
                    Queryable.Where(ApplyIncludes(set, includes), filter);
                TEntity entity = null;
                try
                {
                    entity = await query.SingleOrDefaultAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Entity {typeof(TEntity).Name} with predicate {filter.ToString()} is not unique.", ex);
                }
                if (entity == null)
                {
                    throw new Exception($"Entity {typeof(TEntity).Name} with predicate {filter.ToString()} not found.");
                }
                return entity;
            }
            finally
            {
                if (createdNew)
                {
                    DbContextManager.DisposeContext();
                }
            }
        }
        public virtual async Task<TEntity> GetAsync(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includes)
        {
            bool createdNew = false;
            try
            {
                DbSet<TEntity> set = DbContextManager.BuildOrCurrentContext(out createdNew).Set<TEntity>();
                IQueryable<TEntity> temp = OnSystemFilters ?
                    await ApplySystemFilters(Queryable.Where(ApplyIncludes(set, includes),
                        CreateEqualityExpressionForId(id))) :
                    Queryable.Where(ApplyIncludes(set, includes), CreateEqualityExpressionForId(id));
                TEntity entity = await temp.FirstOrDefaultAsync();
                if (entity == null)
                {
                    throw new Exception($"Entity {typeof(TEntity).Name} with id {id} not found.");
                }
                return entity;
            }
            finally
            {
                if (createdNew)
                {
                    DbContextManager.DisposeContext();
                }
            }
        }
        public virtual async Task CreateAsync(TEntity entity)
        {
            bool createdNew = false;
            try
            {
                DbContextManager.BuildOrCurrentContext(out createdNew).Add(entity);
            }
            finally
            {
                if (createdNew)
                {
                    await DbContextManager.CurrentContext.SaveChangesAsync();
                    DbContextManager.DisposeContext();
                }
            }
        }
        public virtual async Task UpdateAsync(TEntity entity)
        {
            bool createdNew = false;
            try
            {
                DbContextManager.BuildOrCurrentContext(out createdNew).Update(entity);
            }
            finally
            {
                if (createdNew)
                {
                    await DbContextManager.CurrentContext.SaveChangesAsync();
                    DbContextManager.DisposeContext();
                }
            }
        }
        public virtual async Task DeleteAsync(TEntity entity)
        {
            bool createdNew = false;
            try
            {
                DbContextManager.BuildOrCurrentContext(out createdNew).Remove(entity);
            }
            finally
            {
                if (createdNew)
                {
                    await DbContextManager.CurrentContext.SaveChangesAsync();
                    DbContextManager.DisposeContext();
                }
            }
        }

        #region Helpers

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );
            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        protected virtual Task<IQueryable<TEntity>> ApplySystemFilters(IQueryable<TEntity> query)
        {
            return Task.FromResult(query);
        }

        private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> result = query;
            foreach (var include in includes)
            {
                result = result.Include(include);
            }
            return result;
        }

        #endregion
    }
}
