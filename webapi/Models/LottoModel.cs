using Microsoft.EntityFrameworkCore;
using Nethereum.Model;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using webapi.Data;

namespace webapi.Models
{
    public class LottoModel
    {
        public static async Task<int> SendPulseToWinners(NotALotteryGameAPIDbContext dbContext, string addressId)
        {
            var winners = await dbContext.Winners
                .Where(x => x.AddressId == addressId && x.TransactionId == null)
                .ToListAsync();

            var dailyPowerballWinners = await dbContext.DailyPowerballWinners
                .Where(x => x.AddressId == addressId && x.TransactionId == null)
                .ToListAsync();

            long amount = 0;

            if (winners != null && winners.Count > 0)
            {
                foreach (var win in winners)
                {
                    win.TransactionId = "true";
                }
                await dbContext.SaveChangesAsync();

                amount = winners.Sum(x => x.AmountPulse);
            }

            if (dailyPowerballWinners != null && dailyPowerballWinners.Count > 0)
            {
                foreach (var win in dailyPowerballWinners)
                {
                    win.TransactionId = "true";
                }
                await dbContext.SaveChangesAsync();

                amount = dailyPowerballWinners.Sum(x => x.AmountPulse);
            }

            if (amount > 0)
            {
                var url = "https://rpc.pulsechain.com";
                var privateKey = dbContext.MyKeys.Where(x => x.Id == 1).Select(x => x.KeyString).FirstOrDefault();

                var account = new Nethereum.Web3.Accounts.Account(privateKey);
                var web3 = new Web3(account, url);

                var transaction = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(addressId, amount);

                if (winners != null && winners.Count > 0)
                {
                    foreach (var win in winners)
                    {
                        win.TransactionId = transaction.TransactionHash;
                    }
                }

                if (dailyPowerballWinners != null && dailyPowerballWinners.Count > 0)
                {
                    foreach (var win in dailyPowerballWinners)
                    {
                        win.TransactionId = transaction.TransactionHash;
                    }
                }

                await dbContext.SaveChangesAsync();
            }

            return 0;
        }

        public static async Task<int> BuyLottoTickets(NotALotteryGameAPIDbContext dbContext, TicketOrder ticket, long prizeMultiplier)
        {
            if (ticket.TicketNum != null && ticket.TicketNum > 0 && !string.IsNullOrWhiteSpace(ticket.AccountNum) && ticket.Type > 0)
            {

                var stats = await dbContext.Statistics.Where(x => x.Id == 1).FirstOrDefaultAsync();
                if (stats != null)
                {
                    stats.TotalNumberPlayers += ticket.TicketNum;
                    stats.TotalPrizeMoney += ticket.TicketNum * prizeMultiplier;
                }

                switch (ticket.Type)
                {
                    case 1:
                        if (stats != null)
                            stats.TwoMinutePrizeMoney += ticket.TicketNum * prizeMultiplier;

                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new TwoMinuteLottery()
                            {
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.TwoMinuteLottery.AddAsync(t);
                        }
                        break;
                    case 2:
                        if (stats != null)
                            stats.FiveMinutePrizeMoney += ticket.TicketNum * prizeMultiplier;

                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new FiveMinuteLottery()
                            {
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.FiveMinuteLottery.AddAsync(t);
                        }
                        break;
                    case 3:
                        if (stats != null)
                            stats.ThirtyMinutePrizeMoney += ticket.TicketNum * prizeMultiplier;

                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new ThirtyMinuteLottery()
                            {
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.ThirtyMinuteLottery.AddAsync(t);
                        }
                        break;
                    case 4:
                        if (stats != null)
                            stats.OneHourPrizeMoney += ticket.TicketNum * prizeMultiplier;

                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new OneHourLottery()
                            {
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.OneHourLottery.AddAsync(t);
                        }
                        break;
                    case 5:
                        if (stats != null)
                            stats.TwoHourPrizeMoney += ticket.TicketNum * prizeMultiplier;

                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new TwoHourLottery()
                            {
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.TwoHourLottery.AddAsync(t);
                        }
                        break;
                    case 6:
                        if (stats != null)
                            stats.SixHourPrizeMoney += ticket.TicketNum * prizeMultiplier;

                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new SixHourLottery()
                            {
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.SixHourLottery.AddAsync(t);
                        }
                        break;
                    case 7:
                        if (stats != null)
                            stats.TwelveHourPrizeMoney += ticket.TicketNum * prizeMultiplier;

                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new TwelveHourLottery()
                            {
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.TwelveHourLottery.AddAsync(t);
                        }
                        break;
                    case 8:
                        if (stats != null)
                            stats.DailyPrizeMoney += ticket.TicketNum * prizeMultiplier;

                        for (int i = 0; i < ticket.TicketNum; i++)
                        {
                            var t = new DailyLottery()
                            {
                                AddressId = ticket.AccountNum,
                            };
                            await dbContext.DailyLottery.AddAsync(t);
                        }
                        break;
                    case 9:
                        if (stats != null)
                            stats.WeeklyPrizeMoney += ticket.TicketNum * prizeMultiplier;

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
                        return await dbContext.TwoMinuteLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                    case 2:
                        return await dbContext.FiveMinuteLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                    case 3:
                        return await dbContext.ThirtyMinuteLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                    case 4:
                        return await dbContext.OneHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                    case 5:
                        return await dbContext.TwoHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                    case 6:
                        return await dbContext.SixHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                    case 7:
                        return await dbContext.TwelveHourLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                    case 8:
                        return await dbContext.DailyLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                    case 9:
                        return await dbContext.WeeklyLottery.Where(x => x.AddressId == ticket.AccountNum).CountAsync();
                }
                return 0;
            }

            return 0;
        }

        public static async Task<List<DailyPowerball>> BuyPowerballTicket(NotALotteryGameAPIDbContext dbContext, TicketOrder ticket, long prizeMultiplier)
        {
            if (ticket.NumOne != null && ticket.NumOne > 0 && ticket.NumTwo != null && ticket.NumTwo > 0 && ticket.NumThree != null && ticket.NumThree > 0 && !string.IsNullOrWhiteSpace(ticket.AccountNum))
            {
                var stats = await dbContext.Statistics.Where(x => x.Id == 1).FirstOrDefaultAsync();
                //if (stats != null)
                //{
                //    stats.TotalNumberPlayers += ticket.TicketNum;
                //    stats.TotalPrizeMoney += ticket.TicketNum * prizeMultiplier;
                //}

                //if (stats != null)
                //    stats.DailyPrizeMoney += ticket.TicketNum * prizeMultiplier;

                var dailyPowerball = new DailyPowerball()
                {
                    AddressId = ticket.AccountNum,
                    NumOne = (int)ticket.NumOne,
                    NumTwo = (int)ticket.NumTwo,
                    NumThree = (int)ticket.NumThree,
                };

                await dbContext.DailyPowerball.AddAsync(dailyPowerball);

                await dbContext.SaveChangesAsync();

                return await dbContext.DailyPowerball.Where(x => x.AddressId == ticket.AccountNum).ToListAsync();
            }

            return null;
        }

    }
}
