using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingApp.Entity.Migrations
{
    /// <inheritdoc />
    public partial class ParameterValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Detail1", "Detail5" },
                values: new object[] { "Türk Lirası;Turkish Lira", "3" });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 19,
                column: "Detail5",
                value: "3");

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 20,
                column: "Detail5",
                value: "3");

            migrationBuilder.InsertData(
                table: "Parameter",
                columns: new[] { "Id", "Code", "Description", "Detail1", "Detail2", "Detail3", "Detail4", "Detail5", "GroupCode", "RecordDate", "RecordScreen" },
                values: new object[,]
                {
                    { 22, 3, "XAU", "Altın;Gold", "XU", null, null, "3", "Currency", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN" },
                    { 23, 5, "XAG", "Gümüş;Silver", "XG", null, null, "3", "Currency", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN" },
                    { 24, 6, "RUB", "Ruble;Ruble", "RU", null, null, "3", "Currency", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Detail1", "Detail5" },
                values: new object[] { "Türk Lirasi;Turkish Lira", null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 19,
                column: "Detail5",
                value: null);

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 20,
                column: "Detail5",
                value: null);
        }
    }
}
