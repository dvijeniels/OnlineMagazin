using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMagazin.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_OnlineMagazinUserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_OnlineMagazinUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "OnlineMagazinUserId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "UserName");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "OnlineMagazinUserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OnlineMagazinUserId",
                table: "Comments",
                column: "OnlineMagazinUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_OnlineMagazinUserId",
                table: "Comments",
                column: "OnlineMagazinUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
