using ServiceContracts.DTO;
namespace ServiceContracts
{
    public interface ICountriesService
    {
         CountryResponse AddCountry(CountryAddRequest newCountry);
      
        /// <summary>
        /// Get All Available Country
        /// </summary>
        /// <returns>Return List of CountryResponse</returns>
         List<CountryResponse> GetListCountries();

        CountryResponse? GetCountryById(Guid? countryId);

    }
}
