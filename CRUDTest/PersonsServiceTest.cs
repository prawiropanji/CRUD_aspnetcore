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
using Entities;
using EntityFrameworkCoreMock;
using AutoFixture;
using FluentAssertions;
using AutoFixture.Kernel;
using Moq;
using RepositoryContracts;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CRUDTest
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ILogger<PersonsService> _loggerService;
        private readonly IDiagnosticContext _diagnosticService;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Mock<IPersonsRepository> _personRepositoryMock;
        private readonly Mock<ILogger<PersonsService>> _loggerMock;
        private readonly Mock<IDiagnosticContext> _diagnosticMock;

        private readonly IFixture _fixture;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            //create mock object for repository
            _personRepositoryMock = new Mock<IPersonsRepository>();
            _loggerMock = new Mock<ILogger<PersonsService>>();
            _diagnosticMock = new Mock<IDiagnosticContext>();

            //access actual mocked repository instance
            IPersonsRepository _personRepository = _personRepositoryMock.Object;
            ILogger<PersonsService> _logger = _loggerMock.Object;
            IDiagnosticContext _diagnostic = _diagnosticMock.Object;

            //create service object with mocked repository
            _personService = new PersonsService(_personRepository, _logger, _diagnostic);
            _testOutputHelper = testOutputHelper;
            _fixture = new Fixture();

        }
        #region Add Person Service
        //when PersonAddRequest is null, it will throw ArgumentNullException
        [Fact]
        public async Task AddPerson_PersonAddRequestNull()
        {
            //Arange
            PersonAddRequest? personAddRequest = null;


            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            // {
            //     //Act
            //     await _personService.AddPerson(personAddRequest);
            // });

            Func<Task> action = async () =>
            {
                //Act
                await _personService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();

        }
        //when PersonName is null, it will throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameNull()
        {
            //Arange
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();


            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await _personService.AddPerson(personAddRequest);
            //});

            Func<Task> action = async () =>
            {
                //Act
                await _personService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();

        }
        //when PersonAddRequest is propper, it will return PersonResponse with new generated Guid PersonId
        [Fact]
        public async Task AddPerson_PersonAddRequestPropper_ToBeSuccessful()
        {
            //Arange

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "test@mail.com").Create();

            Person person = personAddRequest.ToPerson();
            PersonResponse personResponse_expected = person.ToPersonResponse();

            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            PersonResponse personResponse_added = await _personService.AddPerson(personAddRequest);

            personResponse_expected.PersonId = personResponse_added.PersonId;



            //Assert
            //Assert.True(personResponse_added.PersonId != Guid.Empty);
            //Assert.Contains(personResponse_added, allPersons);

            personResponse_added.PersonId.Should().NotBe(Guid.Empty);
            personResponse_added.Should().BeEquivalentTo(personResponse_expected);


        }
        #endregion


        #region Get Person By PersonId
        //if personId is null, it should throw ArgumentNullException
        [Fact]
        public async Task GetPersonById_PersonIdNull()
        {
            //Arrange
            Guid? personId = null;

            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    await _personService.GetPersonByPersonId(personId);

            //});

            Func<Task> action = async () =>
            {
                await _personService.GetPersonByPersonId(personId);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();

        }
        //if personId is propper, it should return the right PersonResponse
        [Fact]
        public async Task GetPersonById_PersonIdPropper()
        {
            //Arrange
            //CountryResponse country_from_add = await _countryService.AddCountry(_fixture.Create<CountryAddRequest>());
            //PersonResponse person_from_add = await _personService.AddPerson(_fixture.Build<PersonAddRequest>().With(temp => temp.Email, "nanda@mail.com").Create());

            Person person = _fixture.Build<Person>().With(x => x.country, null as Country).Create();
            PersonResponse person_response_expected = person.ToPersonResponse();

            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act
            PersonResponse? person_from_get = await _personService.GetPersonByPersonId(person.PersonId);

            //Assert
            //Assert.NotNull(person_from_get);
            //Assert.Equal(person_from_add, person_from_get);

            person_from_get.Should().NotBeNull();
            person_from_get.Should().Be(person_response_expected);


        }

        #endregion


        #region Get All Persons
        //it should return empty list<PersonResponse> by default
        [Fact]
        public async Task GetAllPersons_Empty()
        {
            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(new List<Person>());

            var persons = await _personService.GetAllPersons();
            //Assert.Empty(persons);

            persons.Should().BeEmpty();
        }
        //few persons added, and it should return the same persons
        [Fact]
        public async Task GetAllPersons_Propper()
        {
            //Arrange


            List<Person> persons = new List<Person>()
            {
                _fixture.Build<Person>().With(temp => temp.country, null as Country).Create(),
                _fixture.Build<Person>().With(temp => temp.country, null as Country).Create(),
                _fixture.Build<Person>().With(temp => temp.country, null as Country).Create()

            };


            _personRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);


            List<PersonResponse> persons_from_get = await _personService.GetAllPersons();

            _testOutputHelper.WriteLine("Actual");
            foreach (var person_get in persons_from_get)
            {
                _testOutputHelper.WriteLine(person_get.ToString());
            }

            _testOutputHelper.WriteLine("Expected");
            //foreach (var person_add in persons_from_add)
            //{
            //    _testOutputHelper.WriteLine(person_add.ToString());
            //    Assert.Contains(person_add, persons_from_get);
            //}


            persons_from_get.Should().BeEquivalentTo(persons.Select(p => p.ToPersonResponse()));

        }

        #endregion



        #region Get Persons by Filter
        //add few persons; if search with empty filter search and filterby PersonName it will return all list PersonResponse
        [Fact]
        public async Task GetpersonsByFilter_EmptyFilterSearch()
        {
            //Arrange

            Person person1 = _fixture.Build<Person>()
                .With(temp => temp.PersonName, "Bambanag")
                .With(temp => temp.country, null as Country)
                .With(temp => temp.Email, "test@mail.com")
                .Create();


            Person person2 = _fixture.Build<Person>()
                .With(temp => temp.PersonName, "gutaw")
                .With(temp => temp.country, null as Country)
                .With(temp => temp.Email, "test@mail.com")
                .Create();


            Person person3 = _fixture.Build<Person>()
                .With(temp => temp.PersonName, "Hana")
                .With(temp => temp.country, null as Country)
                .With(temp => temp.Email, "hana@mail.com")
                .Create();




            List<Person> persons = new List<Person>() {
                person1,
                person2,
                person3
            };



            string filterBy = nameof(PersonResponse.PersonName);
            _testOutputHelper.WriteLine($"filterBy {filterBy}");

            _personRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);

            //Act
            var persons_get_filter = await _personService.GetPersonsByFilter(filterBy, String.Empty);

            //Assert
            //foreach (var person_add in persons_from_add)
            //{
            //    Assert.Contains(person_add, persons_get_filter);
            //}

            persons_get_filter.Should().BeEquivalentTo(persons.Select(p => p.ToPersonResponse()));

        }

        [Fact]
        public async Task GetpersonsByFilter_ByPersonName()
        {
            //Arrange
            //CountryResponse countryOne = await _countryService.AddCountry(_fixture.Create<CountryAddRequest>());
            //CountryResponse countryTwo = await _countryService.AddCountry(_fixture.Create<CountryAddRequest>());



            Person person1 = _fixture.Build<Person>()
                .With(temp => temp.Email, "test@mail.com")
                .With(temp => temp.country, null as Country)
                .Create();


            Person person2 = _fixture.Build<Person>()
                .With(temp => temp.Email, "test@mail.com")
                .With(temp => temp.country, null as Country)
                .Create();


            Person person3 = _fixture.Build<Person>()
                .With(temp => temp.Email, "hana@mail.com")
                .With(temp => temp.country, null as Country)
                .Create();





            List<Person> persons = new List<Person>() {
                person1,
                person2,
                person3
            };



            string filterBy = nameof(PersonResponse.PersonName);
            _testOutputHelper.WriteLine($"filterBy {filterBy}");

            _personRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);

            //Act
            var persons_get_filter = await _personService.GetPersonsByFilter(filterBy, "na");

            //Assert
            //foreach (var person_add in persons_from_add)
            //{
            //    if (person_add.PersonName != null)
            //    {
            //        if (person_add.PersonName.Contains("na", StringComparison.OrdinalIgnoreCase))
            //        {
            //            Assert.Contains(person_add, persons_get_filter);
            //        }
            //    }

            //}

            //persons_get_filter.Should().OnlyContain(p => p.PersonName != null && p.PersonName.Contains("na"));
            persons_get_filter.Should().BeEquivalentTo(persons.Select(x => x.ToPersonResponse()));

        }

        #endregion


        #region Get Sorted Person
        //invalid sortby or invalid sortorder, it should return same personsToOrder list
        [Fact]
        public void GetSortedPerson_InvalidSortby()
        {
            //Arrange




            PersonResponse person1 = _fixture.Build<PersonResponse>()
                .With(temp => temp.PersonName, "Bambanag")
                .With(temp => temp.Email, "test@mail.com")
                .Create();
            PersonResponse person2 = _fixture.Build<PersonResponse>()
                .With(temp => temp.PersonName, "Haji")
                .With(temp => temp.Email, "test@mail.com")
                .Create();
            PersonResponse person3 = _fixture.Build<PersonResponse>()
                .With(temp => temp.PersonName, "Rosa")
                .With(temp => temp.Email, "test@mail.com")
                .Create();


            List<PersonResponse> personsToOrder = new List<PersonResponse>() {person1, person2, person3 };
            string sortBy = "invalid_prop";
            SortOrderOptions sortOrder = SortOrderOptions.ASC;

            //Act
            var persons_sorted = _personService.GetSortedPersons(personsToOrder, sortBy, sortOrder);

            //Assert
            //for (int i = 0; i < persons_sorted.Count; i++)
            //{
            //    Assert.Equal(personsToOrder[i], persons_sorted[i]);
            //}

            persons_sorted.Should().BeEquivalentTo(personsToOrder, option => option.WithStrictOrdering());

        }
        //sortby person name and sortOrder by desc it should return descending ordered of property personName of personsToOrder   
        [Fact]
        public void GetSortedPerson_SortByNameDesc()
        {
            //Arrange
   



            PersonResponse person1 = _fixture.Build<PersonResponse>()
                .With(temp => temp.PersonName, "Bambanag")
                .With(temp => temp.Email, "test@mail.com")
                .Create();


            PersonResponse person2 = _fixture.Build<PersonResponse>()
                .With(temp => temp.PersonName, "gutaw")
                .With(temp => temp.Email, "test@mail.com")
                .Create();


            PersonResponse person3 = _fixture.Build<PersonResponse>()
                .With(temp => temp.PersonName, "Hana")
                .With(temp => temp.Email, "hana@mail.com")
                .Create();





            List<PersonResponse> persons = new List<PersonResponse>() { person1, person2, person3};
            var persons_sorted_expected = persons.OrderByDescending((p) => p.PersonName).ToList();

            string sortBy = nameof(PersonResponse.PersonName);
            SortOrderOptions sortOrder = SortOrderOptions.DESC;

            //Act
            var persons_sorted_actual = _personService.GetSortedPersons(persons, sortBy, sortOrder);

            //Assert
            //for (int i = 0; i < persons_sorted_actual.Count; i++)
            //{
            //    Assert.Equal(persons_sorted_expected[i], persons_sorted_actual[i]);
            //}

            persons_sorted_actual.Should().BeInDescendingOrder(x => x.PersonName, StringComparer.OrdinalIgnoreCase);

        }
        #endregion

        #region Update Person
        //if PersonUpdateRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_PersonUpdateRequestNull()
        {
            //Arrange
            PersonUpdateRequest? person_update = null;


            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //Act
            //    var persons_sorted_actual = await _personService.UpdatePerson(person_update);
            //});

            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(person_update);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();

        }
        //if PersonId is not valid or not found, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonIdNotFound()
        {
            //Arrange
            PersonUpdateRequest? person_update = _fixture.Build<PersonUpdateRequest>()
                .With(temp => temp.Email, "test@mail.com")
                .Create();




            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    var updated_person = await _personService.UpdatePerson(person_update);
            //});

            _personRepositoryMock.Setup(p => p.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(null as Person);

            //Act
            Func<Task> action = async () =>
            {
                var updated_person = await _personService.UpdatePerson(person_update);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }
        //if PersonName is empty, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameEmpty()
        {
            //Arrange


            PersonUpdateRequest? person_update = _fixture.Build<PersonUpdateRequest>()
                .With(temp => temp.PersonName, null as string)
                .With(temp => temp.Email, "test@mail.com")
                .Create();




            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    var updated_person = await _personService.UpdatePerson(person_update);
            //});

            //Act
            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(person_update);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }
        //supplied propper PersonUpdateRequest, it should return updated PersonResponse
        [Fact]
        public async Task UpdatePerson_updateValidPersonId()
        {
            //Arrange


            Person person = _fixture.Build<Person>()
                .With(temp => temp.PersonName, "Hana")
                .With(temp => temp.Email, "hana@mail.com")
                .With(temp => temp.country, null as Country)
                .Create();


            PersonUpdateRequest? person_update = _fixture.Build<PersonUpdateRequest>()
                .With(temp => temp.Email, "test@mail.com")
                .Create();

            //mock method person repository UpdatePerson dan GetPersonByPersonId
            _personRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);
            _personRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);


            //Act
            var updated_person_actual = await _personService.UpdatePerson(person_update);


            //Assert
            updated_person_actual.Should().BeEquivalentTo(person.ToPersonResponse());

        }
        #endregion

        #region Delete Person
        //if PersonId is null it should return false
        [Fact]
        public async Task DeletePerson_PersonIdNull()
        {
            //Arrange
            Guid? personId = null;

            //Act
            bool isDeleted = await _personService.DeletePerson(personId);

            //Assert
            Assert.False(isDeleted);
        }
        //if PersonId is not valid it should return false
        [Fact]
        public async Task DeletePerson_PersonIdNotValid()
        {
            //Arrange
            Guid? personId = Guid.NewGuid();

            _personRepositoryMock.Setup(p => p.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(null as Person);

            //Act
            bool isDeleted = await _personService.DeletePerson(personId);

            //Assert
            Assert.False(isDeleted);
        }
        //if supplied valid PersonId, it should return true
        [Fact]
        public async Task DeletePerson_PersonIdValid()
        {
            //Arrange




            Person person1 = _fixture.Build<Person>()
                .With(temp => temp.PersonName, "Bambanag")
                .With(temp => temp.country, null as Country)
                .Create();


            Guid personIdToDelete = person1.PersonId;

            _personRepositoryMock.Setup(p => p.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person1);
            _personRepositoryMock.Setup(p => p.DeletePerson(It.IsAny<Guid>())).ReturnsAsync(true);

            //Act
            bool isDeleted = await _personService.DeletePerson(personIdToDelete);

            //Assert
            //Assert.DoesNotContain(person_response_from_add_1, getAllPersons);
            //Assert.True(isDeleted);

            //getAllPersons.Should().NotContain(person_response_from_add_1);
            isDeleted.Should().BeTrue();



        }
        #endregion
    }

}
