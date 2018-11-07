using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaGallery.Data.Migrations
{
    public partial class EsimesedMeediaKlassid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    ParentFolderId = table.Column<int>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    Video_FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItem_MediaItem_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "MediaItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaItem_ParentFolderId",
                table: "MediaItem",
                column: "ParentFolderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaItem");
        }
    }
}
