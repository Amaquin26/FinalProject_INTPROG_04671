using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddedUpVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpVotes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DesignId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpVotes", x => new { x.UserId, x.DesignId });
                    table.ForeignKey(
                        name: "FK_UpVotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UpVotes_Designs_DesignId",
                        column: x => x.DesignId,
                        principalTable: "Designs",
                        principalColumn: "Id"
                     );
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpVotes_DesignId",
                table: "UpVotes",
                column: "DesignId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpVotes");
        }
    }
}
