using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Entity.Migrations
{
    /// <inheritdoc />
    public partial class TransactionHistoryColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "TransactionHistory",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "TransactionHistory",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "TransactionHistory",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "TransactionHistory",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                table: "Parameter",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");


            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_CustomerId",
                table: "TransactionHistory",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistory_Customer_CustomerId",
                table: "TransactionHistory",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistory_Customer_CustomerId",
                table: "TransactionHistory");

            migrationBuilder.DropIndex(
                name: "IX_TransactionHistory_CustomerId",
                table: "TransactionHistory");

            migrationBuilder.DeleteData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "TransactionHistory");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "TransactionHistory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransactionDate",
                table: "TransactionHistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "TransactionHistory",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                table: "Parameter",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 1,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7410));

            migrationBuilder.UpdateData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 2,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7410));

            migrationBuilder.UpdateData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 3,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7410));

            migrationBuilder.UpdateData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 4,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7410));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 1,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7250));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 2,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 3,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 4,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 5,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 6,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 7,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 8,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 9,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 10,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 11,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 12,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 13,
                column: "RecordDate",
                value: new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Description", "RecordDate" },
                values: new object[] { "Mühendis:Engineer", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7320) });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Description", "RecordDate" },
                values: new object[] { "Mimar:Architect", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7320) });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Description", "RecordDate" },
                values: new object[] { "Erkek:Male", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7320) });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Description", "RecordDate" },
                values: new object[] { "Kadın:Female", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7320) });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Detail1", "Detail2", "RecordDate" },
                values: new object[] { "Türk Lirasi:Turkish Lira", null, new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7320) });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Detail1", "Detail2", "RecordDate" },
                values: new object[] { "Japon Yeni:Japanese Yen", null, new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7320) });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Detail1", "Detail2", "RecordDate" },
                values: new object[] { "Euro:Euro", null, new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7330) });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Code", "Description", "Detail1", "GroupCode", "RecordDate" },
                values: new object[] { 3, "USD", "Amerikan Doları:U.S. Dollar", "Currency", new DateTime(2024, 9, 8, 21, 57, 9, 357, DateTimeKind.Local).AddTicks(7330) });
        }
    }
}
