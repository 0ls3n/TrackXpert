using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackXpert_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialLinks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Metadata_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metadata_Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metadata_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metadata_ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Metadata_Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metadata_Bpm = table.Column<double>(type: "float", nullable: true),
                    Metadata_Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metadata_IsExplicit = table.Column<bool>(type: "bit", nullable: true),
                    Analytics_FeedbackCount = table.Column<int>(type: "int", nullable: true),
                    Analytics_Likes = table.Column<int>(type: "int", nullable: true),
                    Analytics_PlaybackCount = table.Column<int>(type: "int", nullable: true),
                    FileInfo_Format = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileInfo_Size = table.Column<long>(type: "bigint", nullable: true),
                    FileInfo_PreviewUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileInfo_WaveformData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileInfo_UploadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileInfo_UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileInfo_UploadedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileInfo_IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    ProcessingStatus_ProcessingStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackArtists",
                columns: table => new
                {
                    TrackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_TrackArtists_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackArtists_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtists_ArtistId",
                table: "TrackArtists",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtists_TrackId",
                table: "TrackArtists",
                column: "TrackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackArtists");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Tracks");
        }
    }
}
