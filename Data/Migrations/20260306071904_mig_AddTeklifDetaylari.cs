using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_AddTeklifDetaylari : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tutar",
                table: "TeklifKalemleri",
                newName: "ToplamTutar");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Teklifler",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "AraToplam",
                table: "Teklifler",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "BaslangicTarihi",
                table: "Teklifler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BitisTarihi",
                table: "Teklifler",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ToplamKdv",
                table: "Teklifler",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Miktar",
                table: "TeklifKalemleri",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "KdvOrani",
                table: "TeklifKalemleri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "KdvTutari",
                table: "TeklifKalemleri",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SiraNo",
                table: "TeklifKalemleri",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AraToplam",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "BaslangicTarihi",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "BitisTarihi",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "ToplamKdv",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "KdvOrani",
                table: "TeklifKalemleri");

            migrationBuilder.DropColumn(
                name: "KdvTutari",
                table: "TeklifKalemleri");

            migrationBuilder.DropColumn(
                name: "SiraNo",
                table: "TeklifKalemleri");

            migrationBuilder.RenameColumn(
                name: "ToplamTutar",
                table: "TeklifKalemleri",
                newName: "Tutar");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Teklifler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Miktar",
                table: "TeklifKalemleri",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
