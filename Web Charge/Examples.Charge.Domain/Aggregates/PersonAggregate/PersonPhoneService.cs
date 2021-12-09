using Examples.Charge.Domain.Aggregates.PersonAggregate.Interfaces;
using Examples.Charge.Domain.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examples.Charge.Domain.Aggregates.PersonAggregate
{
    public class PersonPhoneService : ServiceBase<PersonPhone>, IPersonPhoneService
    {
        private readonly IPersonPhoneRepository _personPhoneRepository;
        public PersonPhoneService(IPersonPhoneRepository personPhoneRepository) : base(personPhoneRepository)
        {
            _personPhoneRepository = personPhoneRepository;
        }

        //public async Task<List<PersonPhone>> FindAllAsync() => (await _personPhoneRepository.FindAllAsync()).ToList();
    }
}
