using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPersons_StoreProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
        CREATE PROCEDURE [dbo].[sp_GetAllPersons]
        AS
        BEGIN
            SELECT * FROM [dbo].[Persons]
        END
";

            migrationBuilder.Sql(sp_GetAllPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql = @"
    DROP PROCEDURE [dbo].[sp_GetAllPersons]
";
            migrationBuilder.Sql(sql);
        }
    }
}
