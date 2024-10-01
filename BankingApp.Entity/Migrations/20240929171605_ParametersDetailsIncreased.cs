using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingApp.Entity.Migrations
{
    /// <inheritdoc />
    public partial class ParametersDetailsIncreased : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Detail4",
                table: "Parameter",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Detail5",
                table: "Parameter",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Parameter",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Detail4", "Detail5" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Detail4",
                table: "Parameter");

            migrationBuilder.DropColumn(
                name: "Detail5",
                table: "Parameter");
        }
    }
}
