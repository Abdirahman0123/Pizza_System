using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pizza_System.Migrations
{
    public partial class Address1Address2Street : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "address2",
                table: "Orders",
                newName: "Address2");

            migrationBuilder.RenameColumn(
                name: "address1",
                table: "Orders",
                newName: "Address1");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Street",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Address2",
                table: "Orders",
                newName: "address2");

            migrationBuilder.RenameColumn(
                name: "Address1",
                table: "Orders",
                newName: "address1");
        }
    }
}
