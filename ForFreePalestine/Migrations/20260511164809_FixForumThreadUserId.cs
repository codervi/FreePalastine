using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForFreePalestine.Migrations
{
    /// <inheritdoc />
    public partial class FixForumThreadUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumThreads_UserInfos_UsersUserId",
                table: "ForumThreads");

            migrationBuilder.DropIndex(
                name: "IX_ForumThreads_UsersUserId",
                table: "ForumThreads");

            migrationBuilder.DropColumn(
                name: "UsersUserId",
                table: "ForumThreads");

            migrationBuilder.CreateIndex(
                name: "IX_ForumThreads_UserId",
                table: "ForumThreads",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThreads_UserInfos_UserId",
                table: "ForumThreads",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumThreads_UserInfos_UserId",
                table: "ForumThreads");

            migrationBuilder.DropIndex(
                name: "IX_ForumThreads_UserId",
                table: "ForumThreads");

            migrationBuilder.AddColumn<int>(
                name: "UsersUserId",
                table: "ForumThreads",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ForumThreads_UsersUserId",
                table: "ForumThreads",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumThreads_UserInfos_UsersUserId",
                table: "ForumThreads",
                column: "UsersUserId",
                principalTable: "UserInfos",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
