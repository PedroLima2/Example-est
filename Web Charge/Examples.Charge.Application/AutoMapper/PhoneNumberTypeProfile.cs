using AutoMapper;
using Examples.Charge.Application.Dtos;
using Examples.Charge.Domain.Aggregates.ExampleAggregate;
using Examples.Charge.Domain.Aggregates.PersonAggregate;

namespace Examples.Charge.Application.AutoMapper
{
    public class PhoneNumberTypeProfile : Profile
    {
        public PhoneNumberTypeProfile()
        {
            CreateMap<PhoneNumberType, PhoneNumberTypeDto>()
               .ReverseMap();
            //.ForMember(dest => dest.BusinessEntityID, opt => opt.MapFrom(src => src.BusinessEntityID))
            //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //.ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));
        }
    }
}
