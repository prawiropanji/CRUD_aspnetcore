using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly ApplicationDbContext _db;

        public PersonsRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Person> AddPerson(Person person)
        {
            await _db.AddAsync(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePerson(Guid personId)
        {
            Person? person = await _db.Persons.FirstOrDefaultAsync(x => x.PersonId == personId);
            _db.Persons.Remove(person!);
            int rowsDeleted =  await _db.SaveChangesAsync();
            return rowsDeleted > 0;
                
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _db.Persons.Include(p => p.country).ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons.Include(p => p.country).Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonId(Guid personId)
        {
            return await _db.Persons.Include(p => p.country).FirstOrDefaultAsync(x => x.PersonId == personId);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            var matchingPerson  = _db.Persons.FirstOrDefault(p => p.PersonId == person.PersonId);

            matchingPerson!.PersonName = person.PersonName;
            matchingPerson!.Email = person.Email;
            matchingPerson!.DateOfBirth = person.DateOfBirth;
            matchingPerson!.Gender = person.Gender;
            matchingPerson!.CountryId = person.CountryId;
            matchingPerson!.Address = person.Address;
            matchingPerson!.ReceiveNewsLetters = person.ReceiveNewsLetters;
            matchingPerson!.TIN = person.TIN;

            await _db.SaveChangesAsync();
            return matchingPerson;
        }
    }
}
