using AutoMapper;
using Examples.Charge.Application.Dtos;
using Examples.Charge.Application.Interfaces;
using Examples.Charge.Domain.Aggregates.PersonAggregate;
using Examples.Charge.Domain.Aggregates.PersonAggregate.Interfaces;

namespace Examples.Charge.Application.Facade
{
    public class PersonPhoneFacade : FacadeBase<PersonPhone, PersonPhoneDto>, IPersonPhoneFacade
    {
        private readonly IPersonPhoneService _personPhoneService;
        private readonly IMapper _mapper;

        public PersonPhoneFacade(IPersonPhoneService personPhoneService, IMapper mapper) : base(personPhoneService, mapper)
        {
            _personPhoneService = personPhoneService;
            _mapper = mapper;
        }
    }
}
