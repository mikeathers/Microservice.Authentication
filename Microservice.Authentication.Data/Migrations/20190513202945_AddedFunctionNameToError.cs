using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservice.Authentication.Data.Migrations
{
    public partial class AddedFunctionNameToError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FunctionName",
                table: "Errors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FunctionName",
                table: "Errors");
        }
    }
}
