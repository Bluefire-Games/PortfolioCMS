using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioCMS.Migrations
{
    /// <inheritdoc />
    public partial class SerializeImageList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "ImagePathsSerialized",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePathsSerialized",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }
    }
}
