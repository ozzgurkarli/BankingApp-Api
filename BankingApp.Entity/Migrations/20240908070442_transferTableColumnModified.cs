using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Entity.Migrations
{
    /// <inheritdoc />
    public partial class transferTableColumnModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientAccount",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "SenderAccount",
                table: "Transfer");

            migrationBuilder.AddColumn<int>(
                name: "RecipientAccountId",
                table: "Transfer",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderAccountId",
                table: "Transfer",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_RecipientAccountId",
                table: "Transfer",
                column: "RecipientAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_SenderAccountId",
                table: "Transfer",
                column: "SenderAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_Account_RecipientAccountId",
                table: "Transfer",
                column: "RecipientAccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_Account_SenderAccountId",
                table: "Transfer",
                column: "SenderAccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_Account_RecipientAccountId",
                table: "Transfer");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_Account_SenderAccountId",
                table: "Transfer");

            migrationBuilder.DropIndex(
                name: "IX_Transfer_RecipientAccountId",
                table: "Transfer");

            migrationBuilder.DropIndex(
                name: "IX_Transfer_SenderAccountId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "RecipientAccountId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "SenderAccountId",
                table: "Transfer");

            migrationBuilder.AddColumn<string>(
                name: "RecipientAccount",
                table: "Transfer",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderAccount",
                table: "Transfer",
                type: "text",
                nullable: true);
        }
    }
}
