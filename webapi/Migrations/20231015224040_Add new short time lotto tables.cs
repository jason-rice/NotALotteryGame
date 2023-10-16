using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Addnewshorttimelottotables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwoMinuteLottery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint IDENTITY", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoMinuteLottery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FiveMinuteLottery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint IDENTITY", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiveMinuteLottery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThirtyMinuteLottery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint IDENTITY", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirtyMinuteLottery", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TwoMinuteLottery");
            migrationBuilder.DropTable(name: "FiveMinuteLottery");
            migrationBuilder.DropTable(name: "ThirtyMinuteLottery");
        }
    }
}
