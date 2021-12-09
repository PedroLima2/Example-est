using Examples.Charge.Domain.Aggregates.PersonAggregate;
using Examples.Charge.Domain.Aggregates.PersonAggregate.Interfaces;
using Examples.Charge.Domain.Base.Interfaces;
using Examples.Charge.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examples.Charge.Infra.Data.Repositories
{
    public class PhoneNumberTypeRepository : RepositoryBase<PhoneNumberType>, IPhoneNumberTypeRepository
    {
        private readonly ExampleContext _context;

        public PhoneNumberTypeRepository(ExampleContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //public async Task<IEnumerable<PhoneNumberType>> FindAllAsync() => await Task.Run(() => _context.PhoneNumberType);
    }
}
