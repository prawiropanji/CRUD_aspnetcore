using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    /// <summary>
    /// Data access layer logic for managing country entity
    /// </summary>
    public interface IPersonsRepository
    {
        /// <summary>
        /// Add new person into data source, return the added person
        /// </summary>
        /// <param name="person">Person object to insert</param>
        /// <returns>Person object successfully added</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Get a person by its person id
        /// </summary>
        /// <param name="personId">Person Id to search</param>
        /// <returns>Found Person object</returns>
        Task<Person?> GetPersonByPersonId(Guid personId);

        /// <summary>
        /// Get list of person by given expression
        /// </summary>
        /// <param name="predicate">Linq expression to check</param>
        /// <returns>All mathcing persons</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person,bool>> predicate);

        /// <summary>
        /// Return all person from data source
        /// </summary>
        /// <returns>All list Person object</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Delete person from data source by its person id
        /// </summary>
        /// <param name="personId">person id to search person to be deleted</param>
        /// <returns>boolean indicating success or not of deletion operation</returns>
        Task<bool> DeletePerson(Guid personId);

        /// <summary>
        /// update person data based on the person id
        /// </summary>
        /// <param name="person">person object to update</param>
        /// <returns>updated person object</returns>
        Task<Person> UpdatePerson(Person person);   
    }
}
