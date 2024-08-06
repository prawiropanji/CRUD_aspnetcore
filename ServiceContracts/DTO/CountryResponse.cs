using Entities;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            var countryResponse = (CountryResponse)obj;
            if (this.CountryId != countryResponse.CountryId || this.CountryName != countryResponse.CountryName)
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
                CountryId = country.CountryId,
                CountryName = country.CountryName,
            };
            return response;
        }
    }
 

}
