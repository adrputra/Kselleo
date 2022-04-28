using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addroleonverifytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "VerifyInvites",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "VerifyInvites");
        }
    }
}
