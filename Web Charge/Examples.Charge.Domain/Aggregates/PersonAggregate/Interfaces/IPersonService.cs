using Examples.Charge.Domain.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Charge.Domain.Aggregates.PersonAggregate.Interfaces
{
    public interface IPersonService : IServiceBase<Person>
    {
        //Task<List<Person>> FindAllAsync();
    }
}
