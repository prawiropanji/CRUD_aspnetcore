using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Reflection;
using Azure.Identity;

namespace Entities
{
    public class PersonDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Country> Countries { get; set; }

        public PersonDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //mapping entity class Person to table named "Persons"
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<Country>().ToTable("Countries");

            //seed data to table
            //modelBuilder.Entity<Country>().HasData(new Country() {CountryId = Guid.NewGuid(), CountryName = "Japan" });

            //seed data using json file
            string listCountriesJson = File.ReadAllText("Countries.json");
            string listPersonsJson = File.ReadAllText("Persons.json");

            List<Country>? countries = JsonSerializer.Deserialize<List<Country>>(listCountriesJson);
            if (countries is not null)
            {
                foreach (var country in countries)
                {


                    modelBuilder.Entity<Country>().HasData(country);
                }
            }

            List<Person>? persons = JsonSerializer.Deserialize<List<Person>>(listPersonsJson);
            if (persons is not null)
            {
                foreach (var person in persons)
                {


                    modelBuilder.Entity<Person>().HasData(person);
                }
            }


            //configure column table TIN
            modelBuilder.Entity<Person>().Property(e => e.TIN).HasColumnType("varchar(8)").HasColumnName("TaxIdentifierNumber");



        }

        public List<Person> Sp_GetAllPersons()
        {
            return Persons.FromSqlRaw(@"
                EXECUTE [dbo].[sp_GetAllPersons]
            ").ToList();
        }

        public int sp_InsertPerson(Person person)
        {

            var person_param = new SqlParameter[] {
             new SqlParameter("@PersonId", person.PersonId),
             new SqlParameter("@PersonName", person.PersonName),
             new SqlParameter("@Email", person.Email),
             new SqlParameter("@DateOfBirth", person.DateOfBirth),
             new SqlParameter("@Gender", person.Gender),
             new SqlParameter("@CountryId", person.CountryId),
             new SqlParameter("@Address", person.Address),
             new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
         };

            int rowAffected = Database.ExecuteSqlRaw("EXECUTE [dbo].[sp_InsertPerson] @PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters", person_param);
            return rowAffected;
        }
    }

}