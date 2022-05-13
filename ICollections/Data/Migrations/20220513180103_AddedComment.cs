using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICollections.Data.Migrations
{
    public partial class AddedComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66b4c163-70d8-4dac-8480-1059cced6f34");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f391319-8799-4569-8732-01efacf4b566");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "94509748-8612-4ab2-bbab-2a193e74c8b1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "66b4c163-70d8-4dac-8480-1059cced6f34", "fa589bd1-6a50-4c90-90a0-b0943280d3cd", "user", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8f391319-8799-4569-8732-01efacf4b566", "bc062417-dd29-42f8-b2e9-8208f6cdce9c", "super admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "94509748-8612-4ab2-bbab-2a193e74c8b1", "b592d9b7-b66d-4b75-bbcf-e6c9bb43df77", "admin", null });
        }
    }
}
