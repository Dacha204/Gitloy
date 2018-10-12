using Microsoft.EntityFrameworkCore.Migrations;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.Migrations
{
    public partial class AddDeployUserRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Deployments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_UserId",
                table: "Deployments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deployments_AspNetUsers_UserId",
                table: "Deployments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deployments_AspNetUsers_UserId",
                table: "Deployments");

            migrationBuilder.DropIndex(
                name: "IX_Deployments_UserId",
                table: "Deployments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Deployments");
        }
    }
}
