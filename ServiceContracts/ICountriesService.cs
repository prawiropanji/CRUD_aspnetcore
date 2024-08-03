using ServiceContracts.DTO;
namespace ServiceContracts
{
    public interface ICountriesService
    {
         CountryResponse AddCountry(CountryAddRequest newCountry);
      

         List<CountryResponse> GetListCountries();

        CountryResponse? GetCountryById(Guid? countryId);

    }
}
