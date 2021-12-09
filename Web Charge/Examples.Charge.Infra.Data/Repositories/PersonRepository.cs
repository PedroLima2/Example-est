using Examples.Charge.Domain.Aggregates.PersonAggregate;
using Examples.Charge.Domain.Aggregates.PersonAggregate.Interfaces;
using Examples.Charge.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examples.Charge.Infra.Data.Repositories
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        private readonly ExampleContext _context;

        public PersonRepository(ExampleContext context) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override Person Add(Person Entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var entity = base.Add(Entity);

                using (var repoPersonPhoneNumber = new PersonPhoneRepository(_context))
                {
                    using (var repoPhoneNumberType = new PhoneNumberTypeRepository(_context))
                    {
                        foreach (var phone in Entity.Phones)
                        {
                            var phoneNumberType = repoPhoneNumberType.AddOrUpdate(phone.PhoneNumberType);

                            phone.PhoneNumberTypeID = phoneNumberType.PhoneNumberTypeID;
                            phone.BusinessEntityID = entity.BusinessEntityID;

                            repoPersonPhoneNumber.AddOrUpdate(phone);
                        }
                        transaction.Commit();
                    }
                }


                return entity;
            }
        }

        public override Person Update(Person Entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var entity = base.AddOrUpdate(Entity);

                using (var repoPersonPhoneNumber = new PersonPhoneRepository(_context))
                {
                    using (var repoPhoneNumberType = new PhoneNumberTypeRepository(_context))
                    {
                        foreach (var phone in Entity.Phones)
                        {
                            var phoneNumberType = repoPhoneNumberType.AddOrUpdate(phone.PhoneNumberType);

                            phone.PhoneNumberTypeID = phoneNumberType.PhoneNumberTypeID;
                            phone.BusinessEntityID = entity.BusinessEntityID;

                            repoPersonPhoneNumber.AddOrUpdate(phone);
                        }
                        transaction.Commit();
                    }
                }

                return entity;
            }
        }

        public override IQueryable<Person> GetQuery()
        {
            return base.GetQuery().Include(x => x.Phones);
        }

        public override Person GetById(int id)
        {
            return GetQuery().FirstOrDefault(x => x.BusinessEntityID == id);
        }

        //public async Task<IEnumerable<Person>> FindAllAsync() => await Task.Run(() => _context.Person);
    }
}
