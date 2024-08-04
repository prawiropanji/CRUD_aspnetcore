using Entities;
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
        private readonly List<Person> _listPerson;
        private readonly ICountriesService _CountriesService;
        public PersonsService(bool initialize = true)
        {
            _listPerson = new List<Person>();
            _CountriesService = new CountryService();


            if (initialize)
            {
                //create mock data
                _listPerson = new List<Person>() {
                    new Person() {
                        PersonId = Guid.Parse("17d5b155-46b7-4d91-928f-89f79d7d30dc"),
                        PersonName = "Fairfax",
                        Address = "5 Fuller Trail",
                        DateOfBirth = DateTime.Parse("14/10/2010"),
                        Email = "feadon0@mlb.com",
                        Gender = "Male",
                        ReceiveNewsLetters = true,
                        CountryId = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E")
                    },

                     new Person() {
                        PersonId = Guid.Parse("89ebb632-a89b-4e50-8692-0d52d5e59c09"),
                        PersonName = "Zuzana",
                        Address = "07 Anzinger Pass",
                        DateOfBirth = DateTime.Parse("11/04/2020"),
                        Email = "zgayden1@printfriendly.com",
                        Gender = "Female",
                        ReceiveNewsLetters = true,
                        CountryId = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E")
                    },
                        new Person() {
                            PersonId = Guid.Parse("8c775cdb-bcae-4a5a-94f7-ec13338fba65"),
                            PersonName = "Rory",
                            Address = "1968 Clarendon Junction",
                            DateOfBirth = DateTime.Parse("27/01/2015"),
                            Email = "raughtie2@mtv.com",
                            Gender = "Female",
                            ReceiveNewsLetters = false,
                            CountryId = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E")
                        },
                    new Person() {
                        PersonId = Guid.Parse("798d5d97-989d-43e7-bc59-ddd87124152f"),
                        PersonName = "Friedrick",
                        Address = "0705 John Wall Road",
                        DateOfBirth = DateTime.Parse("19/05/2015"),
                        Email = "fdonan3@statcounter.com",
                        Gender = "Male",
                        ReceiveNewsLetters = false,
                        CountryId = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E")
                    },
                    new Person() {
                        PersonId = Guid.Parse("d3f276a4-f5ea-44ef-b9ab-34db0efc9615"),
                        PersonName = "Gabrielle",
                        Address = "25849 Ridge Oak Crossing",
                        DateOfBirth = DateTime.Parse("15/07/1998"),
                        Email = "gmacandreis4@themeforest.net",
                        Gender = "Female",
                        ReceiveNewsLetters = false,
                        CountryId = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E")
                    },
                    new Person() {
                        PersonId = Guid.Parse("bf16de25-6ce3-4b3d-901c-1d0419a7d136"),
                        PersonName = "Ossie",
                        Address = "01253 Mosinee Street",
                        DateOfBirth = DateTime.Parse("09/03/2002"),
                        Email = "ogriffe5@whitehouse.gov",
                        Gender = "Male",
                        ReceiveNewsLetters = true,
                        CountryId = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E")
                    },
                    new Person() {
                        PersonId = Guid.Parse("3d2f8cf6-43d9-4d09-bcc5-7bffbdfe497f"),
                        PersonName = "Bobette",
                        Address = "8787 Springview Park",
                        DateOfBirth = DateTime.Parse("09/02/2009"),
                        Email = "bkingdon6@weather.com",
                        Gender = "Female",
                        ReceiveNewsLetters = false,
                        CountryId = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E")
                    },
                    new Person() {
                        PersonId = Guid.Parse("4f56539a-9e3f-42dd-b5c8-7ccd0c572a55"),
                        PersonName = "Marion",
                        Address = "4480 Delaware Trail",
                        DateOfBirth = DateTime.Parse("04/09/2004"),
                        Email = "mkitchingham7@creativecommons.org",
                        Gender = "Male",
                        ReceiveNewsLetters = true,
                        CountryId = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E")
                    },
                    new Person() {
                        PersonId = Guid.Parse("3d61b135-3aab-4503-b2bc-434e38d1da58"),
                        PersonName = "Michaeline",
                        Address = "6 Lindbergh Circle",
                        DateOfBirth = DateTime.Parse("18/01/1993"),
                        Email = "maloway8@webnode.com",
                        Gender = "Female",
                        ReceiveNewsLetters = false,
                        CountryId = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E")
                    },
                    new Person() {
                        PersonId = Guid.Parse("13fe2c31-f2d0-4503-b84d-904cc2b63ee3"),
                        PersonName = "Billie",
                        Address = "77 Shelley Circle",
                        DateOfBirth = DateTime.Parse("05/11/1991"),
                        Email = "bstringfellow9@tinyurl.com",
                        Gender = "Female",
                        ReceiveNewsLetters = false,
                        CountryId = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E")
                    }

                };


            }

        }

        public PersonResponse ConverToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.CountryName = _CountriesService.GetCountryById(person.CountryId)?.CountryName;
            return personResponse;  
        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }


            //model validation
            ValidationHelper.ValidateObject(personAddRequest);


            Person person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();
            _listPerson.Add(person);


            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.CountryName = _CountriesService.GetCountryById(personResponse.CountryId)?.CountryName;
            return personResponse;


        }

        public bool DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                return false;
            }

            var deletedPersons = _listPerson.RemoveAll(p => p.PersonId == personId);
            if (deletedPersons == 0)
            {
                return false;
            }

            return true;

        }

        public List<PersonResponse> GetAllPersons()
        {
            return _listPerson.Select(p => ConverToPersonResponse(p)).ToList();
        }

        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = _listPerson.FirstOrDefault(p => p.PersonId == personId);
            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();

        }

        public List<PersonResponse> GetPersonsByFilter(string? filterBy, string? filterSearch)
        {
            List<PersonResponse> listPersons = GetAllPersons();
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
                        return (!string.IsNullOrEmpty(p.CountryName)) ? p.CountryName.Contains(filterBy, StringComparison.OrdinalIgnoreCase) : true;
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

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            //validation
            ValidationHelper.ValidateObject(personUpdateRequest);


            int found_index = _listPerson.FindIndex(p => p.PersonId == personUpdateRequest.PersonId);
            if (found_index < 0)
            {
                throw new ArgumentException($"Person with PersonId {personUpdateRequest.PersonId} was not found");
            }


            _listPerson[found_index].PersonName = personUpdateRequest.PersonName;
            _listPerson[found_index].Email = personUpdateRequest.Email;
            _listPerson[found_index].DateOfBirth = personUpdateRequest.DateOfBirth;
            _listPerson[found_index].Gender = personUpdateRequest.Gender.ToString();
            _listPerson[found_index].CountryId = personUpdateRequest.CountryId;
            _listPerson[found_index].Address = personUpdateRequest.Address;
            _listPerson[found_index].ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return _listPerson[found_index].ToPersonResponse();


        }
    }
}
