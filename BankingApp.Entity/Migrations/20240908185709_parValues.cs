using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingApp.Entity.Migrations
{
    /// <inheritdoc />
    public partial class parValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccountTracker",
                columns: new[] { "Id", "Currency", "FirstAvailableNo", "RecordDate", "RecordScreen" },
                values: new object[,]
                {
                    { 1, "1", "1000000000000001", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7410), "ADMIN" },
                    { 2, "2", "2000000000000001", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7410), "ADMIN" },
                    { 3, "3", "3000000000000001", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7410), "ADMIN" },
                    { 4, "4", "4000000000000001", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7410), "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Parameter",
                columns: new[] { "Id", "Code", "Description", "Detail1", "Detail2", "Detail3", "GroupCode", "RecordDate", "RecordScreen" },
                values: new object[,]
                {
                    { 1, 1, "İstanbul", null, null, null, "City", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7250), "ADMIN" },
                    { 2, 2, "İzmir", null, null, null, "City", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7300), "ADMIN" },
                    { 3, 3, "Çanakkale", null, null, null, "City", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7300), "ADMIN" },
                    { 4, 4, "Ankara", null, null, null, "City", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7300), "ADMIN" },
                    { 5, 1, "Beykoz", "1", null, null, "District", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7300), "ADMIN" },
                    { 6, 2, "Şişli", "1", null, null, "District", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7300), "ADMIN" },
                    { 7, 3, "Avcılar", "1", null, null, "District", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7310), "ADMIN" },
                    { 8, 4, "Karşıyaka", "2", null, null, "District", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7310), "ADMIN" },
                    { 9, 5, "Merkez", "3", null, null, "District", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7310), "ADMIN" },
                    { 10, 6, "Çankaya", "4", null, null, "District", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7310), "ADMIN" },
                    { 11, 1, "Mamak", "4", null, null, "District", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7310), "ADMIN" },
                    { 12, 1, "Gold Plus", null, null, null, "CardType", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7310), "ADMIN" },
                    { 13, 2, "Midnight Prestige", null, null, null, "CardType", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7310), "ADMIN" },
                    { 14, 1, "Mühendis:Engineer", null, null, null, "Profession", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7320), "ADMIN" },
                    { 15, 2, "Mimar:Architect", null, null, null, "Profession", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7320), "ADMIN" },
                    { 16, 1, "Erkek:Male", null, null, null, "Gender", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7320), "ADMIN" },
                    { 17, 2, "Kadın:Female", null, null, null, "Gender", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7320), "ADMIN" },
                    { 18, 1, "TL", "Türk Lirasi:Turkish Lira", null, null, "Currency", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7320), "ADMIN" },
                    { 19, 4, "JPY", "Japon Yeni:Japanese Yen", null, null, "Currency", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7320), "ADMIN" },
                    { 20, 2, "EUR", "Euro:Euro", null, null, "Currency", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7330), "ADMIN" },
                    { 21, 3, "USD", "Amerikan Doları:U.S. Dollar", null, null, "Currency", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Utc).AddTicks(7330), "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 21);
        }
    }
}
