using Microsoft.EntityFrameworkCore;
using webapi.Data;

namespace webapi.Models
{
    public class LottoModel
    {
        public static void RunLotto(NotALotteryGameAPIDbContext dbContext, string winnerAddress, int type, int numTickets)
        {
            long total = numTickets * 18000;
            var winner = new Winners()
            {
                Id = Guid.NewGuid(),
                AddressId = winnerAddress,
                AmountPulse = Math.Abs(total),
                LottoType = type,
                DateAndTime = DateTime.Now,
            };
            dbContext.Winners.Add(winner);

            var endTimeToReset = dbContext.LottoTimes.Where(x => x.Id == type).FirstOrDefault();
            if (endTimeToReset != null)
            {
                endTimeToReset.DateAndTime = ResetTimerSwitch(endTimeToReset.DateAndTime, type);
            }

            switch (type)
            {
                case LottoTypes.OneHour:
                    dbContext.OneHourLottery.ExecuteDelete();
                    break;
                case LottoTypes.TwoHour:
                    dbContext.TwoHourLottery.ExecuteDelete();
                    break;
                case LottoTypes.SixHour:
                    dbContext.SixHourLottery.ExecuteDelete();
                    break;
                case LottoTypes.TwelveHour:
                    dbContext.TwelveHourLottery.ExecuteDelete();
                    break;
                case LottoTypes.Daily:
                    dbContext.DailyLottery.ExecuteDelete();
                    break;
                case LottoTypes.Weekly:
                    dbContext.WeeklyLottery.ExecuteDelete();
                    break;
            }

            dbContext.SaveChanges();
        }

        //public static async Task<int> RunLotto(NotALotteryGameAPIDbContext dbContext, string winnerAddress, int type, int numTickets)
        //{
        //    long total = numTickets * 18000;
        //    var winner = new Winners()
        //    {
        //        Id = Guid.NewGuid(),
        //        AddressId = winnerAddress,
        //        AmountPulse = Math.Abs(total),
        //        LottoType = type,
        //        DateAndTime = DateTime.Now,
        //    };
        //    await dbContext.Winners.AddAsync(winner);


        //    var endTimeToReset = await dbContext.LottoTimes.Where(x => x.Id == type).FirstOrDefaultAsync();
        //    if (endTimeToReset != null)
        //    {
        //        endTimeToReset.DateAndTime = ResetTimerSwitch(endTimeToReset.DateAndTime, type);
        //    }

        //    //await dbContext.SaveChangesAsync();


        //    switch (type)
        //    {
        //        case LottoTypes.OneHour:
        //            dbContext.OneHourLottery.ExecuteDelete();
        //            break;
        //        case LottoTypes.TwoHour:
        //            dbContext.TwoHourLottery.ExecuteDelete();
        //            break;
        //        case LottoTypes.SixHour:
        //            dbContext.SixHourLottery.ExecuteDelete();
        //            break;
        //        case LottoTypes.TwelveHour:
        //            dbContext.TwelveHourLottery.ExecuteDelete();
        //            break;
        //        case LottoTypes.Daily:
        //            dbContext.DailyLottery.ExecuteDelete();
        //            break;
        //        case LottoTypes.Weekly:
        //            dbContext.WeeklyLottery.ExecuteDelete();
        //            break;
        //    }

        //    dbContext.SaveChanges();

        //    return 1;
        //}

        public static async void SaveWinner(NotALotteryGameAPIDbContext dbContext, string winnerAddress, int type, int numTickets)
        {
            long total = numTickets * 18000;
            var winner = new Winners()
            {
                Id = Guid.NewGuid(),
                AddressId = winnerAddress,
                AmountPulse = Math.Abs(total),
                LottoType = type,
                DateAndTime = DateTime.Now,
            };

            await dbContext.Winners.AddAsync(winner);
            await dbContext.SaveChangesAsync();

            //return winner;
        }

        public static void ResetTimer(NotALotteryGameAPIDbContext dbContext, int type)
        {
            var endTimeToReset = dbContext.LottoTimes.Where(x => x.Id == type).FirstOrDefault();

            if (endTimeToReset != null)
            {
                endTimeToReset.DateAndTime = ResetTimerSwitch(endTimeToReset.DateAndTime, type);
                dbContext.SaveChanges();
            }
        }

        public static DateTime ResetTimerSwitch(DateTime dt, int type)
        {
            switch (type)
            {
                case LottoTypes.OneHour:
                    if (dt < DateTime.Now)
                    {
                        dt = dt.AddMinutes(2);
                    }
                    break;
                case LottoTypes.TwoHour:
                    if (dt < DateTime.Now)
                    {
                        dt = dt.AddHours(2);
                    }
                    break;
                case LottoTypes.SixHour:
                    if (dt < DateTime.Now)
                    {
                        dt = dt.AddHours(6);
                    }
                    break;
                case LottoTypes.TwelveHour:
                    if (dt < DateTime.Now)
                    {
                        dt = dt.AddHours(12);
                    }
                    break;
                case LottoTypes.Daily:
                    if (dt < DateTime.Now)
                    {
                        dt = dt.AddDays(1);
                    }
                    break;
                case LottoTypes.Weekly:
                    if (dt < DateTime.Now)
                    {
                        dt = dt.AddDays(7);
                    }
                    break;
            }
            return dt;
        }

        public static void DeleteTickets(NotALotteryGameAPIDbContext dbContext, int type)
        {
            switch (type)
            {
                case LottoTypes.OneHour:
                    dbContext.OneHourLottery.ExecuteDelete();
                    break;
                case LottoTypes.TwoHour:
                    dbContext.TwoHourLottery.ExecuteDelete();
                    break;
                case LottoTypes.SixHour:
                    dbContext.SixHourLottery.ExecuteDelete();
                    break;
                case LottoTypes.TwelveHour:
                    dbContext.TwelveHourLottery.ExecuteDelete();
                    break;
                case LottoTypes.Daily:
                    dbContext.DailyLottery.ExecuteDelete();
                    break;
                case LottoTypes.Weekly:
                    dbContext.WeeklyLottery.ExecuteDelete();
                    break;
            }

            dbContext.SaveChanges();
        }

        //public static async Task<Winners> SaveWinner(NotALotteryGameAPIDbContext dbContext, string winnerAddress, int type, int numTickets)
        //{
        //    long total = numTickets * 18000;
        //    var winner = new Winners()
        //    {
        //        Id = Guid.NewGuid(),
        //        AddressId = winnerAddress,
        //        AmountPulse = Math.Abs(total),
        //        LottoType = type,
        //        DateAndTime = DateTime.Now,
        //    };

        //    await dbContext.Winners.AddAsync(winner);
        //    await dbContext.SaveChangesAsync();

        //    return winner;
        //}

        //public static async Task<DateTime> ResetTimer(NotALotteryGameAPIDbContext dbContext, int type)
        //{
        //    var endTimeToReset = await dbContext.LottoTimes.Where(x => x.Id == type).FirstOrDefaultAsync();

        //    switch (type)
        //    {
        //        case LottoTypes.OneHour:
        //            endTimeToReset.DateAndTime = DateTime.Now.AddMinutes(1);
        //            break;
        //        case LottoTypes.TwoHour:
        //            endTimeToReset.DateAndTime = DateTime.Now.AddHours(2);
        //            break;
        //        case LottoTypes.SixHour:
        //            endTimeToReset.DateAndTime = DateTime.Now.AddHours(6);
        //            break;
        //        case LottoTypes.TwelveHour:
        //            endTimeToReset.DateAndTime = DateTime.Now.AddHours(12);
        //            break;
        //        case LottoTypes.Daily:
        //            endTimeToReset.DateAndTime = DateTime.Now.AddDays(1);
        //            break;
        //        case LottoTypes.Weekly:
        //            endTimeToReset.DateAndTime = DateTime.Now.AddDays(7);
        //            break;
        //    }

        //    await dbContext.SaveChangesAsync();
        //    return endTimeToReset.DateAndTime;
        //}

    }
}
