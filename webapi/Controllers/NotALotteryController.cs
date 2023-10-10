using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class NotALotteryController : ControllerBase
    {
        private readonly NotALotteryGameAPIDbContext dbContext;

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
                    DateAndTime = now.AddHours(1),
                };
                await dbContext.LottoTimes.AddAsync(t1);
                var t2 = new LottoTimes()
                {
                    Id = 2,
                    DateAndTime = now.AddHours(2),
                };
                await dbContext.LottoTimes.AddAsync(t2);
                var t3 = new LottoTimes()
                {
                    Id = 3,
                    DateAndTime = now.AddHours(6),
                };
                await dbContext.LottoTimes.AddAsync(t3);
                var t4 = new LottoTimes()
                {
                    Id = 4,
                    DateAndTime = now.AddHours(12),
                };
                await dbContext.LottoTimes.AddAsync(t4);
                var t5 = new LottoTimes()
                {
                    Id = 5,
                    DateAndTime = now.AddDays(1),
                };
                await dbContext.LottoTimes.AddAsync(t5);
                var t6 = new LottoTimes()
                {
                    Id = 6,
                    DateAndTime = now.AddDays(7),
                };
                await dbContext.LottoTimes.AddAsync(t6);

                await dbContext.SaveChangesAsync();

                t = await dbContext.LottoTimes.ToListAsync();

                return Ok(t);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetTicketsBought(TicketOrder ticket)
        {
            if (!string.IsNullOrWhiteSpace(ticket.AccountNum))
            {
                int[] tickets = new int[6];

                tickets[0] = await dbContext.OneHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[1] = await dbContext.TwoHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[2] = await dbContext.SixHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[3] = await dbContext.TwelveHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[4] = await dbContext.DailyLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[5] = await dbContext.WeeklyLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();

                return Ok(tickets);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> GetWinnings(TicketOrder ticket)
        {
            if (!string.IsNullOrWhiteSpace(ticket.AccountNum))
            {
                return Ok(await dbContext.Winners.Where(x => x.AddressId == ticket.AccountNum).SumAsync(x => x.AmountPulse));
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> GetWinnerLists(TicketOrder ticket)
        {
            if (!string.IsNullOrWhiteSpace(ticket.AccountNum))
            {
                var winners = new List<List<Winners>>
                {
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.OneHour).OrderBy(x => x.DateAndTime).Take(5).ToListAsync(),
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.TwoHour).OrderBy(x => x.DateAndTime).Take(5).ToListAsync(),
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.SixHour).OrderBy(x => x.DateAndTime).Take(5).ToListAsync(),
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.TwelveHour).OrderBy(x => x.DateAndTime).Take(5).ToListAsync(),
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.Daily).OrderBy(x => x.DateAndTime).Take(5).ToListAsync(),
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.Weekly).OrderBy(x => x.DateAndTime).Take(5).ToListAsync(),
                };

                return Ok(winners);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ClaimWinnings(TicketOrder ticket)
        {
            if (!string.IsNullOrWhiteSpace(ticket.AccountNum))
            {
                //var list = await dbContext.Winners.Where(x => x.AddressId == ticket.AccountNum).ToListAsync();
                //dbContext.Winners.RemoveRange(list);
                //await dbContext.SaveChangesAsync();
                return Ok(0);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> BuyTickets(TicketOrder ticket)
        {
            if (ticket.TicketNum != null &&ticket.TicketNum > 0 && !string.IsNullOrWhiteSpace(ticket.AccountNum) && ticket.Type > 0)
            {
                switch (ticket.Type)
                {
                    case 1:
                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new OneHourLottery()
                            {
                                //Id = Guid.NewGuid(),
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.OneHourLottery.AddAsync(t);
                        }
                        break;
                    case 2:
                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new TwoHourLottery()
                            {
                                //Id = Guid.NewGuid(),
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.TwoHourLottery.AddAsync(t);
                        }
                        break;
                    case 3:
                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new SixHourLottery()
                            {
                                //Id = Guid.NewGuid(),
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.SixHourLottery.AddAsync(t);
                        }
                        break;
                    case 4:
                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new TwelveHourLottery()
                            {
                                //Id = Guid.NewGuid(),
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.TwelveHourLottery.AddAsync(t);
                        }
                        break;
                    case 5:
                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new DailyLottery()
                            {
                                //Id = Guid.NewGuid(),
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.DailyLottery.AddAsync(t);
                        }
                        break;
                    case 6:
                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new WeeklyLottery()
                            {
                                //Id = Guid.NewGuid(),
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.WeeklyLottery.AddAsync(t);
                        }
                        break;
                }

                await dbContext.SaveChangesAsync();

                switch (ticket.Type)
                {
                    case 1:
                        return Ok(await dbContext.OneHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync());
                    case 2:
                        return Ok(await dbContext.TwoHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync());
                    case 3:
                        return Ok(await dbContext.SixHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync());
                    case 4:
                        return Ok(await dbContext.TwelveHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync());
                    case 5:
                        return Ok(await dbContext.DailyLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync());
                    case 6:
                        return Ok(await dbContext.WeeklyLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync());
                }
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> RunLotto(RunLotto lotto)
        {
            Random rnd = new Random();
            switch (lotto.Type)
            {
                case LottoTypes.OneHour:
                    var numTickets = await dbContext.OneHourLottery.CountAsync();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = await dbContext.OneHourLottery.ToArrayAsync();
                        if (list != null && list.Length > 0)
                        {
                            //var o = await LottoModel.RunLotto(dbContext, list[winnerInt].AddressId, LottoTypes.OneHour, numTickets);
                            //LottoModel.SaveWinner(dbContext, list[winnerInt].AddressId, LottoTypes.OneHour, numTickets);
                            //LottoModel.ResetTimer(dbContext, LottoTypes.OneHour);
                            //var winner = await LottoModel.SaveWinner(dbContext, list[winnerInt].AddressId, LottoTypes.OneHour, numTickets);
                            //var output = new RunLotto()
                            //{
                            //    Winner = winner,
                            //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.OneHour),
                            //};
                            //LottoModel.DeleteTickets(dbContext, LottoTypes.OneHour);
                            return Ok(await LottoModel.RunLotto(dbContext, list[winnerInt].AddressId, LottoTypes.OneHour, numTickets));
                        }
                        return NoContent();
                    }
                    else
                    {
                        LottoModel.ResetTimer(dbContext, LottoTypes.OneHour);
                        //var output = new RunLotto()
                        //{
                        //    Winner = null,
                        //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.OneHour),
                        //};
                        return Ok(1);
                    }
                case LottoTypes.TwoHour:
                    numTickets = await dbContext.TwoHourLottery.CountAsync();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = await dbContext.TwoHourLottery.ToArrayAsync();
                        if (list != null && list.Length > 0)
                        {
                            //var winner = await LottoModel.SaveWinner(dbContext, list[winnerInt].AddressId, LottoTypes.TwoHour, numTickets);
                            //var output = new RunLotto()
                            //{
                            //    Winner = winner,
                            //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.TwoHour),
                            //};
                            //LottoModel.DeleteTickets(dbContext, LottoTypes.TwoHour);
                            //return Ok(output);
                            return Ok(await LottoModel.RunLotto(dbContext, list[winnerInt].AddressId, LottoTypes.TwoHour, numTickets));
                        }
                        return NoContent();
                    }
                    else
                    {
                        //var output = new RunLotto()
                        //{
                        //    Winner = null,
                        //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.TwoHour),
                        //};
                        LottoModel.ResetTimer(dbContext, LottoTypes.TwoHour);
                        return Ok(1);
                    }
                case LottoTypes.SixHour:
                    numTickets = await dbContext.SixHourLottery.CountAsync();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = await dbContext.SixHourLottery.ToArrayAsync();
                        if (list != null && list.Length > 0)
                        {
                            //var winner = await LottoModel.SaveWinner(dbContext, list[winnerInt].AddressId, LottoTypes.SixHour, numTickets);
                            //var output = new RunLotto()
                            //{
                            //    Winner = winner,
                            //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.SixHour),
                            //};
                            //LottoModel.DeleteTickets(dbContext, LottoTypes.SixHour);
                            //return Ok(output);
                            return Ok(await LottoModel.RunLotto(dbContext, list[winnerInt].AddressId, LottoTypes.SixHour, numTickets));
                        }
                        return NoContent();
                    }
                    else
                    {
                        //var output = new RunLotto()
                        //{
                        //    Winner = null,
                        //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.SixHour),
                        //};
                        LottoModel.ResetTimer(dbContext, LottoTypes.SixHour);
                        return Ok(1);
                    }
                case LottoTypes.TwelveHour:
                    numTickets = await dbContext.TwelveHourLottery.CountAsync();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = await dbContext.TwelveHourLottery.ToArrayAsync();
                        if (list != null && list.Length > 0)
                        {
                            //var winner = await LottoModel.SaveWinner(dbContext, list[winnerInt].AddressId, LottoTypes.TwelveHour, numTickets);
                            //var output = new RunLotto()
                            //{
                            //    Winner = winner,
                            //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.TwelveHour),
                            //};
                            //LottoModel.DeleteTickets(dbContext, LottoTypes.TwelveHour);
                            //return Ok(output);
                            return Ok(await LottoModel.RunLotto(dbContext, list[winnerInt].AddressId, LottoTypes.TwelveHour, numTickets));
                        }
                        return NoContent();
                    }
                    else
                    {
                        //var output = new RunLotto()
                        //{
                        //    Winner = null,
                        //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.TwelveHour),
                        //};
                        LottoModel.ResetTimer(dbContext, LottoTypes.TwelveHour);
                        return Ok(1);
                    }
                case LottoTypes.Daily:
                    numTickets = await dbContext.DailyLottery.CountAsync();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = await dbContext.DailyLottery.ToArrayAsync();
                        if (list != null && list.Length > 0)
                        {
                            //var winner = await LottoModel.SaveWinner(dbContext, list[winnerInt].AddressId, LottoTypes.Daily, numTickets);
                            //var output = new RunLotto()
                            //{
                            //    Winner = winner,
                            //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.Daily),
                            //};
                            //LottoModel.DeleteTickets(dbContext, LottoTypes.Daily);
                            //return Ok(output);
                            return Ok(await LottoModel.RunLotto(dbContext, list[winnerInt].AddressId, LottoTypes.Daily, numTickets));
                        }
                        return NoContent();
                    }
                    else
                    {
                        //var output = new RunLotto()
                        //{
                        //    Winner = null,
                        //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.Daily),
                        //};
                        LottoModel.ResetTimer(dbContext, LottoTypes.Daily);
                        return Ok(1);
                    }
                case LottoTypes.Weekly:
                    numTickets = await dbContext.WeeklyLottery.CountAsync();
                    if (numTickets > 0)
                    {
                        var winnerInt = rnd.Next(1, numTickets) - 1;
                        var list = await dbContext.WeeklyLottery.ToArrayAsync();
                        if (list != null && list.Length > 0)
                        {
                            //var winner = await LottoModel.SaveWinner(dbContext, list[winnerInt].AddressId, LottoTypes.Weekly, numTickets);
                            //var output = new RunLotto()
                            //{
                            //    Winner = winner,
                            //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.Weekly),
                            //};
                            //LottoModel.DeleteTickets(dbContext, LottoTypes.Weekly);
                            //return Ok(output);
                            return Ok(await LottoModel.RunLotto(dbContext, list[winnerInt].AddressId, LottoTypes.Weekly, numTickets));
                        }
                        return NoContent();
                    }
                    else
                    {
                        //var output = new RunLotto()
                        //{
                        //    Winner = null,
                        //    Date = await LottoModel.ResetTimer(dbContext, LottoTypes.Weekly),
                        //};
                        LottoModel.ResetTimer(dbContext, LottoTypes.Weekly);
                        return Ok(1);
                    }
            }

            return BadRequest();
        }
    }
}
