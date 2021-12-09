using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Charge.Domain.Base.Interfaces
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        TEntity Add(TEntity Entity);

        TEntity AddOrUpdate(TEntity Entidade);

        TEntity GetById(int id);

        Task<IEnumerable<TEntity>> FindAllAsync();

        IEnumerable<TEntity> GetAll();

        IQueryable<TEntity> GetQuery();

        //SympResultPaginado<IEnumerable<TEntity>> GetPaginado(int QtdItensPagina, string Filtro, int PaginaAtual, string FieldOrdenar);

        TEntity Update(TEntity Entity);

        void Remove(TEntity Entity);

        void Dispose();
    }
}
