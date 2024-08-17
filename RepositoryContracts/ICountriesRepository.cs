using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Data Access Layer for interact with Country data source
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// Take Country object as parameter and add it to Countries source
        /// </summary>
        /// <param name="country">new Country object that want to be added into sources</param>
        /// <returns>Added Country object</returns>
        Task<Country> AddCountry(Country country);
        
        /// <summary>
        /// Return Country object by its countryName, otherwise null
        /// </summary>
        /// <param name="countryName">country name to search</param>
        /// <returns>Found Country object, otherwise null</returns>
        Task<Country?> GetCountryByCountryName(string countryName);

        /// <summary>
        /// Return country object by its countryId, otherwise null
        /// </summary>
        /// <param name="id">country id to search</param>
        /// <returns>Found Country object, otherwise null</returns>
        Task<Country?> GetCountryByCountryId(Guid id);

        /// <summary>
        /// Returns all list Country object from data source
        /// </summary>
        /// <returns>List Country object</returns>
        Task<List<Country>> GetAllCountries();

    }
}
