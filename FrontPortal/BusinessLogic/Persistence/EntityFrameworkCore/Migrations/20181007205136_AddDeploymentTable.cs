using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Persistence.EntityFrameworkCore.Migrations
{
    public partial class AddDeploymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deployments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<Guid>(nullable: false),
                    DeleteFlag = table.Column<bool>(nullable: false),
                    GitUrl = table.Column<string>(nullable: false),
                    GitBranch = table.Column<string>(nullable: true),
                    FtpUsername = table.Column<string>(nullable: true),
                    FtpPassword = table.Column<string>(nullable: false),
                    FtpHostname = table.Column<string>(nullable: false),
                    FtpPort = table.Column<int>(nullable: false),
                    FtpRootDirectory = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deployments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deployments");
        }
    }
}
