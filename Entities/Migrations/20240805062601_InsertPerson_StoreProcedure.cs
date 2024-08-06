using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoreProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
        CREATE PROCEDURE [dbo].[sp_InsertPerson]
        @PersonId UNIQUEIDENTIFIER,
        @PersonName NVARCHAR(MAX),
        @Email NVARCHAR(MAX),
        @DateOfBirth DATETIME2(7),
        @Gender NVARCHAR(20),
        @CountryId UNIQUEIDENTIFIER,
        @Address NVARCHAR(MAX),
        @ReceiveNewsLetters BIT
        AS
        BEGIN
             INSERT INTO Persons (PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters)
            VALUES (@PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryId, @Address, @ReceiveNewsLetters);
        END
";

            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql = @"
    DROP PROCEDURE [dbo].[sp_InsertPerson]
";
            migrationBuilder.Sql(sql);
        }
    }
}
