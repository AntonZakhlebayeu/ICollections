using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICollections.Migrations
{
    public partial class AddedComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "496b5825-7298-4d21-a450-b7cd19621b2f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a0a06da-2849-4f05-880d-4f6fc2ca1a71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc076996-c081-4854-a6c5-ae05e7fc414b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2708f90e-6066-49f3-9ea1-a78bb65cafdf", "9e7c2783-2c13-4ef8-ac21-c120045ac079", "super admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "48f27907-10e4-4874-894e-e242bc0db24f", "c9852362-cd9f-46cf-bad0-790b60b4184e", "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "eaa02b7b-6800-4bac-9dc4-6afcf5c0371f", "74e43672-7dfb-4379-ba03-af83707c4281", "user", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2708f90e-6066-49f3-9ea1-a78bb65cafdf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "48f27907-10e4-4874-894e-e242bc0db24f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eaa02b7b-6800-4bac-9dc4-6afcf5c0371f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "496b5825-7298-4d21-a450-b7cd19621b2f", "370e7949-1f56-4ab8-9303-ea7df07d6c81", "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7a0a06da-2849-4f05-880d-4f6fc2ca1a71", "a3777fc9-110b-4757-a0bc-ff80530639cf", "super admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bc076996-c081-4854-a6c5-ae05e7fc414b", "141653f2-2330-4266-8d42-e7ccbffac16d", "user", null });
        }
    }
}
