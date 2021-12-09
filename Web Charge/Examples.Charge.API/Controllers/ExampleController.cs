using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Examples.Charge.Application.Interfaces;
using Examples.Charge.Application.Messages.Request;
using Examples.Charge.Application.Messages.Response;
using System.Threading.Tasks;
using Examples.Charge.Application.Dtos;

namespace Examples.Charge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : BaseController<ExampleDto>
    {
        private IExampleFacade _facade;

        public ExampleController(IExampleFacade facade, IMapper mapper)
        {
            _facade = facade;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Response(await _facade.FindAllAsync());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Response(null);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ExampleDto request)
        {
            return Response(0, null);
        }
    }
}
