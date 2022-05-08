using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class change_field_comment_to_text : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comment_",
                table: "Comments",
                newName: "Text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Comments",
                newName: "Comment_");
        }
    }
}
