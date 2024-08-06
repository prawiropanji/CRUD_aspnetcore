﻿using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Country
    {
        [Key]
        public Guid CountryId { get; set; }
        [StringLength(56)]
        public string? CountryName { get; set; } 

        public ICollection<Person>? Persons{ get; set; }

    }


}
