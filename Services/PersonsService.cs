using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly PersonDbContext _db;
        private readonly ICountriesService _CountriesService;
        public PersonsService(PersonDbContext personDbContext, ICountriesService countriesService)
        {
            _db = personDbContext;
            _CountriesService = countriesService;


          

        }

        public PersonResponse ConverToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            return personResponse;  
        }
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }


            //model validation
            ValidationHelper.ValidateObject(personAddRequest);


            Person person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();
            //_db.sp_InsertPerson(person);


            PersonResponse personResponse = person.ToPersonResponse();
            //personResponse.CountryName = await _CountriesService.GetCountryById(personResponse.CountryId)?.CountryName;
            return personResponse;


        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId is null)
            {
                return false;
            }


            Person? person = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == personId);
            if (person is null) {
                return false;
            }

           

            var deletedPersons = _db.Persons.Remove(person);
            if (deletedPersons is null)
            {
                return false;
            }

            await _db.SaveChangesAsync();

            return true;

        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            //return _db.Sp_GetAllPersons().Select(p => ConverToPersonResponse(p)).ToList();
            List<Person> persons = await _db.Persons.Include(p => p.country).ToListAsync();
            return persons.Select((p) =>  ConverToPersonResponse(p)).ToList();

        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == personId);
            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();

        }

        public async Task<List<PersonResponse>> GetPersonsByFilter(string? filterBy, string? filterSearch)
        {
            List<PersonResponse> listPersons = await GetAllPersons();
            if (string.IsNullOrEmpty(filterSearch) || string.IsNullOrEmpty(filterBy))
            {
                return listPersons;
            }

            switch (filterBy)
            {
                case nameof(PersonResponse.PersonName):
                    listPersons = listPersons.Where((p) =>
                     {
                         return (!string.IsNullOrEmpty(p.PersonName)) ? p.PersonName.Contains(filterSearch, StringComparison.OrdinalIgnoreCase) : true;
                     }).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    listPersons = listPersons.Where((p) =>
                    {
                        return (!string.IsNullOrEmpty(p.Email)) ? p.Email.Contains(filterSearch, StringComparison.OrdinalIgnoreCase) : true;
                    }).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    listPersons = listPersons.Where((p) =>
                    {
                        return (p.DateOfBirth != null) ? p.DateOfBirth.GetValueOrDefault().ToString("dd MMM yyyy").Contains(filterSearch, StringComparison.OrdinalIgnoreCase) : true;
                    }).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    listPersons = listPersons.Where((p) =>
                    {
                        return (!string.IsNullOrEmpty(p.Gender)) ? p.Gender.Contains(filterSearch, StringComparison.OrdinalIgnoreCase) : true;
                    }).ToList();
                    break;
                case nameof(PersonResponse.CountryName):
                    listPersons = listPersons.Where((p) =>
                    {
                        return (!string.IsNullOrEmpty(p.CountryName)) ? p.CountryName.Contains(filterSearch, StringComparison.OrdinalIgnoreCase) : true;
                    }).ToList();
                    break;

            }

            return listPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> personsToOrder, string sortBy, SortOrderOptions sortOrder)
        {
            List<PersonResponse> sorted_persons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => personsToOrder.OrderBy(p => p.PersonName).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => personsToOrder.OrderByDescending(p => p.PersonName).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => personsToOrder.OrderBy(p => p.Email).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => personsToOrder.OrderByDescending(p => p.Email).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => personsToOrder.OrderBy(p => p.Gender).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => personsToOrder.OrderByDescending(p => p.Gender).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => personsToOrder.OrderBy(p => p.Address).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => personsToOrder.OrderByDescending(p => p.Address).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => personsToOrder.OrderBy(p => p.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => personsToOrder.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.CountryName), SortOrderOptions.ASC) => personsToOrder.OrderBy(p => p.CountryName).ToList(),
                (nameof(PersonResponse.CountryName), SortOrderOptions.DESC) => personsToOrder.OrderByDescending(p => p.CountryName).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => personsToOrder.OrderBy(p => p.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => personsToOrder.OrderByDescending(p => p.Age).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => personsToOrder.OrderBy(p => p.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => personsToOrder.OrderByDescending(p => p.DateOfBirth).ToList(),

                _ => personsToOrder.ToList(),
            };

            return sorted_persons;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            //validation
            ValidationHelper.ValidateObject(personUpdateRequest);


            Person? person_found = await _db.Persons.FirstOrDefaultAsync(p => p.PersonId == personUpdateRequest.PersonId);
            if (person_found is null )
            {
                throw new ArgumentException($"Person with PersonId {personUpdateRequest.PersonId} was not found");
            }

            person_found.PersonName = personUpdateRequest.PersonName;
            person_found.Email = personUpdateRequest.Email;
            person_found.DateOfBirth = personUpdateRequest.DateOfBirth;
            person_found.Gender = personUpdateRequest.Gender.ToString();
            person_found.CountryId = personUpdateRequest.CountryId;
            person_found.Address = personUpdateRequest.Address;
            person_found.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            await _db.SaveChangesAsync();

            return person_found.ToPersonResponse();


        }
    }
}
