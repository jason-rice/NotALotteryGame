﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nethereum.Web3;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using webapi.Data;
using webapi.Models;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Model;

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

        [HttpGet]
        public async Task<IActionResult> GetWinnerLists()
        {
            var winners = new List<List<Winners>>
                {
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.OneHour).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.TwoHour).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.SixHour).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.TwelveHour).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.Daily).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                    await dbContext.Winners.Where(x => x.LottoType == LottoTypes.Weekly).OrderByDescending(x => x.DateAndTime).Take(5).ToListAsync(),
                };

            return Ok(winners);
        }

        [HttpPost]
        public async Task<IActionResult> GetTicketsBought(TicketOrder ticket)
        {
            int[] tickets = new int[6];

            if (!string.IsNullOrWhiteSpace(ticket.AccountNum))
            {
                tickets[0] = await dbContext.OneHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[1] = await dbContext.TwoHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[2] = await dbContext.SixHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[3] = await dbContext.TwelveHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[4] = await dbContext.DailyLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                tickets[5] = await dbContext.WeeklyLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
            }

            return Ok(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalTicketsBought()
        {
            int[] tickets = new int[6];

            tickets[0] = await dbContext.OneHourLottery.CountAsync();
            tickets[1] = await dbContext.TwoHourLottery.CountAsync();
            tickets[2] = await dbContext.SixHourLottery.CountAsync();
            tickets[3] = await dbContext.TwelveHourLottery.CountAsync();
            tickets[4] = await dbContext.DailyLottery.CountAsync();
            tickets[5] = await dbContext.WeeklyLottery.CountAsync();

            return Ok(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalPlsList()
        {
            long[] totalPls = new long[6];

            totalPls[0] = await dbContext.OneHourLottery.CountAsync() * 18000;
            totalPls[1] = await dbContext.TwoHourLottery.CountAsync() * 18000;
            totalPls[2] = await dbContext.SixHourLottery.CountAsync() * 18000;
            totalPls[3] = await dbContext.TwelveHourLottery.CountAsync() * 18000;
            totalPls[4] = await dbContext.DailyLottery.CountAsync() * 18000;
            totalPls[5] = await dbContext.WeeklyLottery.CountAsync() * 18000;

            return Ok(totalPls);
        }

        [HttpPost]
        public async Task<IActionResult> GetWinnings(TicketOrder ticket)
        {
            if (!string.IsNullOrWhiteSpace(ticket.AccountNum))
            {
                return Ok(await LottoModel.GetWinnings(dbContext, ticket.AccountNum));
            }

            return Ok(0);
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
