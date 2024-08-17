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
        /// <summary>
        /// Add new countries by upload file excel
        /// </summary>
        /// <param name="memoryStream">memory stream with excel content</param>
        /// <returns>numbers of added row</returns>
        Task<Response<int>> AddCountryExcelFile(MemoryStream memoryStream);

    }
}
