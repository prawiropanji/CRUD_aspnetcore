using CsvHelper;
using Entities;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonsSorterService : IPersonsSorterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnostic;

        public PersonsSorterService(IPersonsRepository personsRepository, ILogger<PersonsSorterService> logger, IDiagnosticContext diagnostic)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnostic = diagnostic;
        }


        public List<PersonResponse> GetSortedPersons(List<PersonResponse> personsToOrder, string sortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("Execute GetSortedPersons in PersonsService");

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

      
    }
}
