using Microsoft.EntityFrameworkCore;
using System;
using webapi.Data;
using webapi.Models;

namespace webapi
{
    public class RepeatingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        //private readonly static int numHours = -5; // production
        private readonly static int numHours = 0; // development
        public RepeatingService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<NotALotteryGameAPIDbContext>();

                    var times = await dbContext.LottoTimes.ToArrayAsync();
                    var now = DateTime.Now.AddHours(numHours);
                    if (times[0].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.OneHour, times[0]);
                    }
                    if (times[1].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.TwoHour, times[1]);
                    }
                    if (times[2].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.SixHour, times[2]);
                    }
                    if (times[3].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.TwelveHour, times[3]);
                    }
                    if (times[4].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.Daily, times[4]);
                    }
                    if (times[5].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.Weekly, times[5]);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }
            }
        }

        private static void RunLotto1(NotALotteryGameAPIDbContext dbContext, int type, LottoTimes time)
        {
            Random rnd = new Random();
            switch (type)
            {
                case LottoTypes.OneHour:
                    var numTickets = dbContext.OneHourLottery.Count();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = dbContext.OneHourLottery.ToArray();
                        if (list != null && list.Length > 0)
                        {
                            RunLotto2(dbContext, list[winnerInt].AddressId, LottoTypes.OneHour, numTickets);
                        }
                    }
                    ResetTimer(dbContext, LottoTypes.OneHour, time);
                    break;
                case LottoTypes.TwoHour:
                    numTickets = dbContext.TwoHourLottery.Count();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = dbContext.TwoHourLottery.ToArray();
                        if (list != null && list.Length > 0)
                        {
                            RunLotto2(dbContext, list[winnerInt].AddressId, LottoTypes.TwoHour, numTickets);
                        }
                    }
                    ResetTimer(dbContext, LottoTypes.TwoHour, time);
                    break;
                case LottoTypes.SixHour:
                    numTickets = dbContext.SixHourLottery.Count();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = dbContext.SixHourLottery.ToArray();
                        if (list != null && list.Length > 0)
                        {
                            RunLotto2(dbContext, list[winnerInt].AddressId, LottoTypes.SixHour, numTickets);
                        }
                    }
                    ResetTimer(dbContext, LottoTypes.SixHour, time);
                    break;
                case LottoTypes.TwelveHour:
                    numTickets = dbContext.TwelveHourLottery.Count();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = dbContext.TwelveHourLottery.ToArray();
                        if (list != null && list.Length > 0)
                        {
                            RunLotto2(dbContext, list[winnerInt].AddressId, LottoTypes.TwelveHour, numTickets);
                        }
                    }
                    ResetTimer(dbContext, LottoTypes.TwelveHour, time);
                    break;
                case LottoTypes.Daily:
                    numTickets = dbContext.DailyLottery.Count();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = dbContext.DailyLottery.ToArray();
                        if (list != null && list.Length > 0)
                        {
                            RunLotto2(dbContext, list[winnerInt].AddressId, LottoTypes.Daily, numTickets);
                        }
                    }
                    ResetTimer(dbContext, LottoTypes.Daily, time);
                    break;
                case LottoTypes.Weekly:
                    numTickets = dbContext.WeeklyLottery.Count();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = dbContext.WeeklyLottery.ToArray();
                        if (list != null && list.Length > 0)
                        {
                            RunLotto2(dbContext, list[winnerInt].AddressId, LottoTypes.Weekly, numTickets);
                        }
                    }
                    ResetTimer(dbContext, LottoTypes.Weekly, time);
                    break;
            }
        }

        private static void RunLotto2(NotALotteryGameAPIDbContext dbContext, string winnerAddress, int type, int numTickets)
        {
            long total = numTickets * 18000;
            var winner = new Winners()
            {
                Id = Guid.NewGuid(),
                AddressId = winnerAddress,
                AmountPulse = Math.Abs(total),
                LottoType = type,
                DateAndTime = DateTime.Now.AddHours(numHours),
            };
            dbContext.Winners.Add(winner);

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

        private static void ResetTimer(NotALotteryGameAPIDbContext dbContext, int type, LottoTimes time)
        {
            time.DateAndTime = ResetTimerSwitch(time.DateAndTime, type);
            dbContext.SaveChanges();
        }

        private static DateTime ResetTimerSwitch(DateTime dt, int type)
        {
            switch (type)
            {
                case LottoTypes.OneHour:
                    if (dt < DateTime.Now.AddHours(numHours))
                    {
                        dt = dt.AddMinutes(2);
                    }
                    break;
                case LottoTypes.TwoHour:
                    if (dt < DateTime.Now.AddHours(numHours))
                    {
                        dt = dt.AddHours(2);
                    }
                    break;
                case LottoTypes.SixHour:
                    if (dt < DateTime.Now.AddHours(numHours))
                    {
                        dt = dt.AddHours(6);
                    }
                    break;
                case LottoTypes.TwelveHour:
                    if (dt < DateTime.Now.AddHours(numHours))
                    {
                        dt = dt.AddHours(12);
                    }
                    break;
                case LottoTypes.Daily:
                    if (dt < DateTime.Now.AddHours(numHours))
                    {
                        dt = dt.AddDays(1);
                    }
                    break;
                case LottoTypes.Weekly:
                    if (dt < DateTime.Now.AddHours(numHours))
                    {
                        dt = dt.AddDays(7);
                    }
                    break;
            }
            return dt;
        }

    }
}
