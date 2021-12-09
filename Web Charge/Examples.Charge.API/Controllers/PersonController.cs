using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Examples.Charge.Application.Interfaces;
using Examples.Charge.Application.Messages.Request;
using Examples.Charge.Application.Messages.Response;
using System.Threading.Tasks;
using Examples.Charge.Application.Facade;
using Examples.Charge.Application.Common.Messages;
using Examples.Charge.Application.Dtos;
using System;
using System.Linq;

namespace Examples.Charge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : BaseController<PersonDto>
    {
        private IPersonFacade _facade;

        public PersonController(IPersonFacade facade, IMapper mapper)
        {
            _facade = facade;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Response(await _facade.FindAllAsync());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Response(_facade.GetById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] PersonDto request)
        {
            var entity = _facade.Add(request);

            return Response(0, entity);
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] PersonDto request)
        {
            if (id != request.BusinessEntityID)
            {
                return Response(null);
            }

            var entity = _facade.Update(request);

            return Response(entity);
        }

    }
}
