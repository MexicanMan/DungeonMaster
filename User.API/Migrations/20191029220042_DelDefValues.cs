using Microsoft.EntityFrameworkCore.Migrations;

namespace User.API.Migrations
{
    public partial class DelDefValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxHP",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 3);

            migrationBuilder.AlterColumn<int>(
                name: "CurrentHP",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MaxHP",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CurrentHP",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(int));
        }
    }
}
