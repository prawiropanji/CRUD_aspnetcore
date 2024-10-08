﻿using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsGetterService
    {

        Task<List<PersonResponse>> GetAllPersons();
        /// <summary>
        /// Find person by person id, it will return PersonResponse object if found. if not found it will return null
        /// </summary>
        /// <param name="personId">person id too looking for</param>
        /// <returns>PersondResponse object with personID you supplied</returns>
        Task<PersonResponse?> GetPersonByPersonId(Guid? personId);

        Task<List<PersonResponse>> GetPersonsByFilter(string? filterBy, string? filterSearch);
        Task<MemoryStream> GetPersonsCSV();
        Task<MemoryStream> GetPersonsXlsx();

    }
}
