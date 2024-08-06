using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace CRUDTest
{
    public class CountryTest
    {
        private readonly CountryService _countryService;
        public CountryTest()
        {
            _countryService = new CountryService(new Entities.PersonDbContext(new DbContextOptionsBuilder().Options));
        }
        #region Add Country
        //When CountryAddRequest is null, it should throw exception ArgumentNullException
        [Fact]
        public void CountryAdd_WhenNull()
        {
            //Arrange
            CountryAddRequest? request = null;


            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countryService.AddCountry(request);
            });
        }

        //When CountryName is null, it should throw ArgumentException
        [Fact]
        public void CountryAdd_WhenCountryNameNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest();


            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.AddCountry(request);
            });
        }

        //when CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public void CountryAdd_CountryNameDuplicate()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };


            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.AddCountry(request);
                _countryService.AddCountry(request2);
            });
        }

        //When CountryAddRequest is propper, it should add to list of Countries
        [Fact]
        public void CountryAdd_PropperCountryAddRequest()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Indonesia" };


            //Act
            CountryResponse countryResponse = _countryService.AddCountry(request);


            //Assert
            Assert.Contains(countryResponse, _countryService.GetListCountries());
            Assert.True(countryResponse.CountryId != Guid.Empty);
        }

        #endregion


        #region Get List Countries
        [Fact]
        public void GetCountry_EmptyList()
        {

            //Acct Assert

            Assert.Empty(_countryService.GetListCountries());

        }


        [Fact]
        public void GetCountry_AddViewCountries()
        {
            //Arrange
            List<CountryAddRequest> requests = new List<CountryAddRequest>() {
                new (){CountryName = "USA"},
                new (){CountryName = "Indonesia"},
            };

            foreach (var request in requests)
            {
                var response = _countryService.AddCountry(request);
                Assert.Contains<CountryResponse>(response, _countryService.GetListCountries());
            }

        }

        #endregion


        #region Get Country by id
        //When country id is null it should throw ArgumentNullException
        [Fact]
        public void GetCountry_NullId()
        {
            //Arange 
            Guid? id = null;


            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                CountryResponse? response = _countryService.GetCountryById(id);
            });

        }
        //when there is no country found, it should return null
        [Fact]
        public void GetCountry_IdNotFound()
        {
            //Arange 
            Guid id = Guid.NewGuid();


            //Act
            CountryResponse? response = _countryService.GetCountryById(id);

            //Assert
            Assert.Null(response);
        }
        //when a propper id is given, it should return CountryResponse with same id
        [Fact]
        public void GetCountry_AddCountry()
        {
            //Arange 
            CountryResponse newAdded =  _countryService.AddCountry(new CountryAddRequest() { CountryName = "Australia" });
            Guid id = newAdded.CountryId;

            //Act
            CountryResponse? response = _countryService.GetCountryById(id);

            //Assert
            Assert.Equal(newAdded, response);

        }
        #endregion
    }
}
