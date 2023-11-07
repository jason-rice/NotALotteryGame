using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotALotteryController : ControllerBase
    {
        private readonly NotALotteryGameAPIDbContext dbContext;
        public static long prizeMultiplier = 18000;

        public NotALotteryController(NotALotteryGameAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetLottoTimes()
        {
            var t = await dbContext.LottoTimes.ToListAsync();
            if (t.Count > 0)
            {
                return Ok(t);
            }
            else
            {
                var now = DateTime.Now;
                var t1 = new LottoTimes()
                {
                    Id = 1,
                    DateAndTime = now.AddMinutes(2),
                };
                await dbContext.LottoTimes.AddAsync(t1);
                var t2 = new LottoTimes()
                {
                    Id = 2,
                    DateAndTime = now.AddMinutes(5),
                };
                await dbContext.LottoTimes.AddAsync(t2);
                var t3 = new LottoTimes()
                {
                    Id = 3,
                    DateAndTime = now.AddMinutes(30),
                };
                await dbContext.LottoTimes.AddAsync(t3);
                var t4 = new LottoTimes()
                {
                    Id = 4,
                    DateAndTime = now.AddHours(1),
                };
                await dbContext.LottoTimes.AddAsync(t4);
                var t5 = new LottoTimes()
                {
                    Id = 5,
                    DateAndTime = now.AddHours(2),
                };
                await dbContext.LottoTimes.AddAsync(t5);
                var t6 = new LottoTimes()
                {
                    Id = 6,
                    DateAndTime = now.AddHours(6),
                };
                await dbContext.LottoTimes.AddAsync(t6);
                var t7 = new LottoTimes()
                {
                    Id = 7,
                    DateAndTime = now.AddHours(12),
                };
                await dbContext.LottoTimes.AddAsync(t7);
                var t8 = new LottoTimes()
                {
                    Id = 8,
                    DateAndTime = now.AddDays(1),
                };
                await dbContext.LottoTimes.AddAsync(t8);
                var t9 = new LottoTimes()
                {
                    Id = 9,
                    DateAndTime = now.AddDays(7),
                };
                await dbContext.LottoTimes.AddAsync(t9);
                var t10 = new LottoTimes()
                {
                    Id = 10,
                    DateAndTime = now.AddDays(1),
                };
                await dbContext.LottoTimes.AddAsync(t10);

                await dbContext.SaveChangesAsync();

                t = await dbContext.LottoTimes.ToListAsync();

                return Ok(t);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            return Ok(await dbContext.Statistics.Where(x => x.Id == 1).FirstOrDefaultAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetLottoWinnerLists()
        {
            var winners = new List<List<Winners>>
            {
                await dbContext.Winners.Where(x => x.LottoType == LottoTypes.TwoMinute).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                await dbContext.Winners.Where(x => x.LottoType == LottoTypes.FiveMinute).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                await dbContext.Winners.Where(x => x.LottoType == LottoTypes.ThirtyMinute).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                await dbContext.Winners.Where(x => x.LottoType == LottoTypes.OneHour).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                await dbContext.Winners.Where(x => x.LottoType == LottoTypes.TwoHour).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                await dbContext.Winners.Where(x => x.LottoType == LottoTypes.SixHour).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                await dbContext.Winners.Where(x => x.LottoType == LottoTypes.TwelveHour).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                await dbContext.Winners.Where(x => x.LottoType == LottoTypes.Daily).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                await dbContext.Winners.Where(x => x.LottoType == LottoTypes.Weekly).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
            };

            return Ok(winners);
        }

        [HttpGet]
        public async Task<IActionResult> GetPowerballWinnerLists()
        {
            var winners = new List<List<DailyPowerballWinners>>
            {
                await dbContext.DailyPowerballWinners.Where(x => x.AddressId != null).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
            };

            return Ok(winners);
        }

        [HttpPost]
        public async Task<IActionResult> GetTicketsBought(TicketOrder ticket)
        {
            int[] tickets = new int[10];

            if (!string.IsNullOrWhiteSpace(ticket.AccountNum))
            {
                tickets[0] = await dbContext.TwoMinuteLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[1] = await dbContext.FiveMinuteLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[2] = await dbContext.ThirtyMinuteLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[3] = await dbContext.OneHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[4] = await dbContext.TwoHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[5] = await dbContext.SixHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[6] = await dbContext.TwelveHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[7] = await dbContext.DailyLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[8] = await dbContext.WeeklyLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[9] = await dbContext.DailyPowerball.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
            }

            return Ok(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> GetMyPowerballTicketsBought(TicketOrder ticket)
        {
            return Ok(await dbContext.DailyPowerball.Where(x => x.AddressId == ticket.AccountNum).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalTicketsBought()
        {
            int[] tickets = new int[10];

            tickets[0] = await dbContext.TwoMinuteLottery.CountAsync();
            tickets[1] = await dbContext.FiveMinuteLottery.CountAsync();
            tickets[2] = await dbContext.ThirtyMinuteLottery.CountAsync();
            tickets[3] = await dbContext.OneHourLottery.CountAsync();
            tickets[4] = await dbContext.TwoHourLottery.CountAsync();
            tickets[5] = await dbContext.SixHourLottery.CountAsync();
            tickets[6] = await dbContext.TwelveHourLottery.CountAsync();
            tickets[7] = await dbContext.DailyLottery.CountAsync();
            tickets[8] = await dbContext.WeeklyLottery.CountAsync();
            tickets[9] = await dbContext.DailyPowerball.CountAsync();

            return Ok(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalPlsList()
        {
            long[] totalPls = new long[10];

            totalPls[0] = await dbContext.TwoMinuteLottery.CountAsync() * prizeMultiplier;
            totalPls[1] = await dbContext.FiveMinuteLottery.CountAsync() * prizeMultiplier;
            totalPls[2] = await dbContext.ThirtyMinuteLottery.CountAsync() * prizeMultiplier;
            totalPls[3] = await dbContext.OneHourLottery.CountAsync() * prizeMultiplier;
            totalPls[4] = await dbContext.TwoHourLottery.CountAsync() * prizeMultiplier;
            totalPls[5] = await dbContext.SixHourLottery.CountAsync() * prizeMultiplier;
            totalPls[6] = await dbContext.TwelveHourLottery.CountAsync() * prizeMultiplier;
            totalPls[7] = await dbContext.DailyLottery.CountAsync() * prizeMultiplier;
            totalPls[8] = await dbContext.WeeklyLottery.CountAsync() * prizeMultiplier;
            var num1 = await dbContext.DailyPowerballWinners.Where(x => x.AddressId == null).Select(x => x.AmountPulse).SumAsync();
            var num2 = await dbContext.DailyPowerball.CountAsync() * prizeMultiplier;
            totalPls[9] = num1 + num2;

            return Ok(totalPls);
        }

        [HttpPost]
        public async Task<IActionResult> GetWinnings(TicketOrder ticket)
        {
            if (!string.IsNullOrWhiteSpace(ticket.AccountNum))
            {
                var num1 = await dbContext.Winners.Where(x => x.AddressId == ticket.AccountNum && x.TransactionId == null).SumAsync(x => x.AmountPulse);
                var num2 = await dbContext.DailyPowerballWinners.Where(x => x.AddressId == ticket.AccountNum && x.TransactionId == null).SumAsync(x => x.AmountPulse);
                var total = num1 + num2;
                return Ok(total);
            }

            return Ok(0);
        }

        [HttpGet]
        public async Task<IActionResult> GetEscrowAccountNum()
        {
            return Ok(await dbContext.MyKeys.Where(x => x.Id == 2).FirstOrDefaultAsync());
        }

        [HttpPost]
        public async Task<IActionResult> BuyLottoTickets(TicketOrder ticket)
        {
            return Ok(await LottoModel.BuyLottoTickets(dbContext, ticket, prizeMultiplier));
        }

        [HttpPost]
        public async Task<IActionResult> BuyPowerballTicket(TicketOrder ticket)
        {
            return Ok(await LottoModel.BuyPowerballTicket(dbContext, ticket, prizeMultiplier));
        }

        [HttpPost]
        public async Task<IActionResult> ClaimWinnings(TicketOrder ticket)
        {
            if (!string.IsNullOrWhiteSpace(ticket.AccountNum))
            {
                await LottoModel.SendPulseToWinners(dbContext, ticket.AccountNum);
                return Ok(0);
            }

            return BadRequest();
        }

    }
}
