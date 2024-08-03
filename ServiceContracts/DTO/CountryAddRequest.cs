using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServiceContracts.DTO
{
    public class CountryAddRequest
    {
        [Required]
        public string? CountryName { get; set; }

        public Country ToCountry()
        {
            Country country = new Country() { CountryName = this.CountryName};
            return country;
        }

    }
}
