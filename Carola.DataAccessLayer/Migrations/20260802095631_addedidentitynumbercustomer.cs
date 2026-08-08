using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carola.DataAccessLayer.Migrations
{
    public partial class addedidentitynumbercustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                table: "Customers");
        }
    }
}
