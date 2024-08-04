using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class CountryService : ICountriesService
    {

        private readonly List<Country> _countries;

        public CountryService(bool initialize = true)
        {
            _countries = new List<Country>();

            if (initialize)
            {
                _countries = new List<Country> {
                    new Country()
                    {
                        Id = Guid.Parse("5C1C7726-80D9-4083-8950-1DB8C5C7E49E"),
                        CountryName = "Argentina"
                    },

                    new Country()
                    {
                        Id = Guid.Parse("E7E71C8F-7E20-4481-A956-A953203216CA"),
                        CountryName = "Japan" 
                    },
                    new Country()
                    {
                        Id = Guid.Parse("D9E788C5-37A0-4973-814E-FE5C05AE92B2"),
                        CountryName = "Indonesia" 
                    },
                    new Country()
                    {
                        Id = Guid.Parse("B3EB75FE-9783-46F4-89DE-865CAFAAEDE1"),
                        CountryName = "Australia" 
                    }


                };
            }

        }

        public CountryResponse AddCountry(CountryAddRequest? newCountry)
        {
            if (newCountry == null)
            {
                throw new ArgumentNullException(nameof(newCountry));
            }

            ValidationHelper.ValidateObject(newCountry);


            if (_countries.Any(country => country.CountryName == newCountry.CountryName))
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
            if (countryId == null)
            {
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
