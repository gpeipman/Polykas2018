using Microsoft.EntityFrameworkCore.Migrations;

namespace MediaGallery.Data.Migrations
{
    public partial class LatLon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaItem_MediaItem_ParentFolderId",
                table: "MediaItem");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaItem",
                table: "MediaItem");

            migrationBuilder.RenameTable(
                name: "MediaItem",
                newName: "Items");

            migrationBuilder.RenameIndex(
                name: "IX_MediaItem_ParentFolderId",
                table: "Items",
                newName: "IX_Items_ParentFolderId");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Video_Latitude",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Video_Longitude",
                table: "Items",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Items_ParentFolderId",
                table: "Items",
                column: "ParentFolderId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Items_ParentFolderId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Video_Latitude",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Video_Longitude",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "MediaItem");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ParentFolderId",
                table: "MediaItem",
                newName: "IX_MediaItem_ParentFolderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaItem",
                table: "MediaItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaItem_MediaItem_ParentFolderId",
                table: "MediaItem",
                column: "ParentFolderId",
                principalTable: "MediaItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
