using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

// Open Package Manager Console
// Add-Migration "name of migration"
// Update-Database

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OneHourLottery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint IDENTITY", nullable: false),
                    //Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneHourLottery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwoHourLottery",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoHourLottery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SixHourLottery",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SixHourLottery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwelveHourLottery",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwelveHourLottery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyLottery",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyLottery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyLottery",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyLottery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LottoTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: DateTime.UtcNow),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LottoTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Winners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    AmountPulse = table.Column<long>(type: "bigint", nullable: false),
                    LottoType = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Winners", x => x.Id);
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "OneHourLottery");
            migrationBuilder.DropTable(name: "TwoHourLottery");
            migrationBuilder.DropTable(name: "SixHourLottery");
            migrationBuilder.DropTable(name: "TwelveHourLottery");
            migrationBuilder.DropTable(name: "DailyLottery");
            migrationBuilder.DropTable(name: "WeeklyLottery");
            migrationBuilder.DropTable(name: "LottoTimes");
            migrationBuilder.DropTable(name: "Winners");
        }
    }
}
