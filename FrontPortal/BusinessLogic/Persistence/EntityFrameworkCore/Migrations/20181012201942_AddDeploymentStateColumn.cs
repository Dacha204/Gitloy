using Microsoft.EntityFrameworkCore.Migrations;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.Migrations
{
    public partial class AddDeploymentStateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Deployments",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Deployments");
        }
    }
}
