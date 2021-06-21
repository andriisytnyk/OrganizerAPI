using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizerAPI.Infrastructure.Migrations
{
    public partial class ChangesInAllTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_User",
                table: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "RefreshToken",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_User",
                table: "RefreshToken",
                newName: "IX_RefreshToken_UserId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_UserId",
                table: "UserTasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_Users_UserId",
                table: "UserTasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_Users_UserId",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_Users_UserId",
                table: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_UserId",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserTasks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RefreshToken",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                newName: "IX_RefreshToken_User");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_Users_User",
                table: "RefreshToken",
                column: "User",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
