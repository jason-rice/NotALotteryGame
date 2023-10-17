using Microsoft.EntityFrameworkCore;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;
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

            if (winners != null && winners.Count > 0)
            {
                foreach (var win in winners)
                {
                    win.TransactionId = "true";
                }
                await dbContext.SaveChangesAsync();

                long amount = winners.Sum(x => x.AmountPulse);

                var url = "https://rpc.pulsechain.com";
                var privateKey = dbContext.MyKeys.Where(x => x.Id == 1).Select(x => x.KeyString).FirstOrDefault();

                var account = new Account(privateKey);
                var web3 = new Web3(account, url);

                var transaction = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(addressId, amount);
                
                foreach (var win in winners)
                {
                    win.TransactionId = transaction.TransactionHash;
                }

                await dbContext.SaveChangesAsync();
            }
            return 0;
        }
    }
}
