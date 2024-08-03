using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? CountryName { get; set; }

        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public int? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(obj.GetType() != typeof(PersonResponse))
            {
                return false;
            }

            PersonResponse personResponse = (PersonResponse)obj;
            if (
                personResponse.PersonId != this.PersonId ||
                personResponse.PersonName != this.PersonName ||
                personResponse.Email != this.Email ||
                personResponse.ReceiveNewsLetters != this.ReceiveNewsLetters ||
                personResponse.Address != this.Address ||
                personResponse.Gender != this.Gender ||
                personResponse.CountryId != this.CountryId ||
                personResponse.CountryName != this.CountryName ||
                personResponse.Age != this.Age ||
                personResponse.DateOfBirth != this.DateOfBirth
                ) {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"PersonResponse{{PersonId:{PersonId}, " +
                $"PersonName:{PersonName}, Email:{Email}, " +
                $"ReceiveNewsLatters:{ReceiveNewsLetters}, " +
                $"Address:{Address}, " +
                $"Gender:{Gender}, " +
                $"CountryId:{CountryId}, " +
                $"CountryName:{CountryName}," +
                $"Age:{Age}," +
                $"DateOfBirth:{DateOfBirth} }}";

        }

    }


    public static class PersonExtension
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {

            
            PersonResponse personResponse = new PersonResponse() {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryId = person.CountryId,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = person.DateOfBirth is not null ? Convert.ToInt32(Math.Round((DateTime.Now - person.DateOfBirth).Value.TotalDays / 365.25)) : null
            };

            return personResponse;
        }
    }
}
