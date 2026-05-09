using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForFreePalestine.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleToUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "UserInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "UserInfos");
        }
    }
}
