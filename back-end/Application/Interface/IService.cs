using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interface
{
    public interface IService<T> where T : class
    {
        public T Find(object id);
        public T Create(T entity);
        public bool Delete(T entity);

        public bool Delete(object id);
        public IEnumerable<T> GetAll(int pageIndex = 0, int pageSize = int.MaxValue, string includeProperties = "");
        public T Update(T entity);

    }
}
