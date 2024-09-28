using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankingApp.Entity.Migrations
{
    /// <inheritdoc />
    public partial class testValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Customer_CustomerId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditCard_Customer_CustomerId",
                table: "CreditCard");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "TransactionHistory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "CreditCard",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                table: "CreditCard",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                table: "Account",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 1,
                column: "FirstAvailableNo",
                value: "1000000000000003");

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "Id", "Active", "CreditScore", "Gender", "IdentityNo", "Name", "PhoneNo", "Profession", "RecordDate", "RecordScreen", "Salary", "Surname" },
                values: new object[,]
                {
                    { 100000000000L, true, 1000, 1, "11111111111", "Test", "+90 (533) 333 33-33", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN", 5000m, "Data" },
                    { 100000000001L, false, 300, 1, "11111111112", "Test2", "+90 (533) 333 33-34", 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN", 40000m, "Data2" }
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "Id", "AccountNo", "Active", "Balance", "Branch", "Currency", "CustomerId", "Primary", "RecordDate", "RecordScreen" },
                values: new object[,]
                {
                    { 1, "1000000000000000", true, 2000.5m, 2, "TL", 100000000000L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN" },
                    { 2, "1000000000000001", false, 7.5m, 2, "TL", 100000000000L, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN" },
                    { 3, "1000000000000002", true, 0m, 2, "TL", 100000000001L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN" },
                    { 4, "2000000000000000", true, 0m, 2, "EUR", 100000000001L, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "MailAddress",
                columns: new[] { "Id", "CustomerId", "MailAddress", "Primary", "RecordDate", "RecordScreen" },
                values: new object[,]
                {
                    { 1, 100000000000L, "ozgurkrl533@hotmail.com", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN" },
                    { 2, 100000000001L, "karli2002ozgur@hotmail.com", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Customer_CustomerId",
                table: "Account",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCard_Customer_CustomerId",
                table: "CreditCard",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Customer_CustomerId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditCard_Customer_CustomerId",
                table: "CreditCard");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MailAddress",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MailAddress",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 100000000000L);

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "Id",
                keyValue: 100000000001L);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "TransactionHistory",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "CreditCard",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                table: "CreditCard",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CustomerId",
                table: "Account",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "AccountTracker",
                keyColumn: "Id",
                keyValue: 1,
                column: "FirstAvailableNo",
                value: "1000000000000001");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Customer_CustomerId",
                table: "Account",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCard_Customer_CustomerId",
                table: "CreditCard",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
