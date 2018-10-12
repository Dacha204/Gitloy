using Microsoft.EntityFrameworkCore.Migrations;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.Migrations
{
    public partial class AddDeploymentLogEventTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "DeploymentLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "DeploymentLogs");
        }
    }
}
