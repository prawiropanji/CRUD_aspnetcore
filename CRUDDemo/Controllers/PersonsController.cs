using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using System.Runtime.CompilerServices;
using ServiceContracts.DTO;
using Entities;
using ServiceContracts.Enums;

namespace CRUDDemo.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;

        public PersonsController(IPersonsService personsService)
        {
            _personsService = personsService;
        }

        [Route("persons/index")]
        [Route("persons")]
        [Route("/")]
        public IActionResult Index(string? filterBy, string? filterSearch, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC )
        {

            ViewBag.FilterByOptions = new Dictionary<string, string>() {
                {nameof(PersonResponse.PersonName), "Person Name" },
                {nameof(PersonResponse.Email), "Email" },
                {nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                {nameof(PersonResponse.Gender), "Gender" },
                {nameof(PersonResponse.CountryName), "Country Name" }
            };

            ViewBag.CurrentFilterBy = filterBy;
            ViewBag.CurrentFilterSearch = filterSearch;
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            List<PersonResponse> persons = _personsService.GetPersonsByFilter(filterBy, filterSearch);

            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);

            return View(sortedPersons);
        }

        [HttpGet]
        [Route("persons/create")]
        public IActionResult GetCreate()
        {
            return View();
        }



        [HttpPost]
        [Route("persons/create")]
        public IActionResult PostCreate()
        {
            return RedirectToAction("Index", "Persons");
        }

    }
}
