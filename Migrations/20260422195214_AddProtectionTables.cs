using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artify.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProtectionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtworkHashes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HashValue = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkHashes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtworkHashes_Artworks_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "Artworks",
                        principalColumn: "ArtworkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtworkMetadataLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtworkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CopyrightText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtworkMetadataLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArtworkMetadataLogs_Artworks_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "Artworks",
                        principalColumn: "ArtworkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkHashes_ArtworkId",
                table: "ArtworkHashes",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtworkMetadataLogs_ArtworkId",
                table: "ArtworkMetadataLogs",
                column: "ArtworkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtworkHashes");

            migrationBuilder.DropTable(
                name: "ArtworkMetadataLogs");
        }
    }
}
