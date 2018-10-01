using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gitloy.Services.WebhookAPI.BusinessLogic.Core.Repositories;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.EntityFrameworkCore;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Persistence.EntityFrameworkCore.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {   
        private DbContext Context { get; set; }
        private readonly DbSet<TEntity> _dbSet;
        
        public Repository(DbContext context)
        {
            Context = context;
            _dbSet = context.Set<TEntity>();
        }
        
        public TEntity Get(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IEnumerable<TEntity> GetAllPaginate(int pageSize, int pageIndex)
        {
            if(pageSize < 0)
                throw new ArgumentException("PageSize is negative");
            
            if (pageIndex < 0)
                throw new ArgumentException("PageIndex is negative");
            
            return _dbSet.Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllPaginateAsync(int pageSize = 10, int pageIndex = 0)
        {
            if (pageSize < 0)
                throw new ArgumentException("PageSize is negative");

            if (pageIndex < 0)
                throw new ArgumentException("PageIndex is negative");

            return await _dbSet.Skip(pageSize * pageIndex)
                              .Take(pageSize)
                              .ToListAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.SingleOrDefault(predicate);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(TEntity entityToUpdate)
		{
			_dbSet.Attach(entityToUpdate);
			Context.Entry(entityToUpdate).State = EntityState.Modified;
		}

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public bool All(Expression<Func<TEntity, bool>> predicate)
		{
			return _dbSet.All(predicate);
		}

        public async Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AllAsync(predicate);
        }

        public bool Contains(TEntity entity)
		{
			return _dbSet.Contains(entity);
		}

        public async Task<bool> ContainsAsync(TEntity entity)
        {
            return await _dbSet.ContainsAsync(entity);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
		{
			return _dbSet.Count(predicate);
		}

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return Any(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await AnyAsync(predicate);
        }
    }
}