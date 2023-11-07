using Microsoft.EntityFrameworkCore;

namespace webapi.Data
{
    public class NotALotteryGameAPIDbContext : DbContext
    {
        public NotALotteryGameAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        // add a migration to add changes to db.
        // go to Package Manager Console, type Add-Migration "name"
        // next type Update-Database
        public DbSet<TwoMinuteLottery> TwoMinuteLottery { get; set; }
        public DbSet<FiveMinuteLottery> FiveMinuteLottery { get; set; }
        public DbSet<ThirtyMinuteLottery> ThirtyMinuteLottery { get; set; }
        public DbSet<OneHourLottery> OneHourLottery { get; set; }
        public DbSet<TwoHourLottery> TwoHourLottery { get; set; }
        public DbSet<SixHourLottery> SixHourLottery { get; set; }
        public DbSet<TwelveHourLottery> TwelveHourLottery { get; set; }
        public DbSet<DailyLottery> DailyLottery { get; set; }
        public DbSet<WeeklyLottery> WeeklyLottery { get; set; }
        public DbSet<LottoTimes> LottoTimes { get; set; }
        public DbSet<Winners> Winners { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<MyKeys> MyKeys { get; set; }
        public DbSet<DailyPowerball> DailyPowerball { get; set; }
        public DbSet<DailyPowerballWinners> DailyPowerballWinners { get; set; }
    }
}
