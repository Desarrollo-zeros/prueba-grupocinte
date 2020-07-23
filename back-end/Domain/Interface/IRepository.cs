using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Interface
{
    namespace Domain.Interface
    {
        public interface IRepository<T> : IDisposable where T : class
        {
            public T Add(T entity);
            public T Edit(T entity);

            public bool Any(Expression<Func<T, bool>> predicate);

            public void Delete(object id);
            public void Delete(T entity);
            public T Find(object id);
            public void AddRange(IEnumerable<T> entities);
            public void DeleteRange(IEnumerable<T> entities);
            public IEnumerable<T> GetAll(int pageIndex = 0, int pageSize = int.MaxValue, string includeProperties = "");

            public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
            public IEnumerable<T> FindBy(
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                string includeProperties = ""
            );


        }
    }

}
