using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fora.Server.Migrations
{
    public partial class AddedSeedingInterests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Interests",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "Games", null },
                    { 2, "Sports", null },
                    { 3, "Politics", null },
                    { 4, "Religion", null },
                    { 5, "Design", null },
                    { 6, "Garden", null },
                    { 7, "Technology", null },
                    { 8, "Pets", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
