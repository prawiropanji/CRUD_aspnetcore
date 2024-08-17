using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly ApplicationDbContext _db;

        public CountriesRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Country> AddCountry(Country country)
        {
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _db.Countries.Include(c => c.Persons).ToListAsync();
        }

        public async Task<Country?> GetCountryByCountryId(Guid id)
        {
           return await _db.Countries.Include(c => c.Persons).FirstOrDefaultAsync(c => c.CountryId == id);
        }

        public async Task<Country?> GetCountryByCountryName(string countryName)
        {
            return await _db.Countries.Include(c => c.Persons).FirstOrDefaultAsync(c => c.CountryName != null && c.CountryName.Equals(countryName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
