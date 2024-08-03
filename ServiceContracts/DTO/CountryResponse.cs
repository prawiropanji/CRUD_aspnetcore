using Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid Id { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            var countryResponse = (CountryResponse)obj;
            if (this.Id != countryResponse.Id || this.CountryName != countryResponse.CountryName)
            {

                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public static class CountryExtension
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            CountryResponse response = new CountryResponse()
            {
                Id = country.Id,
                CountryName = country.CountryName,
            };
            return response;
        }
    }
 

}
