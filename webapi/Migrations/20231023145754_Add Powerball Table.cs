using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddPowerballTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyPowerball",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint IDENTITY", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    NumOne = table.Column<int>(type: "int", nullable: false),
                    NumTwo = table.Column<int>(type: "int", nullable: false),
                    NumThree = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPowerball", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyPowerballWinners",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint IDENTITY", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    NumOne = table.Column<int>(type: "int", nullable: false),
                    NumTwo = table.Column<int>(type: "int", nullable: false),
                    NumThree = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPowerballWinners", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "DailyPowerball");
            migrationBuilder.DropTable(name: "DailyPowerballWinners");
        }
    }
}
