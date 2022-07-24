using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cfefb129-6848-4437-afd6-8744ab51e290", "24dbd532-4c42-412d-9eb4-1d5e90e859f9", "Manager", "MANAGER" },
                    { "f90b7b10-07a7-4296-ab55-ee1a700d6896", "d04397bd-87ca-4fe5-b513-363d7832bcd9", "Administrator", "ADMINISTRATOR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cfefb129-6848-4437-afd6-8744ab51e290");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f90b7b10-07a7-4296-ab55-ee1a700d6896");
        }
    }
}
