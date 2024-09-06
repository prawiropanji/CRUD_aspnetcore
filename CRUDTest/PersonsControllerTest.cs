using AutoFixture;
using Castle.Core.Logging;
using CRUDDemo.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTest
{
    public class PersonsControllerTest
    {
        private readonly Mock<IPersonsService> _personsServiceMock;
        private readonly Mock<ICountriesService> _countiesServiceMock;
        private readonly Mock<ILogger<PersonsController>> _loggerServiceMock;
        private readonly PersonsController _personsController;
        private readonly IFixture _fixture;
        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _personsServiceMock = new Mock<IPersonsService>();
            _countiesServiceMock = new Mock<ICountriesService>();
            _loggerServiceMock = new Mock<ILogger<PersonsController>>();

            IPersonsService _personsService = _personsServiceMock.Object;
            ICountriesService _countriesService = _countiesServiceMock.Object;
            ILogger<PersonsController> loggerService = _loggerServiceMock.Object;

            _personsController = new PersonsController(_personsService, _countriesService, loggerService);
        }


        [Fact]
        public async Task Index_ShouldreturnViewWithModelData()
        {
            //Arrange
            string filterBy = _fixture.Create<string>();
            string filterSearch = _fixture.Create<string>();
            string sortBy = _fixture.Create<string>();
            SortOrderOptions sortOrder = _fixture.Create<SortOrderOptions>();
            List<PersonResponse> personResponses = _fixture.Create<List<PersonResponse>>();

            //mock service method
            _personsServiceMock.Setup(temp => temp.GetPersonsByFilter(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(personResponses);
            _personsServiceMock.Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>())).Returns(personResponses);

            //Act
            IActionResult actionResult = await _personsController.Index(filterBy, filterSearch, sortBy, sortOrder);

            //Assert
            actionResult.Should().BeOfType<ViewResult>();
            if(actionResult is ViewResult && actionResult is not null )
            {
                ViewResult? view = actionResult as ViewResult; 
                view!.ViewData.Model.Should().BeOfType<List<PersonResponse>>();
            }
            

        }

        [Fact]
        public async Task Create_InvalidValidationShouldRetrunView()
        {
            //Arrange
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();
            PersonResponse personResponse = _fixture.Create<PersonResponse>();
            List<CountryResponse> list_country = _fixture.Create<List<CountryResponse>>();

            //mock service method
            _countiesServiceMock.Setup(temp => temp.GetListCountries()).ReturnsAsync(list_country);
            _personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);

            //simulate invalid modelstate
            _personsController.ModelState.AddModelError("Person Name", "Person Name is Required");
            //Act
            IActionResult actionResult = await _personsController.Create(personAddRequest);

            //Assert
            actionResult.Should().BeOfType<ViewResult>();
            if (actionResult is ViewResult && actionResult is not null)
            {
                ViewResult? view = actionResult as ViewResult;
                view!.ViewData.Model.Should().BeOfType<PersonAddRequest>();
                view!.ViewData.Model.Should().BeEquivalentTo(personAddRequest);
            }


        }


        [Fact]
        public async Task Create_ValidShouldRedirectToPersonsIndex()
        {
            //Arrange
            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();
            PersonResponse personResponse = _fixture.Create<PersonResponse>();
            List<CountryResponse> list_country = _fixture.Create<List<CountryResponse>>();

            //mock service method
            _countiesServiceMock.Setup(temp => temp.GetListCountries()).ReturnsAsync(list_country);
            _personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>())).ReturnsAsync(personResponse);

            //Act
            IActionResult actionResult = await _personsController.Create(personAddRequest);

            //Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
            if (actionResult is ViewResult && actionResult is not null)
            {
                RedirectToActionResult? redirectResult = actionResult as RedirectToActionResult;
                redirectResult!.ActionName.Should().Be("Index");
                redirectResult!.ControllerName.Should().Be("Persons");
            }


        }

    }
}
