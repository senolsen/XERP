using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_AddIndirimToTeklif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ToplamIndirim",
                table: "Teklifler",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "IndirimOrani",
                table: "TeklifKalemleri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "IndirimTutari",
                table: "TeklifKalemleri",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToplamIndirim",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "IndirimOrani",
                table: "TeklifKalemleri");

            migrationBuilder.DropColumn(
                name: "IndirimTutari",
                table: "TeklifKalemleri");
        }
    }
}
