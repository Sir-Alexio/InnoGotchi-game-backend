using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoGotchi_backend.Migrations
{
    public partial class removecolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "HungerLevel",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "ThirstyLevel",
                table: "Pets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HungerLevel",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThirstyLevel",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
