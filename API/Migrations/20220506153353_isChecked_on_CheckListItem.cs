using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class isChecked_on_CheckListItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChecked",
                table: "CheckListItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChecked",
                table: "CheckListItems");
        }
    }
}
