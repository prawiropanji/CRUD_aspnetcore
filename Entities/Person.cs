using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        [MaxLength]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(20)]
        public String? Gender { get; set; }
        public Guid? CountryId { get; set; }
        [MaxLength]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public string? TIN { get; set; }

        [ForeignKey("CountryId")]
        public Country? country { get; set; }
  
    }
}
