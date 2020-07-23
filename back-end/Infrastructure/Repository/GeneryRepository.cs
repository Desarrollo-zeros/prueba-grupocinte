using Domain.Interface.Domain.Interface;
using Infrastructure.Base;
using Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Repository
{
    public class GeneryRepository<T> : IRepository<T> where T : class
    {
        protected IDbContext _db;
        protected readonly DbSet<T> _dbset;

        public GeneryRepository(IDbContext context)
        {
            _db = context;
            _dbset = context.Set<T>();
        }

        public T Add(T entity) => _dbset.Add(entity).Entity;

        public void AddRange(IEnumerable<T> entities) => _dbset.AddRange(entities);


        public void Delete(object id) => _dbset.Remove(Find(id));


        public void Delete(T entity) => _dbset.Remove(entity);


        public void DeleteRange(IEnumerable<T> entities) => _dbset.RemoveRange(entities);


        public T Edit(T entity) => _dbset.Update(entity).Entity;


        public T Find(object id) => _dbset.Find(id);

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbset;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public virtual IEnumerable<T> GetAll(int pageIndex = 0, int pageSize = int.MaxValue, string includeProperties = "") {
            IQueryable<T> query = _dbset;
            foreach (var includeProperty in includeProperties.Split
               (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return new PagedList<T>(query.ToList(), pageIndex, pageSize).ToList();
           
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
            _db.Dispose();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Any(predicate);
        }
    }
}
