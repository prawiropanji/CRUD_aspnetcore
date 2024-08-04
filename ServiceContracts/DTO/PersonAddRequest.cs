using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{

    public class PersonAddRequest
    {
        [Required]
        [DisplayName("Person Name")]
        public string? PersonName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public GenderOptions? Gender { get; set; }
        [Required]
        public Guid? CountryId { get; set; }
        [Required]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson()
        {
            Person person = new Person() { 
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender.ToString(),
                CountryId = this.CountryId,
                Address = this.Address,
                ReceiveNewsLetters = this.ReceiveNewsLetters,
            };

            return person;
        }


    }
}
