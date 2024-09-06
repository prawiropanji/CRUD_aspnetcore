using CRUDDemo.Controllers;
using Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using System;

namespace CRUDDemo.Filters.ActionFilters
{
    public class PersonValidationActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;

        public PersonValidationActionFilter(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //before
            PersonsController? personController = context.Controller as PersonsController;
            if (personController != null)
            {
                if (!personController.ModelState.IsValid)
                {
                    List<string> listError = personController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    personController.ViewBag.ListAddPersonError = listError;
                    //personController.ViewBag.ListCountry = await _countriesService.GetListCountries();
                    List<CountryResponse> countryResponse = await _countriesService.GetListCountries();
                    personController.ViewBag.ListCountry = countryResponse.Select(c => new SelectListItem() { Text = c.CountryName, Value = c.CountryId.ToString() });

                    var person = context.ActionArguments["person"] as PersonAddRequest;
                    context.Result = personController.View(person);
                    return;
                }

           

            }


            await next();



        }
    }
}
