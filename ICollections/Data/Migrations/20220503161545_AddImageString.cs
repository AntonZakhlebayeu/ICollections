using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICollections.Data.Migrations
{
    public partial class AddImageString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36b6da0d-d47f-414d-a181-f66794eaa1b8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5fa8faab-905e-4ba8-a8c8-da74aaa36750");

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    CollectionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AuthorId = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Theme = table.Column<string>(type: "TEXT", nullable: false),
                    LastEditName = table.Column<string>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: true),
                    AddDates = table.Column<string>(type: "TEXT", nullable: true),
                    AddBrands = table.Column<string>(type: "TEXT", nullable: true),
                    AddComments = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.CollectionId);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    LastEditDate = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    Brand = table.Column<string>(type: "TEXT", nullable: true),
                    FileName = table.Column<string>(type: "TEXT", nullable: true),
                    CollectionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9760ed61-1fb2-4dae-ae0a-6613b09c3194", "4dae496a-4e0d-47b5-81f3-f976d5f562ac", "super admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a5c3f93f-699e-4d2e-9ab3-47cd244408bb", "ca1efd90-e286-4d5d-a9b0-37fdd55d2be6", "user", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e79d487b-b7b5-43e1-9702-94931fc197d7", "2cb4d7d9-3495-43d4-a2ec-c9ec5c68e18a", "admin", null });

            migrationBuilder.CreateIndex(
                name: "IX_Items_CollectionId",
                table: "Items",
                column: "CollectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9760ed61-1fb2-4dae-ae0a-6613b09c3194");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5c3f93f-699e-4d2e-9ab3-47cd244408bb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e79d487b-b7b5-43e1-9702-94931fc197d7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "36b6da0d-d47f-414d-a181-f66794eaa1b8", "9e30eda2-e1be-4fdd-a26f-6a979f1f3891", "admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5fa8faab-905e-4ba8-a8c8-da74aaa36750", "0cd37185-4764-4907-8d3a-d17b76feabb3", "user", null });
        }
    }
}
