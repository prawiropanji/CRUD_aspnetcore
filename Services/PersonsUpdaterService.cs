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
    public class PersonsUpdaterService : IPersonsUpdaterService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnostic;

        public PersonsUpdaterService(IPersonsRepository personsRepository, ILogger<PersonsUpdaterService> logger, IDiagnosticContext diagnostic)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnostic = diagnostic;
        }

 

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            //validation
            ValidationHelper.ValidateObject(personUpdateRequest);


            Person? person_found = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonId);
            if (person_found is null)
            {
                throw new InvalidPersonIDException($"Person with PersonId {personUpdateRequest.PersonId} was not found");
            }

            person_found.PersonName = personUpdateRequest.PersonName;
            person_found.Email = personUpdateRequest.Email;
            person_found.DateOfBirth = personUpdateRequest.DateOfBirth;
            person_found.Gender = personUpdateRequest.Gender.ToString();
            person_found.CountryId = personUpdateRequest.CountryId;
            person_found.Address = personUpdateRequest.Address;
            person_found.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            await _personsRepository.UpdatePerson(person_found);


            return person_found.ToPersonResponse();


        }

    }
}
