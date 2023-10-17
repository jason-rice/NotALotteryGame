using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Addstatstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint IDENTITY", nullable: false),
                    TotalPrizeMoney = table.Column<long>(type: "bigint", nullable: true),
                    TotalNumberPlayers = table.Column<long>(type: "bigint", nullable: true),
                    TwoMinutePrizeMoney = table.Column<long>(type: "bigint", nullable: true),
                    FiveMinutePrizeMoney = table.Column<long>(type: "bigint", nullable: true),
                    ThirtyMinutePrizeMoney = table.Column<long>(type: "bigint", nullable: true),
                    OneHourPrizeMoney = table.Column<long>(type: "bigint", nullable: true),
                    TwoHourPrizeMoney = table.Column<long>(type: "bigint", nullable: true),
                    SixHourPrizeMoney = table.Column<long>(type: "bigint", nullable: true),
                    TwelveHourPrizeMoney = table.Column<long>(type: "bigint", nullable: true),
                    DailyPrizeMoney = table.Column<long>(type: "bigint", nullable: true),
                    WeeklyPrizeMoney = table.Column<long>(type: "bigint", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statistics");

        }
    }
}
