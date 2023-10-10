using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using webapi.Data;

namespace webapi.Models
{
    public class LottoModel
    {
        public static async Task<int> RunLotto(NotALotteryGameAPIDbContext dbContext, string winnerAddress, int type, int numTickets)
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


            var endTimeToReset = await dbContext.LottoTimes.Where(x => x.Id == type).FirstOrDefaultAsync();
            switch (type)
            {
                case LottoTypes.OneHour:
                    endTimeToReset.DateAndTime = DateTime.Now.AddMinutes(1);
                    break;
                case LottoTypes.TwoHour:
                    endTimeToReset.DateAndTime = DateTime.Now.AddHours(2);
                    break;
                case LottoTypes.SixHour:
                    endTimeToReset.DateAndTime = DateTime.Now.AddHours(6);
                    break;
                case LottoTypes.TwelveHour:
                    endTimeToReset.DateAndTime = DateTime.Now.AddHours(12);
                    break;
                case LottoTypes.Daily:
                    endTimeToReset.DateAndTime = DateTime.Now.AddDays(1);
                    break;
                case LottoTypes.Weekly:
                    endTimeToReset.DateAndTime = DateTime.Now.AddDays(7);
                    break;
            }


            await dbContext.SaveChangesAsync();


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

            return 1;
        }

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

        public static void ResetTimer(NotALotteryGameAPIDbContext dbContext, int type)
        {
            var endTimeToReset = dbContext.LottoTimes.Where(x => x.Id == type).FirstOrDefault();

            if (endTimeToReset != null)
            {
                switch (type)
                {
                    case LottoTypes.OneHour:
                        endTimeToReset.DateAndTime = endTimeToReset.DateAndTime.AddHours(1);
                        break;
                    case LottoTypes.TwoHour:
                        endTimeToReset.DateAndTime = endTimeToReset.DateAndTime.AddHours(2);
                        break;
                    case LottoTypes.SixHour:
                        endTimeToReset.DateAndTime = endTimeToReset.DateAndTime.AddHours(6);
                        break;
                    case LottoTypes.TwelveHour:
                        endTimeToReset.DateAndTime = endTimeToReset.DateAndTime.AddHours(12);
                        break;
                    case LottoTypes.Daily:
                        endTimeToReset.DateAndTime = endTimeToReset.DateAndTime.AddDays(1);
                        break;
                    case LottoTypes.Weekly:
                        endTimeToReset.DateAndTime = endTimeToReset.DateAndTime.AddDays(7);
                        break;
                }
                dbContext.SaveChanges();
            }
        }

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

    }
}
