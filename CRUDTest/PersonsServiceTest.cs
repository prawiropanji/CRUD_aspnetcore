using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CRUDTest
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _countryService = new CountryService(new Entities.PersonDbContext(new DbContextOptionsBuilder().Options));
            _personService = new PersonsService(new Entities.PersonDbContext(new DbContextOptionsBuilder().Options), _countryService);
            _testOutputHelper = testOutputHelper;
        }
        #region Add Person Service
        //when PersonAddRequest is null, it will throw ArgumentNullException
        [Fact]
        public void AddPerson_PersonAddRequestNull()
        {
            //Arange
            PersonAddRequest? personAddRequest = null;


            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            });
        }
        //when PersonName is null, it will throw ArgumentException
        [Fact]
        public void AddPerson_PersonNameNull()
        {
            //Arange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };


            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            });
        }
        //when PersonAddRequest is propper, it will return PersonResponse with new generated Guid PersonId
        [Fact]
        public void AddPerson_PersonAddRequestPropper()
        {
            //Arange
            CountryResponse countryResponse = _countryService.AddCountry(new() { CountryName = "indonesia" });
            Guid countryId = countryResponse.CountryId;

            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "testName", Address = "jl kupag", CountryId = countryId, DateOfBirth = DateTime.Parse("2001-01-02"), Email = "test@mail.com", Gender = GenderOptions.Male, ReceiveNewsLetters = false };

            PersonResponse personResponse_added = _personService.AddPerson(personAddRequest);

            List<PersonResponse> allPersons = _personService.GetAllPersons();

            Assert.True(personResponse_added.PersonId != Guid.Empty);
            Assert.Contains(personResponse_added, allPersons);

          
        }
        #endregion


        #region Get Person By PersonId
        //if personId is null, it should throw ArgumentNullException
        [Fact]
        public void GetPersonById_PersonIdNull()
        {
            //Arrange
            Guid? personId = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.GetPersonByPersonId(personId);
                
            });

        }
        //if personId is propper, it should return the right PersonResponse
        [Fact]
        public void GetPersonById_PersonIdPropper()
        {
            //Arrange
            CountryResponse country_from_add = _countryService.AddCountry(new() { CountryName = "Canada" });
            PersonResponse person_from_add = _personService.AddPerson(new()
            {
                Address = "jl siliwangi",
                CountryId = country_from_add.CountryId,
                DateOfBirth = DateTime.Parse("2002-09-09"),
                PersonName = "nanda",
                Email = "nanda@mail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false,
            });
            

            //Act
            PersonResponse? person_from_get =  _personService.GetPersonByPersonId(person_from_add.PersonId);

            //Assert
            Assert.NotNull(person_from_get);
            Assert.Equal(person_from_add, person_from_get);
           

        }

        #endregion


        #region Get All Persons
        //it should return empty list<PersonResponse> by default
        [Fact]
        public void GetAllPersons_Empty()
        {
            var persons = _personService.GetAllPersons();
            Assert.Empty(persons);
        }
        //few persons added, and it should return the same persons
        [Fact]
        public void GetAllPersons_Propper()
        {
            //Arrange
            CountryResponse usaCountry =  _countryService.AddCountry(new() { CountryName = "USA" });
            CountryResponse ukCountry = _countryService.AddCountry(new() { CountryName = "UK" });

            

            PersonAddRequest person1 = new PersonAddRequest() {
                PersonName = "Sisil",
                Address = "slipi",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("1980-01-06"),
                Email = "sisil@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Bambang",
                Address = "klender",
                CountryId = ukCountry.CountryId,
                DateOfBirth = DateTime.Parse("1990-07-16"),
                Email = "bambang@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };


            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Hana",
                Address = "kuningan",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-15"),
                Email = "hama@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            List<PersonResponse> persons_from_add = new List<PersonResponse>();

            PersonResponse person_response_from_add_1 = _personService.AddPerson(person1);
            persons_from_add.Add(person_response_from_add_1);
            PersonResponse person_response_from_add_2 = _personService.AddPerson(person2);
            persons_from_add.Add(person_response_from_add_2);
            PersonResponse person_response_from_add_3 = _personService.AddPerson(person3);
            persons_from_add.Add(person_response_from_add_3);


            List<PersonResponse> persons_from_get = _personService.GetAllPersons();

            _testOutputHelper.WriteLine("Actual");
            foreach (var person_get in persons_from_get)
            {
                _testOutputHelper.WriteLine(person_get.ToString());
            }

            _testOutputHelper.WriteLine("Expected");
            foreach (var person_add in persons_from_add)
            {
                _testOutputHelper.WriteLine(person_add.ToString());
                Assert.Contains(person_add, persons_from_get);
            }


        }

        #endregion



        #region Get Persons by Filter
        //add few persons; if search with empty filter search and filterby PersonName it will return all list PersonResponse
        [Fact]
        public void GetpersonsByFilter_EmptyFilterSearch()
        {
            //Arrange
            CountryResponse usaCountry = _countryService.AddCountry(new() { CountryName = "USA" });
            CountryResponse ukCountry = _countryService.AddCountry(new() { CountryName = "UK" });



            PersonAddRequest person1 = new PersonAddRequest()
            {
                PersonName = "Sisil",
                Address = "slipi",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("1980-01-06"),
                Email = "sisil@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Bambanag",
                Address = "klender",
                CountryId = ukCountry.CountryId,
                DateOfBirth = DateTime.Parse("1990-07-16"),
                Email = "bambang@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };


            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Hana",
                Address = "kuningan",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-15"),
                Email = "hama@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            List<PersonResponse> persons_from_add = new List<PersonResponse>();

            PersonResponse person_response_from_add_1 = _personService.AddPerson(person1);
            persons_from_add.Add(person_response_from_add_1);
            PersonResponse person_response_from_add_2 = _personService.AddPerson(person2);
            persons_from_add.Add(person_response_from_add_2);
            PersonResponse person_response_from_add_3 = _personService.AddPerson(person3);
            persons_from_add.Add(person_response_from_add_3);

            string filterBy = nameof(PersonResponse.PersonName);
            _testOutputHelper.WriteLine($"filterBy {filterBy}");

            //Act
            var persons_get_filter = _personService.GetPersonsByFilter(filterBy, String.Empty);

            //Assert
            foreach (var person_add in persons_from_add)
            {
                Assert.Contains(person_add, persons_get_filter);
            }

        }

        [Fact]
        public void GetpersonsByFilter_ByPersonName()
        {
            //Arrange
            CountryResponse usaCountry = _countryService.AddCountry(new() { CountryName = "USA" });
            CountryResponse ukCountry = _countryService.AddCountry(new() { CountryName = "UK" });



            PersonAddRequest person1 = new PersonAddRequest()
            {
                PersonName = "Sisil",
                Address = "slipi",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("1980-01-06"),
                Email = "sisil@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Bambanag",
                Address = "klender",
                CountryId = ukCountry.CountryId,
                DateOfBirth = DateTime.Parse("1990-07-16"),
                Email = "bambang@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };


            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Hana",
                Address = "kuningan",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-15"),
                Email = "hama@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            List<PersonResponse> persons_from_add = new List<PersonResponse>();

            PersonResponse person_response_from_add_1 = _personService.AddPerson(person1);
            persons_from_add.Add(person_response_from_add_1);
            PersonResponse person_response_from_add_2 = _personService.AddPerson(person2);
            persons_from_add.Add(person_response_from_add_2);
            PersonResponse person_response_from_add_3 = _personService.AddPerson(person3);
            persons_from_add.Add(person_response_from_add_3);

            string filterBy = nameof(PersonResponse.PersonName);
            _testOutputHelper.WriteLine($"filterBy {filterBy}");

            //Act
            var persons_get_filter = _personService.GetPersonsByFilter(filterBy, "na");

            //Assert
            foreach (var person_add in persons_from_add)
            {
                if(person_add.PersonName != null)
                {
                    if(person_add.PersonName.Contains("na", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_add, persons_get_filter);
                    }
                }

            }

        }

        #endregion


        #region Get Sorted Person
        //invalid sortby or invalid sortorder, it should return same personsToOrder list
        [Fact]
        public void GetSortedPerson_InvalidSortby()
        {
            //Arrange
            CountryResponse usaCountry = _countryService.AddCountry(new() { CountryName = "USA" });
            CountryResponse ukCountry = _countryService.AddCountry(new() { CountryName = "UK" });



            PersonAddRequest person1 = new PersonAddRequest()
            {
                PersonName = "Sisil",
                Address = "slipi",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("1980-01-06"),
                Email = "sisil@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Bambanag",
                Address = "klender",
                CountryId = ukCountry.CountryId,
                DateOfBirth = DateTime.Parse("1990-07-16"),
                Email = "bambang@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };


            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Hana",
                Address = "kuningan",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-15"),
                Email = "hama@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };


            PersonResponse person_response_from_add_1 = _personService.AddPerson(person1);
            PersonResponse person_response_from_add_2 = _personService.AddPerson(person2);
            PersonResponse person_response_from_add_3 = _personService.AddPerson(person3);

            List<PersonResponse> personsToOrder = _personService.GetAllPersons();
            string sortBy = "invalid_prop";
            SortOrderOptions sortOrder = SortOrderOptions.ASC;

            //Act
            var persons_sorted = _personService.GetSortedPersons(personsToOrder, sortBy, sortOrder);

            //Assert
            for (int i = 0; i < persons_sorted.Count; i++) {
                Assert.Equal(personsToOrder[i], persons_sorted[i]);
            }

        }
        //sortby person name and sortOrder by desc it should return descending ordered of property personName of personsToOrder   
        [Fact]
        public void GetSortedPerson_SortByNameDesc()
        {
            //Arrange
            CountryResponse usaCountry = _countryService.AddCountry(new() { CountryName = "USA" });
            CountryResponse ukCountry = _countryService.AddCountry(new() { CountryName = "UK" });



            PersonAddRequest person1 = new PersonAddRequest()
            {
                PersonName = "Sisil",
                Address = "slipi",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("1980-01-06"),
                Email = "sisil@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Bambanag",
                Address = "klender",
                CountryId = ukCountry.CountryId,
                DateOfBirth = DateTime.Parse("1990-07-16"),
                Email = "bambang@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };


            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Hana",
                Address = "kuningan",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-15"),
                Email = "hama@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };


            PersonResponse person_response_from_add_1 = _personService.AddPerson(person1);
            PersonResponse person_response_from_add_2 = _personService.AddPerson(person2);
            PersonResponse person_response_from_add_3 = _personService.AddPerson(person3);
            
            List<PersonResponse> persons = _personService.GetAllPersons();
            var persons_sorted_expected = persons.OrderByDescending((p) => p.PersonName).ToList();

            string sortBy = nameof(PersonResponse.PersonName);
            SortOrderOptions sortOrder = SortOrderOptions.DESC;

            //Act
            var persons_sorted_actual = _personService.GetSortedPersons(persons, sortBy, sortOrder);

            //Assert
            for (int i = 0; i < persons_sorted_actual.Count; i++)
            {
                Assert.Equal(persons_sorted_expected[i], persons_sorted_actual[i]);
            }

        }
        #endregion

        #region Update Person
        //if PersonUpdateRequest is null, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_PersonUpdateRequestNull()
        {
            //Arrange
            PersonUpdateRequest? person_update = null;


            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                var persons_sorted_actual = _personService.UpdatePerson(person_update);
            });

        }
        //if PersonId is not valid or not found, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_PersonIdNotFound()
        {
            //Arrange
            CountryResponse usaCountry = _countryService.AddCountry(new() { CountryName = "USA" });
            CountryResponse ukCountry = _countryService.AddCountry(new() { CountryName = "UK" });



            PersonAddRequest person1 = new PersonAddRequest()
            {
                PersonName = "Sisil",
                Address = "slipi",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("1980-01-06"),
                Email = "sisil@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Bambanag",
                Address = "klender",
                CountryId = ukCountry.CountryId,
                DateOfBirth = DateTime.Parse("1990-07-16"),
                Email = "bambang@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };


            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Hana",
                Address = "kuningan",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-15"),
                Email = "hama@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };


            PersonResponse person_response_from_add_1 = _personService.AddPerson(person1);
            PersonResponse person_response_from_add_2 = _personService.AddPerson(person2);
            PersonResponse person_response_from_add_3 = _personService.AddPerson(person3);

            PersonUpdateRequest? person_update = new PersonUpdateRequest()
            {
                PersonId = Guid.NewGuid(),
                PersonName = person_response_from_add_1.PersonName,
                Address = person_response_from_add_1.Address,
                CountryId = person_response_from_add_1.CountryId,
                DateOfBirth = person_response_from_add_1.DateOfBirth,
                Email = person_response_from_add_1.Email,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), person_response_from_add_1.Gender ?? "", true),
                ReceiveNewsLetters = person_response_from_add_1.ReceiveNewsLetters
            };

         


            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                var updated_person = _personService.UpdatePerson(person_update);
            });

        }
        //if PersonName is empty, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_PersonNameEmpty()
        {
            //Arrange
            CountryResponse usaCountry = _countryService.AddCountry(new() { CountryName = "USA" });
            CountryResponse ukCountry = _countryService.AddCountry(new() { CountryName = "UK" });



            PersonAddRequest person1 = new PersonAddRequest()
            {
                PersonName = "Sisil",
                Address = "slipi",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("1980-01-06"),
                Email = "sisil@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Bambanag",
                Address = "klender",
                CountryId = ukCountry.CountryId,
                DateOfBirth = DateTime.Parse("1990-07-16"),
                Email = "bambang@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };


            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Hana",
                Address = "kuningan",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-15"),
                Email = "hama@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };


            PersonResponse person_response_from_add_1 = _personService.AddPerson(person1);
            PersonResponse person_response_from_add_2 = _personService.AddPerson(person2);
            PersonResponse person_response_from_add_3 = _personService.AddPerson(person3);

            PersonUpdateRequest? person_update = new PersonUpdateRequest()
            {
                PersonId = person_response_from_add_1.PersonId,
                PersonName = string.Empty,
                Address = person_response_from_add_1.Address,
                CountryId = person_response_from_add_1.CountryId,
                DateOfBirth = person_response_from_add_1.DateOfBirth,
                Email = person_response_from_add_1.Email,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), person_response_from_add_1.Gender ?? "", true),
                ReceiveNewsLetters = person_response_from_add_1.ReceiveNewsLetters
            };



            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                var updated_person = _personService.UpdatePerson(person_update);
            });

        }
        //supplied propper PersonUpdateRequest, it should return updated PersonResponse
        [Fact]
        public void UpdatePerson_updateValidPersonId()
        {
            //Arrange
            CountryResponse usaCountry = _countryService.AddCountry(new() { CountryName = "USA" });
            CountryResponse ukCountry = _countryService.AddCountry(new() { CountryName = "UK" });



            PersonAddRequest person1 = new PersonAddRequest()
            {
                PersonName = "Sisil",
                Address = "slipi",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("1980-01-06"),
                Email = "sisil@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Bambanag",
                Address = "klender",
                CountryId = ukCountry.CountryId,
                DateOfBirth = DateTime.Parse("1990-07-16"),
                Email = "bambang@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };


            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Hana",
                Address = "kuningan",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-15"),
                Email = "hama@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };


            PersonResponse person_response_from_add_1 = _personService.AddPerson(person1);
            PersonResponse person_response_from_add_2 = _personService.AddPerson(person2);
            PersonResponse person_response_from_add_3 = _personService.AddPerson(person3);

            PersonUpdateRequest? person_update = new PersonUpdateRequest()
            {
                PersonId = person_response_from_add_1.PersonId,
                PersonName = "Gyan",
                Address = person_response_from_add_1.Address,
                CountryId = person_response_from_add_1.CountryId,
                DateOfBirth = person_response_from_add_1.DateOfBirth,
                Email = "Gyan@exaple.com",
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), person_response_from_add_1.Gender ?? "", true),
                ReceiveNewsLetters = person_response_from_add_1.ReceiveNewsLetters
            };

            //Act
            var updated_person_actual = _personService.UpdatePerson(person_update);
            var person_expected = _personService.GetPersonByPersonId(person_update.PersonId);


            //Assert
            Assert.Equal(person_expected, updated_person_actual);

        }
        #endregion

        #region Delete Person
        //if PersonId is null it should return false
        [Fact]
        public void DeletePerson_PersonIdNull()
        {
            //Arrange
            Guid? personId = null;

            //Act
            bool isDeleted = _personService.DeletePerson(personId);

            //Assert
            Assert.False(isDeleted);
        }
        //if PersonId is not valid it should return false
        [Fact]
        public void DeletePerson_PersonIdNotValid()
        {
            //Arrange
            Guid? personId = Guid.NewGuid();

            //Act
            bool isDeleted = _personService.DeletePerson(personId);

            //Assert
            Assert.False(isDeleted);
        }
        //if supplied valid PersonId, it should return true
        [Fact]
        public void DeletePerson_PersonIdValid()
        {
            //Arrange
            CountryResponse usaCountry = _countryService.AddCountry(new() { CountryName = "USA" });
            CountryResponse ukCountry = _countryService.AddCountry(new() { CountryName = "UK" });



            PersonAddRequest person1 = new PersonAddRequest()
            {
                PersonName = "Sisil",
                Address = "slipi",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("1980-01-06"),
                Email = "sisil@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };

            PersonAddRequest person2 = new PersonAddRequest()
            {
                PersonName = "Bambanag",
                Address = "klender",
                CountryId = ukCountry.CountryId,
                DateOfBirth = DateTime.Parse("1990-07-16"),
                Email = "bambang@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };


            PersonAddRequest person3 = new PersonAddRequest()
            {
                PersonName = "Hana",
                Address = "kuningan",
                CountryId = usaCountry.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-15"),
                Email = "hama@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            };


            PersonResponse person_response_from_add_1 = _personService.AddPerson(person1);
            PersonResponse person_response_from_add_2 = _personService.AddPerson(person2);
            PersonResponse person_response_from_add_3 = _personService.AddPerson(person3);

            Guid personIdToDelete = person_response_from_add_1.PersonId;

            //Act
            bool isDeleted = _personService.DeletePerson(personIdToDelete);

            //Assert
            var getAllPersons = _personService.GetAllPersons();
            Assert.DoesNotContain(person_response_from_add_1, getAllPersons );
            Assert.True(isDeleted);
            
        }
        #endregion
    }

}
