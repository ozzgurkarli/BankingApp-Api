using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Entity.Migrations
{
    /// <inheritdoc />
    public partial class StoredProcedureForGettingMailAddressByMailAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE SelectMailAddressByMailAddress
                    @MailAddress VARCHAR
                AS
                BEGIN
                    SELECT TOP 1 M.MAILADDRESS AS MAILADDRESS
                    FROM MAILADDRESSES M
                    WHERE M.MAILADDRESS = @MailAddress
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
