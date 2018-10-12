using Microsoft.EntityFrameworkCore.Migrations;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.Migrations
{
    public partial class AddOneToManyDeploymentDeploymentLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeploymentId",
                table: "DeploymentLogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentLogs_DeploymentId",
                table: "DeploymentLogs",
                column: "DeploymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeploymentLogs_Deployments_DeploymentId",
                table: "DeploymentLogs",
                column: "DeploymentId",
                principalTable: "Deployments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeploymentLogs_Deployments_DeploymentId",
                table: "DeploymentLogs");

            migrationBuilder.DropIndex(
                name: "IX_DeploymentLogs_DeploymentId",
                table: "DeploymentLogs");

            migrationBuilder.DropColumn(
                name: "DeploymentId",
                table: "DeploymentLogs");
        }
    }
}
