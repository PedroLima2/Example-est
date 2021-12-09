using AutoMapper;
using Examples.Charge.Application.Common.Messages;
using Examples.Charge.Application.Interfaces;
using Examples.Charge.Domain.Base.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examples.Charge.Application.Facade
{
    public class FacadeBase<TEntity, TEntityDto> : IFacadeBase<TEntityDto>
        //where TEntityRequest : class
        //where TEntityResponse : class
        where TEntity : class
        where TEntityDto : class
    {
        private readonly IServiceBase<TEntity> _serviceBase;
        private readonly IMapper _mapper;

        public FacadeBase(IServiceBase<TEntity> serviceBase, IMapper mapper)
        {
            _serviceBase = serviceBase;
            _mapper = mapper;
        }

        public virtual ResponseBase<TEntityDto> Add(TEntityDto Entity)
        {
            var objCliente = _mapper.Map<TEntity>(Entity);
            var enti = _serviceBase.Add(objCliente);
            var response = new ResponseBase<TEntityDto>();
            response.Objects = new List<TEntityDto>();
            response.Objects.Add(_mapper.Map<TEntityDto>(enti));
            return response;

        }

        public virtual void Dispose()
        {
            _serviceBase?.Dispose();
        }

        public virtual async Task<ResponseBase<TEntityDto>> FindAllAsync()
        {
            var result = await _serviceBase.FindAllAsync();
            var response = new ResponseBase<TEntityDto>();
            response.Objects = new List<TEntityDto>();
            response.Objects.AddRange(result.Select(x => _mapper.Map<TEntityDto>(x)));
            return response;
        }

        public virtual ResponseBase<TEntityDto> GetById(int id)
        {
            var entity = _serviceBase.GetById(id);

            var response = new ResponseBase<TEntityDto>();
            response.Objects = new List<TEntityDto>();
            response.Objects.Add(_mapper.Map<TEntityDto>(entity));
            return response;

        }

        public virtual void Remove(TEntityDto Entity)
        {
            throw new System.NotImplementedException();
        }

        public virtual ResponseBase<TEntityDto> Update(TEntityDto Entity)
        {
            var entity = _mapper.Map<TEntity>(Entity);
            _serviceBase.Update(entity);

            var response = new ResponseBase<TEntityDto>();
            response.Objects = new List<TEntityDto>();
            response.Objects.Add(_mapper.Map<TEntityDto>(entity));
            return response;
        }

        //public Task<TEntityResponse> CreateAsync<TCreateInput>(TCreateInput input) where TCreateInput : class
        //{
        //    var entity = _mapper.Map<TEntity>(input);

        //    var enti =_serviceBase.Add(entity);

        //    var response = new ResponseBase<TEntityDto>();
        //    response.Objects = new List<TEntityDto>();
        //    response.Objects.Add(_mapper.Map<TEntityDto>(enti));
        //    return Task.Run(()=> response as TEntityResponse);
        //}

        //public Task DeleteAsync<TDeleteInput>(TDeleteInput input) where TDeleteInput : class
        //{
        //    throw new System.NotImplementedException();
        //}

        //public async Task<TEntityResponse> FindAllAsync()
        //{
        //    var result = await _serviceBase.FindAllAsync();
        //    var response = new ResponseBase<TEntityDto>();
        //    response.Objects = new List<TEntityDto>();
        //    response.Objects.AddRange(result.Select(x => _mapper.Map<TEntityDto>(x)));
        //    return response as TEntityResponse;
        //}

        ////public async Task<TEntityResponse> FindAllAsync()
        ////{
        ////    var result = await _personService.FindAllAsync();
        ////    var response = new PersonResponse();
        ////    response.PersonObjects = new List<PersonDto>();
        ////    response.PersonObjects.AddRange(result.Select(x => _mapper.Map<PersonDto>(x)));
        ////    return response;
        ////}

        //public Task<TEntityResponse> GetAsync<TGetInput>(TGetInput input) where TGetInput : class
        //{
        //    throw new System.NotImplementedException();
        //}

        //public Task<TEntityResponse> UpdateAsync<TUpdateInput>(TUpdateInput input) where TUpdateInput : class
        //{
        //    var entity = _mapper.Map<TEntity>(input);

        //    var enti = _serviceBase.Update(entity);

        //    var response = new ResponseBase<TEntityDto>();
        //    response.Objects = new List<TEntityDto>();
        //    response.Objects.Add(_mapper.Map<TEntityDto>(enti));
        //    return Task.Run(() => response as TEntityResponse);
        //}
    }
}
