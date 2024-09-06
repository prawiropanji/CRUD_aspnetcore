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
    public class PersonsService : IPersonsService
    {
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsService> _logger;
        private readonly IDiagnosticContext _diagnostic;

        public PersonsService(IPersonsRepository personsRepository, ILogger<PersonsService> logger, IDiagnosticContext diagnostic)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnostic = diagnostic;
        }

        public PersonResponse ConverToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            return personResponse;
        }
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }


            //model validation
            ValidationHelper.ValidateObject(personAddRequest);


            Person person = personAddRequest.ToPerson();
            person.PersonId = Guid.NewGuid();
            await _personsRepository.AddPerson(person);


            PersonResponse personResponse = person.ToPersonResponse();
            return personResponse;


        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId is null)
            {
                return false;
            }


            Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);
            if (person is null)
            {
                return false;
            }

            await _personsRepository.DeletePerson(personId.Value);

            return true;

        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            //return _db.Sp_GetAllPersons().Select(p => ConverToPersonResponse(p)).ToList();
            List<Person> persons = await _personsRepository.GetAllPersons();
            return persons.Select((p) => ConverToPersonResponse(p)).ToList();

        }

        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);
            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();

        }

        public async Task<List<PersonResponse>> GetPersonsByFilter(string? filterBy, string? filterSearch)
        {
            _logger.LogInformation("Execute GetPersonsByFilter in PersonsService");


            List<Person> listPerson;
            using (Operation.Time("Timming taken for Get Filtered Persons from database"))
            {
                listPerson = filterBy switch
                {
                    nameof(PersonResponse.PersonName) =>
                       await _personsRepository.GetFilteredPersons(p => p.PersonName != null && p.PersonName.Contains(filterSearch)),
                    nameof(PersonResponse.Email) =>
                       await _personsRepository.GetFilteredPersons(p => p.Email != null && p.Email.Contains(filterSearch)),
                    nameof(PersonResponse.DateOfBirth) =>
                        await _personsRepository.GetFilteredPersons(p => p.DateOfBirth != null && p.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(filterSearch)),
                    nameof(PersonResponse.Gender) =>
                        await _personsRepository.GetFilteredPersons(p => p.Gender != null && p.Gender.Contains(filterSearch)),
                    nameof(PersonResponse.CountryName) =>
                        await _personsRepository.GetFilteredPersons(p => p.country!.CountryName != null && p.country.CountryName.Contains(filterSearch)),
                    _ => await _personsRepository.GetAllPersons()

                };
            }//end of timming


            var persons = listPerson.Select(p => p.ToPersonResponse()).ToList();
            _diagnostic.Set("Persons", persons);
            return persons;
            
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

        public async Task<MemoryStream> GetPersonsCSV()
        {
            List<PersonResponse> persons = await GetAllPersons();
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memoryStream);
            CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);


            //write header
            csv.WriteField(nameof(PersonResponse.PersonId));
            csv.WriteField(nameof(PersonResponse.PersonName));
            csv.WriteField(nameof(PersonResponse.Email));
            csv.WriteField(nameof(PersonResponse.DateOfBirth));
            csv.WriteField(nameof(PersonResponse.Gender));
            csv.WriteField(nameof(PersonResponse.CountryName));
            csv.WriteField(nameof(PersonResponse.Address));
            csv.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
            csv.WriteField(nameof(PersonResponse.Age));
            await csv.NextRecordAsync();


            foreach (var person in persons)
            {
                csv.WriteField(person.PersonId);
                csv.WriteField(person.PersonName);
                csv.WriteField(person.Email);
                csv.WriteField(person.DateOfBirth.GetValueOrDefault().ToString("dd-MM-yyyy"));
                csv.WriteField(person.Gender);
                csv.WriteField(person.CountryName);
                csv.WriteField(person.Address);
                csv.WriteField(person.ReceiveNewsLetters);
                csv.WriteField(person.Age);
                await csv.NextRecordAsync();
            }


            memoryStream.Position = 0;



            return memoryStream;

        }

        public async Task<MemoryStream> GetPersonsXlsx()
        {
            var persons = await GetAllPersons();
            MemoryStream memoryStream = new MemoryStream();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Persons");


                //set Header
                var listProperyInfo = typeof(PersonResponse).GetProperties();
                for (int i = 0; i < listProperyInfo.Length; i++)
                {
                    var propertyName = listProperyInfo[i].Name;
                    worksheet.Cells[1, (i + 1)].Value = propertyName;
                }

                //style header
                var range = worksheet.Cells[1, 1, 1, listProperyInfo.Length];
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                //set data
                int row = 2;
                foreach (var person in persons)
                {
                    worksheet.Cells[row, 1].Value = person.PersonId;
                    worksheet.Cells[row, 2].Value = person.PersonName;
                    worksheet.Cells[row, 3].Value = person.Email;
                    worksheet.Cells[row, 4].Value = person.DateOfBirth.GetValueOrDefault().ToString("dd-MM-yyyy");
                    worksheet.Cells[row, 5].Value = person.Gender;
                    worksheet.Cells[row, 6].Value = person.CountryId;
                    worksheet.Cells[row, 7].Value = person.CountryName;
                    worksheet.Cells[row, 8].Value = person.Address;
                    worksheet.Cells[row, 9].Value = person.ReceiveNewsLetters;
                    worksheet.Cells[row, 10].Value = person.Age;
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                worksheet.Cells[1, 1, row - 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                await package.SaveAsAsync(memoryStream);



            }

            memoryStream.Position = 0;

            return memoryStream;

        }
    }
}
