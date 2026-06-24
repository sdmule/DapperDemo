using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DapperDemo.Migrations
{
    /// <inheritdoc />
    public partial class ReplacedCountryWithPostalCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Companies",
                newName: "PostalCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Companies",
                newName: "Country");
        }
    }
}
