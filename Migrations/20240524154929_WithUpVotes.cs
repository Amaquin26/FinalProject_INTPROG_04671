using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionWebsite.Migrations
{
    /// <inheritdoc />
    public partial class WithUpVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpVotes",
                table: "Designs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpVotes",
                table: "Designs");
        }
    }
}
