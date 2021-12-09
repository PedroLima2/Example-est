using Examples.Charge.Domain.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Charge.Domain.Base
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repository;

        public ServiceBase(IRepositoryBase<TEntity> Repository)
        {
            _repository = Repository;
        }

        public virtual TEntity Add(TEntity Entity)
        {
            return _repository.Add(Entity);
        }

        public virtual TEntity AddOrUpdate(TEntity Entidade)
        {
            return _repository.AddOrUpdate(Entidade);
        }

        public virtual void Dispose()
        {
            _repository.Dispose();
        }

        public virtual Task<IEnumerable<TEntity>> FindAllAsync()
        {
            return _repository.FindAllAsync();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual TEntity GetById(int id)
        {
            return _repository.GetById(id);
        }

        public virtual IQueryable<TEntity> GetQuery()
        {
            return _repository.GetQuery();
        }

        public virtual void Remove(TEntity Entity)
        {
            _repository.Remove(Entity);
        }

        public virtual TEntity Update(TEntity Entity)
        {
            return _repository.Update(Entity);
        }
    }
}
