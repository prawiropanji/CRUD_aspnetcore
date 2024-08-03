using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        List<PersonResponse> GetAllPersons();

        PersonResponse? GetPersonByPersonId(Guid? personId);

        List<PersonResponse> GetPersonsByFilter(string? filterBy, string? filterSearch);

        List<PersonResponse> GetSortedPersons(List<PersonResponse> personsToOrder, string sortBy, SortOrderOptions sortOrder );

        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        bool DeletePerson(Guid? personId);

    }
}
