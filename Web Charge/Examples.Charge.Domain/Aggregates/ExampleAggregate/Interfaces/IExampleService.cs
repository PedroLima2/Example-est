using Examples.Charge.Domain.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examples.Charge.Domain.Aggregates.ExampleAggregate.Interfaces
{
    public interface IExampleService : IServiceBase<Example>
    {
        //Task<List<Example>> FindAllAsync();
    }
}
