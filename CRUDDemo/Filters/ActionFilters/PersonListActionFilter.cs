using CRUDDemo.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyModel;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDDemo.Filters.ActionFilters
{
    public class PersonListActionFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public PersonListActionFilter(ILogger<PersonListActionFilter> logger)
        {
            this._logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("exectuing {FilterName}.{MethodName}", nameof(PersonListActionFilter), nameof(OnActionExecuted));

            //access ViewData
            var controller = (PersonsController)context.Controller;

            //access action method arguments via HttpContext.Items
            IDictionary<string, Object?>? arguments = (IDictionary<string, Object?>?)context.HttpContext.Items["arguments"];

            

            //set ViewData
            controller.ViewBag.FilterByOptions = new Dictionary<string, string>() {
                {nameof(PersonResponse.PersonName), "Person Name" },
                {nameof(PersonResponse.Email), "Email" },
                {nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                {nameof(PersonResponse.Gender), "Gender" },
                {nameof(PersonResponse.CountryName), "Country Name" }
            };

            if (arguments != null)
            {
                if (arguments.ContainsKey("filterBy"))
                {
                    controller.ViewBag.CurrentFilterBy = (string?)arguments["filterBy"];
                }

                if (arguments.ContainsKey("filterSearch"))
                {
                    controller.ViewBag.CurrentFilterSearch = (string?)arguments["filterSearch"];
                }       
                
                if (arguments.ContainsKey("sortBy"))
                {
                    controller.ViewBag.CurrentSortBy = (string?)arguments["sortBy"];
                }       
                
                if (arguments.ContainsKey("sortOrder"))
                {
                    controller.ViewBag.CurrentSortOrder = (SortOrderOptions?)arguments["sortOrder"];
                }
            }

            

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("exectuing {FilterName}.{MethodName}", nameof(PersonListActionFilter), nameof(OnActionExecuting));

            context.HttpContext.Items["arguments"] = context.ActionArguments;

            if (context.ActionArguments.ContainsKey("filterBy"))
            {
                List<string> listFilterBy = new List<string>() {
                    nameof(PersonResponse.PersonName),
                    nameof(PersonResponse.Email),
                    nameof(PersonResponse.DateOfBirth),
                    nameof(PersonResponse.Gender),
                    nameof(PersonResponse.CountryName),
                };

                var filterBy = context.ActionArguments["filterBy"]?.ToString();

                if (!listFilterBy.Any(f => f == filterBy))
                {
                    context.ActionArguments["filterBy"] = nameof(PersonResponse.PersonName);
                }

            }
        }
    }
}
