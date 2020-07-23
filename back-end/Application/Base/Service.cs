using Application.Interface;
using Domain.Interface;
using Domain.Interface.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Base
{
    public class Service<T> : IService<T> where T : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IRepository<T> _repository;

        public Service(IUnitOfWork unitOfWork, IRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            
        }

        public virtual T Create(T entity)
        {
            if (entity == null) throw new Exception("Entity " + entity.GetType().Name + " empty or null");
            try
            {

                var t = _repository.Add(entity);

                if (_unitOfWork.Commit() > 0)
                {
                    return t;
                }
            }catch(Exception e)
            {
                throw new Exception("Entity " + e.Message);
            }
            return null;
        }

        public virtual bool Delete(T entity)
        {
            if (entity == null) throw new Exception("Entity " + entity.GetType().Name + " empty or null");
            try
            {
                _repository.Delete(entity);
                if (_unitOfWork.Commit() > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Entity " + e.Message.ToString());
            }
            
            return false;
        }

        public virtual bool Delete(object id)
        {
            if (id == null) throw new Exception("id empty or null");
            try
            {
                _repository.Delete(id);
                if (_unitOfWork.Commit() > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Entity " + e.Message.ToString());
            }

            return false;
        }

        public virtual T Find(object id)
        {
            if (id == null) throw new Exception("id empty or null");
            return _repository.Find(id);
        }

        public virtual IEnumerable<T> GetAll(int pageIndex = 0, int pageSize = int.MaxValue, string includeProperties = "")
        {
            return _repository.GetAll(pageIndex, pageSize, includeProperties);
        }

        public virtual T Update(T entity)
        {
            if (entity == null) throw new Exception("Entity " + entity.GetType().Name + " empty or null");
            try
            {
                var t = _repository.Edit(entity);
                if (_unitOfWork.Commit() > 0)
                {
                    return t;
                }
            }
            catch(Exception e)
            {
                throw new Exception("Entity " + e.Message.ToString());
            }
            
            return null;
        }
    }
}
