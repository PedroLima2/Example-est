using Examples.Charge.Domain.Aggregates.PersonAggregate;
using Examples.Charge.Domain.Aggregates.PersonAggregate.Interfaces;
using Examples.Charge.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examples.Charge.Infra.Data.Repositories
{
    public class PersonPhoneRepository : RepositoryBase<PersonPhone>, IPersonPhoneRepository
    {
        private readonly ExampleContext _context;

        public PersonPhoneRepository(ExampleContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override PersonPhone Add(PersonPhone Entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {

                using (var repoPhoneNumberType = new PhoneNumberTypeRepository(_context))
                {
                    if (Entity.PhoneNumberType != null)
                    {
                        var phoneNumberType = repoPhoneNumberType.AddOrUpdate(Entity.PhoneNumberType);

                        Entity.PhoneNumberTypeID = phoneNumberType.PhoneNumberTypeID;

                    }

                    var entity = base.Add(Entity);

                    transaction.Commit();

                    return entity;
                }
            }
        }
    }
}
