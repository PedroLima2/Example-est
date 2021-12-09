using Examples.Charge.Domain.Aggregates.PersonAggregate.Interfaces;
using Examples.Charge.Domain.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Charge.Domain.Aggregates.ExampleAggregate.Interfaces
{
    public interface IExampleRepository : IRepositoryBase<Example>
    {
        //Task<IEnumerable<Example>> FindAllAsync();
    }
}
