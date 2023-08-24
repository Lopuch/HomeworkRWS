using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TranslationManagement.Application.Migrations
{
    /// <inheritdoc />
    public partial class MigrDecimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "TranslationJobs",
                type: "TEXT",
                precision: 14,
                scale: 4,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "TranslationJobs",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldPrecision: 14,
                oldScale: 4);
        }
    }
}
