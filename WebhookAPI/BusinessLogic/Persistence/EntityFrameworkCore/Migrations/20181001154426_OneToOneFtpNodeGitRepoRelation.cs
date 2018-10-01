using Microsoft.EntityFrameworkCore.Migrations;

namespace Gitloy.Services.WebhookAPI.Migrations
{
    public partial class OneToOneFtpNodeGitRepoRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FtpNodeId",
                table: "GitRepos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GitRepos_FtpNodeId",
                table: "GitRepos",
                column: "FtpNodeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GitRepos_FtpNodes_FtpNodeId",
                table: "GitRepos",
                column: "FtpNodeId",
                principalTable: "FtpNodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GitRepos_FtpNodes_FtpNodeId",
                table: "GitRepos");

            migrationBuilder.DropIndex(
                name: "IX_GitRepos_FtpNodeId",
                table: "GitRepos");

            migrationBuilder.DropColumn(
                name: "FtpNodeId",
                table: "GitRepos");
        }
    }
}
