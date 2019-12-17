using Microsoft.EntityFrameworkCore.Migrations;

namespace Room.API.Migrations
{
    public partial class BoolRoomProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EastRoom",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NorthRoom",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SouthRoom",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WestRoom",
                table: "Rooms",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EastRoom",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NorthRoom",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "SouthRoom",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "WestRoom",
                table: "Rooms");
        }
    }
}
