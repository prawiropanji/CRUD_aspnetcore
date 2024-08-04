using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage ="Must provide {0}")]
        [DisplayName("Person Id")]
        public Guid PersonId { get; set; }
        [Required]
        public string? PersonName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

   

    }
}
