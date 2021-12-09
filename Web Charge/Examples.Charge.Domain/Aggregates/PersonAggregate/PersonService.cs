using Examples.Charge.Domain.Aggregates.PersonAggregate.Interfaces;
using Examples.Charge.Domain.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examples.Charge.Domain.Aggregates.PersonAggregate
{
    public class PersonService : ServiceBase<Person>, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        public PersonService(IPersonRepository personRepository) : base(personRepository)
        {
            _personRepository = personRepository;

        }

        //public async Task<List<Person>> FindAllAsync() => (await _personRepository.FindAllAsync()).ToList();
    }
}
