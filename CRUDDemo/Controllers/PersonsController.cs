﻿using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using System.Runtime.CompilerServices;
using ServiceContracts.DTO;
using Entities;
using ServiceContracts.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

namespace CRUDDemo.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }
        [HttpGet]
        [Route("[action]")] //match route "/persons/index" 
        [Route("")]      //match route "/persons"
        [Route("/")]     //match route "/"
        public async Task<IActionResult> Index(string? filterBy, string? filterSearch, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC )
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

            List<PersonResponse> persons = await _personsService.GetPersonsByFilter(filterBy, filterSearch);

            


            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);

            return View(sortedPersons);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> PersonsPdf()
        {
            IEnumerable<PersonResponse> persons = await _personsService.GetAllPersons();
            return new ViewAsPdf("PersonsPDF", persons, ViewData) { 
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
            };
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countryResponse = await _countriesService.GetListCountries();
            ViewBag.ListCountry = countryResponse.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryId.ToString() });
            //ViewBag.ListCountry = _countriesService.GetListCountries();
            return View();
        }



        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create([FromForm] PersonAddRequest person)
        {
            if (!ModelState.IsValid)
            {
                List<string> listError = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.ListAddPersonError = listError;
                ViewBag.ListCountry = await _countriesService.GetListCountries();

                return View(person);
            }

            await _personsService.AddPerson(person);

            return RedirectToAction("Index", "Persons");
        }


        [HttpGet]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Edit(Guid personId)
        {
            List<CountryResponse> countryResponses = await _countriesService.GetListCountries();
            ViewBag.ListCountry = countryResponses.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryId.ToString() });
            PersonResponse? person = await _personsService.GetPersonByPersonId(personId);
            PersonUpdateRequest? updatePerson = person?.ToPersonUpdateRequest();
            return View(updatePerson);
        }



        [HttpPost]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Edit(PersonUpdateRequest person)
        {

            if (!ModelState.IsValid)
            {
                List<string> listError = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.ListAddPersonError = listError;
                List<CountryResponse> countryResponse = await _countriesService.GetListCountries();
                ViewBag.ListCountry = countryResponse.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryId.ToString() });

                return View(person);
            }

            await _personsService.UpdatePerson(person);

            return RedirectToAction("Index","Persons");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Delete(Guid personId) {

            PersonResponse? person = await _personsService.GetPersonByPersonId(personId);
            if(person == null)
            {
                return RedirectToAction("index", "persons");
            }

            return View(person);
        }


        [HttpPost]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest person) {
            bool isDeleted = await _personsService.DeletePerson(person.PersonId);
            if (!isDeleted) {
                return BadRequest("deleted person was failed!");
            }

            return RedirectToAction("index", "persons");
        }



    }
}
