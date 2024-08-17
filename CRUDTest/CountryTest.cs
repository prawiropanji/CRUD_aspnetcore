using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepositoryContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace CRUDTest
{
    public class CountryTest
    {
        private readonly CountryService _countryService;
        private readonly IFixture _fixture;
        private readonly Mock<ICountriesRepository> _mockCountriesRepository;

        public CountryTest()
        {
            List<Country> listCountry = new List<Country>();

            //create mock object for ICountryRepository
            _mockCountriesRepository = new Mock<ICountriesRepository>();

            //access mocked CountriesRepoistory object
            ICountriesRepository _countriesRepository = _mockCountriesRepository.Object;

            var dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            ApplicationDbContext applicationDbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, listCountry);

            _countryService = new CountryService(_countriesRepository);
            _fixture = new Fixture();
        }
        #region Add Country
        //When CountryAddRequest is null, it should throw exception ArgumentNullException
        [Fact]
        public async Task CountryAdd_WhenNull()
        {
            //Arrange
            CountryAddRequest? request = null;


            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //Act
            //    await _countryService.AddCountry(request);
            //});

            Func<Task> action = async () =>
            {
                await _countryService.AddCountry(request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task CountryAdd_WhenCountryNameNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest();


            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await _countryService.AddCountry(request);
            //});

            Func<Task> action = async () =>
            {
                await _countryService.AddCountry(request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        //when CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task CountryAdd_CountryNameDuplicate()
        {
            //Arrange

            //Assert
            //await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    //Act
            //    await _countryService.AddCountry(request);
            //    await _countryService.AddCountry(request2);
            //});

            CountryAddRequest countryAddRequest = _fixture.Build<CountryAddRequest>().Create();
            Country country = countryAddRequest.ToCountry();

            _mockCountriesRepository.Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>())).ReturnsAsync(country);



            Func<Task> action = async () =>
            {
                await _countryService.AddCountry(countryAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();

        }

        //When CountryAddRequest is propper, it should add to list of Countries
        [Fact]
        public async Task CountryAdd_PropperCountryAddRequest()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = _fixture.Build<CountryAddRequest>().Create();
            Country? expected_country = countryAddRequest.ToCountry();


            _mockCountriesRepository.Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>())).ReturnsAsync(null as Country);
            _mockCountriesRepository.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(expected_country);

            //Act
            CountryResponse countryResponse = await _countryService.AddCountry(countryAddRequest);
            expected_country.CountryId = countryResponse.CountryId;


            //Assert
            //Assert.Contains(countryResponse, await _countryService.GetListCountries());
            //Assert.True(countryResponse.CountryId != Guid.Empty);
            countryResponse.Should().BeEquivalentTo(expected_country.ToCountryResponse());

            countryResponse.CountryId.Should().NotBe(Guid.Empty);

        }

        #endregion


        #region Get List Countries
        [Fact]
        public async Task GetCountry_EmptyList()
        {

            //Acct Assert

            //Assert.Empty(await _countryService.GetListCountries());

            _mockCountriesRepository.Setup(temp => temp.GetAllCountries()).ReturnsAsync(new List<Country>());

            var listCountries = await _countryService.GetListCountries();
            listCountries.Should().BeEmpty();

        }


        [Fact]
        public async Task GetCountry_AddViewCountries()
        {
            //Arrange
            List<Country> listCountry = new List<Country>()
            {
                _fixture.Build<Country>().With(temp => temp.Persons, null as List<Person>).Create(),
                _fixture.Build<Country>().With(temp => temp.Persons, null as List<Person>).Create(),
                _fixture.Build<Country>().With(temp => temp.Persons, null as List<Person>).Create()
            };


            _mockCountriesRepository.Setup(temp => temp.GetAllCountries()).ReturnsAsync(listCountry);

            //Assert
            var listCountries = await _countryService.GetListCountries();

            listCountries.Should().BeEquivalentTo(listCountry.Select(c => c.ToCountryResponse()));






        }

        #endregion


        #region Get Country by id
        //When country id is null it should throw ArgumentNullException
        [Fact]
        public async Task GetCountry_NullId()
        {
            //Arange 
            Guid? id = null;


            //Assert
            //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{
            //    //Act
            //    CountryResponse? response = await _countryService.GetCountryById(id);
            //});

            Func<Task> action = async () =>
            {
                CountryResponse? response = await _countryService.GetCountryById(id);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();

        }
        //when there is no country found, it should return null
        [Fact]
        public async Task GetCountry_IdNotFound()
        {
            //Arange 
            Guid id = Guid.NewGuid();

            _mockCountriesRepository.Setup(temp => temp.GetCountryByCountryId(It.IsAny<Guid>())).ReturnsAsync(null as Country);

            //Act
            CountryResponse? response = await _countryService.GetCountryById(id);

            //Assert
            //Assert.Null(response);
            response.Should().BeNull();
        }
        //when a propper id is given, it should return CountryResponse with same id
        [Fact]
        public async Task GetCountry_AddCountry()
        {
            Country expected_country = _fixture.Build<Country>().With(c => c.Persons, null as List<Person>).Create();
            CountryResponse expected_countryResponse = expected_country.ToCountryResponse();
            //Arange 
            Guid id = expected_country.CountryId;

            _mockCountriesRepository.Setup(temp => temp.GetCountryByCountryId(It.IsAny<Guid>())).ReturnsAsync(expected_country);

            //Act
            CountryResponse? response = await _countryService.GetCountryById(id);

            //Assert
            //Assert.Equal(newAdded, response);

            response.Should().BeEquivalentTo(expected_countryResponse);

        }
        #endregion
    }
}
