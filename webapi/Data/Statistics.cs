namespace webapi.Data
{
    public class Statistics
    {
        public long Id { get; set; }
        public long? TotalPrizeMoney { get; set; }
        public long? TotalNumberPlayers { get; set; }
        public long? TwoMinutePrizeMoney { get; set; }
        public long? FiveMinutePrizeMoney { get; set; }
        public long? ThirtyMinutePrizeMoney { get; set; }
        public long? OneHourPrizeMoney { get; set; }
        public long? TwoHourPrizeMoney { get; set; }
        public long? SixHourPrizeMoney { get; set; }
        public long? TwelveHourPrizeMoney { get; set; }
        public long? DailyPrizeMoney { get; set; }
        public long? WeeklyPrizeMoney { get; set; }
    }
}
