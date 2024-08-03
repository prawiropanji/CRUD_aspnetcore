using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class CountryService : ICountriesService
    {

        private readonly List<Country> _countries;

        public CountryService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse AddCountry(CountryAddRequest? newCountry)
        {
            if(newCountry == null)
            {
                throw new ArgumentNullException(nameof(newCountry));
            }

            ValidationHelper.ValidateObject(newCountry);
            

            if(_countries.Any(country => country.CountryName == newCountry.CountryName))
            {
                throw new ArgumentException("given country name already exist in list countries", nameof(newCountry.CountryName));
            }

            Country country = newCountry.ToCountry();
            country.Id = Guid.NewGuid();
            _countries.Add(country);

            CountryResponse countryResponse = country.ToCountryResponse();

            return countryResponse;
        }

        public CountryResponse? GetCountryById(Guid? countryId)
        {
            if (countryId == null) { 
                throw new ArgumentNullException(nameof(countryId));
            }


            Country? country = _countries.FirstOrDefault(c => c.Id == countryId);

            if (country is null)
            {
                return null;
            }

            return country.ToCountryResponse();

        }

        public List<CountryResponse> GetListCountries()
        {
            List<CountryResponse> countryResponses = this._countries.Select(country => country.ToCountryResponse()).ToList();
            return countryResponses;
        }

     
    }
}
