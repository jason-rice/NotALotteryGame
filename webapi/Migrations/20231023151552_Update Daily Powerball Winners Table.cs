using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDailyPowerballWinnersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AmountPulse",
                table: "DailyPowerballWinners",
                type: "bigint",
                nullable: false);
            migrationBuilder.AddColumn<DateTime>(
                name: "DateAndTime",
                table: "DailyPowerballWinners",
                type: "datetime",
                nullable: false);
            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "DailyPowerballWinners",
                type: "nvarchar(100)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPulse",
                table: "DailyPowerballWinners");
            migrationBuilder.DropColumn(
                name: "DateAndTime",
                table: "DailyPowerballWinners");
            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "DailyPowerballWinners");
        }
    }
}
