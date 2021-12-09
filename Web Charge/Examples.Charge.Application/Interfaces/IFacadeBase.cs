using Examples.Charge.Application.Common.Messages;
using Examples.Charge.Application.Messages.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Charge.Application.Interfaces
{
    public interface IFacadeBase<TEntityDto>
        where TEntityDto : class
    {
        ResponseBase<TEntityDto> Add(TEntityDto Entity);

        ResponseBase<TEntityDto> GetById(int id);

        Task<ResponseBase<TEntityDto>> FindAllAsync();

        //SympResultPaginadoDTO<IEnumerable<SexoDTO>> GetPaginado(int QtdItensPagina, string Filtro, int PaginaAtual, string FieldOrdenar);

        ResponseBase<TEntityDto> Update(TEntityDto Entity);

        void Remove(TEntityDto Entity);

        void Dispose();
    }
}
