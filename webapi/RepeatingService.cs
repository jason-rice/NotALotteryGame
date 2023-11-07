using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi
{
    public class RepeatingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public static long prizeMultiplier = 18000;

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
                        RunLotto1(dbContext, LottoTypes.TwoMinute, times[0]);
                    }
                    if (times[1].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.FiveMinute, times[1]);
                    }
                    if (times[2].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.ThirtyMinute, times[2]);
                    }
                    if (times[3].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.OneHour, times[3]);
                    }
                    if (times[4].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.TwoHour, times[4]);
                    }
                    if (times[5].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.SixHour, times[5]);
                    }
                    if (times[6].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.TwelveHour, times[6]);
                    }
                    if (times[7].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.Daily, times[7]);
                    }
                    if (times[8].DateAndTime < now)
                    {
                        RunLotto1(dbContext, LottoTypes.Weekly, times[8]);
                    }
                    if (times[9].DateAndTime < now)
                    {
                        RunPowerball(dbContext, LottoTypes.DailyPowerball, times[9]);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }
        }

        private static void RunLotto1(NotALotteryGameAPIDbContext dbContext, int type, LottoTimes time)
        {
            Random rnd = new Random();
            switch (type)
            {
                case LottoTypes.TwoMinute:
                    var numTickets = dbContext.TwoMinuteLottery.Count();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = dbContext.TwoMinuteLottery.ToArray();
                        if (list != null && list.Length > 0)
                        {
                            RunLotto2(dbContext, list[winnerInt].AddressId, LottoTypes.TwoMinute, numTickets);
                        }
                    }
                    ResetTimer(dbContext, LottoTypes.TwoMinute, time);
                    break;
                case LottoTypes.FiveMinute:
                    numTickets = dbContext.FiveMinuteLottery.Count();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = dbContext.FiveMinuteLottery.ToArray();
                        if (list != null && list.Length > 0)
                        {
                            RunLotto2(dbContext, list[winnerInt].AddressId, LottoTypes.FiveMinute, numTickets);
                        }
                    }
                    ResetTimer(dbContext, LottoTypes.FiveMinute, time);
                    break;
                case LottoTypes.ThirtyMinute:
                    numTickets = dbContext.ThirtyMinuteLottery.Count();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = dbContext.ThirtyMinuteLottery.ToArray();
                        if (list != null && list.Length > 0)
                        {
                            RunLotto2(dbContext, list[winnerInt].AddressId, LottoTypes.ThirtyMinute, numTickets);
                        }
                    }
                    ResetTimer(dbContext, LottoTypes.ThirtyMinute, time);
                    break;
                case LottoTypes.OneHour:
                    numTickets = dbContext.OneHourLottery.Count();
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
                //case LottoTypes.DailyPowerball:
                //    numTickets = dbContext.DailyPowerball.Count();
                //    if (numTickets > 0)
                //    {
                //        //var winnerInt = rnd.Next(1, numTickets) - 1;
                //        var num1 = rnd.Next(1, 99);
                //        var num2 = rnd.Next(1, 99);
                //        var num3 = rnd.Next(1, 99);
                //        List<DailyPowerballWinners>? list = dbContext.DailyPowerballWinners
                //            .Where(x => x.NumTwo == num1 && x.NumTwo == num2 && x.NumThree == num3)
                //            .ToList();
                //        if (list != null && list.Count > 0)
                //        {
                //            RunPowerball(dbContext, LottoTypes.DailyPowerball, numTickets, num1, num2, num3, list);
                //        }
                //    }
                //    ResetTimer(dbContext, LottoTypes.Weekly, time);
                //    break;
            }
        }

        private static void RunLotto2(NotALotteryGameAPIDbContext dbContext, string winnerAddress, int type, int numTickets)
        {
            long total = numTickets * prizeMultiplier;
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
                case LottoTypes.TwoMinute:
                    dbContext.TwoMinuteLottery.ExecuteDelete();
                    break;
                case LottoTypes.FiveMinute:
                    dbContext.FiveMinuteLottery.ExecuteDelete();
                    break;
                case LottoTypes.ThirtyMinute:
                    dbContext.ThirtyMinuteLottery.ExecuteDelete();
                    break;
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

        private static void RunPowerball(NotALotteryGameAPIDbContext dbContext, int type, LottoTimes time)
        {
            Random rnd = new Random();
            var numTickets = dbContext.DailyPowerball.Count();

            if (numTickets > 0)
            {
                var num1 = rnd.Next(1, 99);
                var num2 = rnd.Next(1, 99);
                var num3 = rnd.Next(1, 99);

                var winners = dbContext.DailyPowerball
                    .Where(x => x.NumTwo == num1 && x.NumTwo == num2 && x.NumThree == num3)
                    .ToList();

                var totalPls1 = dbContext.DailyPowerballWinners.Where(x => x.AddressId == null).Select(x => x.AmountPulse).Sum();
                var totalPls2 = dbContext.DailyPowerball.Count() * prizeMultiplier;
                long totalPls = totalPls1 + totalPls2;

                if (winners != null && winners.Count > 0)
                {
                    long eachWinnerTotal = totalPls / winners.Count;

                    foreach (var winner in winners)
                    {
                        var w = new DailyPowerballWinners()
                        {
                            AddressId = winner.AddressId,
                            AmountPulse = Math.Abs(eachWinnerTotal),
                            DateAndTime = DateTime.Now.AddHours(numHours),
                            NumOne = num1,
                            NumTwo = num2,
                            NumThree = num3,
                        };
                        dbContext.DailyPowerballWinners.Add(w);
                    }
                    dbContext.SaveChanges();
                    var winnersToDelete = dbContext.DailyPowerballWinners.Where(x => x.AddressId == null).ToList();
                    dbContext.DailyPowerballWinners.RemoveRange(winnersToDelete);
                }
                else
                {
                    var w = new DailyPowerballWinners()
                    {
                        AddressId = null,
                        AmountPulse = Math.Abs(totalPls),
                        DateAndTime = DateTime.Now.AddHours(numHours),
                        NumOne = num1,
                        NumTwo = num2,
                        NumThree = num3,
                    };
                    dbContext.DailyPowerballWinners.Add(w);
                }
            }
            else
            {
                var num1 = rnd.Next(1, 99);
                var num2 = rnd.Next(1, 99);
                var num3 = rnd.Next(1, 99);

                var w = new DailyPowerballWinners()
                {
                    AddressId = null,
                    AmountPulse = 0,
                    DateAndTime = DateTime.Now.AddHours(numHours),
                    NumOne = num1,
                    NumTwo = num2,
                    NumThree = num3,
                };
                dbContext.DailyPowerballWinners.Add(w);
            }

            dbContext.DailyPowerball.ExecuteDelete();
            dbContext.SaveChanges();

            ResetTimer(dbContext, LottoTypes.DailyPowerball, time);
        }

        private static void ResetTimer(NotALotteryGameAPIDbContext dbContext, int type, LottoTimes time)
        {
            switch (type)
            {
                case LottoTypes.TwoMinute:
                    if (time.DateAndTime < DateTime.Now.AddHours(numHours))
                    {
                        time.DateAndTime = DateTime.Now.AddHours(numHours).AddMinutes(2);
                    }
                    break;
                case LottoTypes.FiveMinute:
                    if (time.DateAndTime < DateTime.Now.AddHours(numHours))
                    {
                        time.DateAndTime = DateTime.Now.AddHours(numHours).AddMinutes(5);
                    }
                    break;
                case LottoTypes.ThirtyMinute:
                    if (time.DateAndTime < DateTime.Now.AddHours(numHours))
                    {
                        time.DateAndTime = DateTime.Now.AddHours(numHours).AddMinutes(30);
                    }
                    break;
                case LottoTypes.OneHour:
                    if (time.DateAndTime < DateTime.Now.AddHours(numHours))
                    {
                        time.DateAndTime = DateTime.Now.AddHours(numHours).AddHours(1);
                    }
                    break;
                case LottoTypes.TwoHour:
                    if (time.DateAndTime < DateTime.Now.AddHours(numHours))
                    {
                        time.DateAndTime = DateTime.Now.AddHours(numHours).AddHours(2);
                    }
                    break;
                case LottoTypes.SixHour:
                    if (time.DateAndTime < DateTime.Now.AddHours(numHours))
                    {
                        time.DateAndTime = DateTime.Now.AddHours(numHours).AddHours(6);
                    }
                    break;
                case LottoTypes.TwelveHour:
                    if (time.DateAndTime < DateTime.Now.AddHours(numHours))
                    {
                        time.DateAndTime = DateTime.Now.AddHours(numHours).AddHours(12);
                    }
                    break;
                case LottoTypes.Daily:
                    if (time.DateAndTime < DateTime.Now.AddHours(numHours))
                    {
                        time.DateAndTime = DateTime.Now.AddHours(numHours).AddDays(1);
                    }
                    break;
                case LottoTypes.Weekly:
                    if (time.DateAndTime < DateTime.Now.AddHours(numHours))
                    {
                        time.DateAndTime = DateTime.Now.AddHours(numHours).AddDays(7);
                    }
                    break;
                case LottoTypes.DailyPowerball:
                    if (time.DateAndTime < DateTime.Now.AddHours(numHours))
                    {
                        time.DateAndTime = DateTime.Now.AddHours(numHours).AddDays(1);
                    }
                    break;
            }
            dbContext.SaveChanges();
        }

    }
}
