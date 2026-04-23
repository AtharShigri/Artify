using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artify.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedToArtistProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "ArtistProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ArtistProfiles");
        }
    }
}
