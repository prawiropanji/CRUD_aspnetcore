using ServiceContracts.DTO;
namespace ServiceContracts
{
    public interface ICountriesService
    {
         Task<CountryResponse> AddCountry(CountryAddRequest newCountry);
      
        /// <summary>
        /// Get All Available Country
        /// </summary>
        /// <returns>Return List of CountryResponse</returns>
         Task<List<CountryResponse>> GetListCountries();

        Task<CountryResponse?> GetCountryById(Guid? countryId);

    }
}
