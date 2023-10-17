using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class MyKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MyKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int IDENTITY", nullable: false),
                    KeyString = table.Column<string>(type: "nvarchar(200)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyKeys", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyKeys");
        }
    }
}
