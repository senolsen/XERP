using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_AddMusteriAndUrunRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MusteriAdi",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "UrunAdi",
                table: "TeklifKalemleri");

            migrationBuilder.AddColumn<int>(
                name: "MusteriId",
                table: "Teklifler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UrunId",
                table: "TeklifKalemleri",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unvan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VergiDairesi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VergiNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunKodu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrunAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birimi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuncelFiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KdvOrani = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teklifler_MusteriId",
                table: "Teklifler",
                column: "MusteriId");

            migrationBuilder.CreateIndex(
                name: "IX_TeklifKalemleri_UrunId",
                table: "TeklifKalemleri",
                column: "UrunId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeklifKalemleri_Urunler_UrunId",
                table: "TeklifKalemleri",
                column: "UrunId",
                principalTable: "Urunler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teklifler_Musteriler_MusteriId",
                table: "Teklifler",
                column: "MusteriId",
                principalTable: "Musteriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeklifKalemleri_Urunler_UrunId",
                table: "TeklifKalemleri");

            migrationBuilder.DropForeignKey(
                name: "FK_Teklifler_Musteriler_MusteriId",
                table: "Teklifler");

            migrationBuilder.DropTable(
                name: "Musteriler");

            migrationBuilder.DropTable(
                name: "Urunler");

            migrationBuilder.DropIndex(
                name: "IX_Teklifler_MusteriId",
                table: "Teklifler");

            migrationBuilder.DropIndex(
                name: "IX_TeklifKalemleri_UrunId",
                table: "TeklifKalemleri");

            migrationBuilder.DropColumn(
                name: "MusteriId",
                table: "Teklifler");

            migrationBuilder.DropColumn(
                name: "UrunId",
                table: "TeklifKalemleri");

            migrationBuilder.AddColumn<string>(
                name: "MusteriAdi",
                table: "Teklifler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrunAdi",
                table: "TeklifKalemleri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
