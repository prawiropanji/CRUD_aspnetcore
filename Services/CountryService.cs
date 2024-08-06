using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class CountryService : ICountriesService
    {

        private readonly PersonDbContext _db;

        public CountryService(PersonDbContext personDbContext)
        {
            _db = personDbContext;

           

        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? newCountry)
        {
            if (newCountry == null)
            {
                throw new ArgumentNullException(nameof(newCountry));
            }

            ValidationHelper.ValidateObject(newCountry);


            if (_db.Countries.Any(country => country.CountryName == newCountry.CountryName))
            {
                throw new ArgumentException("given country name already exist in list countries", nameof(newCountry.CountryName));
            }

            Country country = newCountry.ToCountry();
            country.CountryId = Guid.NewGuid();
            await _db.Countries.AddAsync(country);
            await _db.SaveChangesAsync();
          

            CountryResponse countryResponse = country.ToCountryResponse();

            return countryResponse;
        }

        public async Task<CountryResponse?> GetCountryById(Guid? countryId)
        {
            if (countryId == null)
            {
                throw new ArgumentNullException(nameof(countryId));
            }


            Country? country = await _db.Countries.FirstOrDefaultAsync(c => c.CountryId == countryId);

            if (country is null)
            {
                return null;
            }

            return country.ToCountryResponse();

        }

        public async Task<List<CountryResponse>> GetListCountries()
        {
            List<Country> countries = await this._db.Countries.Include(p => p.Persons).ToListAsync();
            List<CountryResponse> countryResponses = countries.Select(country => country.ToCountryResponse()).ToList();
            return countryResponses;
        }


    }
}
