using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackXpert_API.Migrations
{
    /// <inheritdoc />
    public partial class WaveformDataDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileInfo_WaveformData",
                table: "Tracks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileInfo_WaveformData",
                table: "Tracks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
