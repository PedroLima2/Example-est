using AutoMapper;
using Examples.Charge.Application.Dtos;
using Examples.Charge.Application.Interfaces;
using Examples.Charge.Application.Messages.Response;
using Examples.Charge.Domain.Aggregates.ExampleAggregate;
using Examples.Charge.Domain.Aggregates.ExampleAggregate.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examples.Charge.Application.Facade
{
    public class ExampleFacade : FacadeBase<Example, ExampleDto>, IExampleFacade
    {
        private IExampleService _exampleService;
        private IMapper _mapper;

        public ExampleFacade(IExampleService exampleService, IMapper mapper) : base(exampleService, mapper)
        {
            _exampleService = exampleService;
            _mapper = mapper;
        }

        //public async Task<ExampleListResponse> FindAllAsync()
        //{
        //    var result = await _exampleService.FindAllAsync();
        //    var response = new ExampleListResponse();
        //    response.ExampleObjects = new List<ExampleDto>();
        //    response.ExampleObjects.AddRange(result.Select(x => _mapper.Map<ExampleDto>(x)));
        //    return response;
        //}
    }
}
