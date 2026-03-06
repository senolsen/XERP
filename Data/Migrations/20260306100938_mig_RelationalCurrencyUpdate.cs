using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class mig_RelationalCurrencyUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParaBirimi",
                table: "DovizKurlari");

            migrationBuilder.AddColumn<int>(
                name: "ParaBirimiId",
                table: "DovizKurlari",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DovizKurlari_ParaBirimiId",
                table: "DovizKurlari",
                column: "ParaBirimiId");

            migrationBuilder.AddForeignKey(
                name: "FK_DovizKurlari_ParaBirimleri_ParaBirimiId",
                table: "DovizKurlari",
                column: "ParaBirimiId",
                principalTable: "ParaBirimleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DovizKurlari_ParaBirimleri_ParaBirimiId",
                table: "DovizKurlari");

            migrationBuilder.DropIndex(
                name: "IX_DovizKurlari_ParaBirimiId",
                table: "DovizKurlari");

            migrationBuilder.DropColumn(
                name: "ParaBirimiId",
                table: "DovizKurlari");

            migrationBuilder.AddColumn<string>(
                name: "ParaBirimi",
                table: "DovizKurlari",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
