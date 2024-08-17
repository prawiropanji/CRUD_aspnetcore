using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System.Collections.Specialized;

namespace Services
{
    public class CountryService : ICountriesService
    {

        private readonly ICountriesRepository _countriesRepository;

        public CountryService(ICountriesRepository countriesRepository)
        {
           _countriesRepository = countriesRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? newCountry)
        {
            if (newCountry == null)
            {
                throw new ArgumentNullException(nameof(newCountry));
            }

            ValidationHelper.ValidateObject(newCountry);


            if (await _countriesRepository.GetCountryByCountryName(newCountry.CountryName!) != null)
            {
                throw new ArgumentException("given country name already exist in list countries", nameof(newCountry.CountryName));
            }

            Country country = newCountry.ToCountry();
            country.CountryId = Guid.NewGuid();
            await _countriesRepository.AddCountry(country);


            CountryResponse countryResponse = country.ToCountryResponse();

            return countryResponse;
        }

        public async Task<Response<int>> AddCountryExcelFile(MemoryStream memoryStream)
        {
            int rowsAdded = 0;
            try
            {
                using (var package = new ExcelPackage(memoryStream))
                {
                    var worksheet = package.Workbook.Worksheets["Countries"];
                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    //Read rows
                    for (int i = 2; i <= rowCount; i++)
                    {
                        CountryAddRequest country = new CountryAddRequest();
                        for (int j = 1; j <= colCount; j++)
                        {

                            country.CountryName = worksheet.Cells[i, j].Text;
                            if (country.CountryName.IsNullOrEmpty())
                            {
                                continue;
                            }

                        }

                        await AddCountry(country);

                        rowsAdded++;
                    }

                    return new Response<int>() { Value = rowsAdded, Message = $"{rowsAdded} rows added successfully" };
                }
            }
            catch (Exception ex)
            {

                return new Response<int>() { Value = rowsAdded, Message = ex.Message };
            }



        }

        public async Task<CountryResponse?> GetCountryById(Guid? countryId)
        {
            if (countryId == null)
            {
                throw new ArgumentNullException(nameof(countryId));
            }


            Country? country = await _countriesRepository.GetCountryByCountryId(countryId.Value);

            if (country is null)
            {
                return null;
            }

            return country.ToCountryResponse();

        }

        public async Task<List<CountryResponse>> GetListCountries()
        {
            List<Country> countries = await _countriesRepository.GetAllCountries();
            List<CountryResponse> countryResponses = countries.Select(country => country.ToCountryResponse()).ToList();
            return countryResponses;
        }


    }
}
