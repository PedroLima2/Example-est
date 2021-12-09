using AutoMapper;
using Examples.Charge.Application.Dtos;
using Examples.Charge.Application.Interfaces;
using Examples.Charge.Domain.Aggregates.PersonAggregate;
using Examples.Charge.Domain.Aggregates.PersonAggregate.Interfaces;

namespace Examples.Charge.Application.Facade
{
    public class PhoneNumberTypeFacade : FacadeBase<PhoneNumberType, PhoneNumberTypeDto>, IPhoneNumberTypeFacade
    {
        private readonly IPhoneNumberTypeService _phoneNumberTypeService;
        private readonly IMapper _mapper;

        public PhoneNumberTypeFacade(IPhoneNumberTypeService phoneNumberTypeService, IMapper mapper) : base(phoneNumberTypeService, mapper)
        {
            _phoneNumberTypeService = phoneNumberTypeService;
            _mapper = mapper;
        }
    }
}
