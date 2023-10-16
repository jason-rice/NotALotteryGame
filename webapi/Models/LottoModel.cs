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
        public static async Task<long> GetWinnings(NotALotteryGameAPIDbContext dbContext, string addressId)
        {
            return await dbContext.Winners
                    .Where(x => x.AddressId == addressId && x.TransactionId == null)
                    .SumAsync(x => x.AmountPulse);
        }

        public static async Task<int> SendPulseToWinners(NotALotteryGameAPIDbContext dbContext, string addressId)
        {
            var winners = await dbContext.Winners
                .Where(x => x.AddressId == addressId && x.TransactionId == null)
                .ToListAsync();

            if (winners != null && winners.Count > 0)
            {
                long amount = winners.Sum(x => x.AmountPulse);

                //var url = "https://rpc.v4.testnet.pulsechain.com";
                var url = "https://rpc.pulsechain.com";
                //var privateKey = "6a73ef66f7c10141d8c171616cc07df0741c7f325748e63af7bbedeec0c7fd38"; // address 3 account 1
                var privateKey = "45b3870d4d893a9842af06b453d6400849ee9354a00e7462b68cd5d7054fed1d"; // address 3 account 2

                var account = new Account(privateKey);//, chainId);
                var web3 = new Web3(account, url);

                var transaction = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(addressId, amount);
                //web3.Eth.GetEtherTransferService().TransferEtherAsync(addressId, amount);

                //await SaveTransactionId(dbContext, winners, transaction);
                foreach (var win in winners)
                {
                    win.TransactionId = transaction.TransactionHash;
                }
                await dbContext.SaveChangesAsync();
            }
            return 0;
        }

        //public static async Task<int> SaveTransactionId(NotALotteryGameAPIDbContext dbContext, List<Winners> winners, TransactionReceipt transaction)
        //{
        //    foreach (var win in winners)
        //    {
        //        win.TransactionId = transaction.TransactionHash;
        //    }
        //    await dbContext.SaveChangesAsync();
        //    return 0;
        //}

        //public static async void DeleteWinners(NotALotteryGameAPIDbContext dbContext, string addressId)
        //{
        //    var list = await dbContext.Winners.Where(x => x.AddressId == addressId).ToListAsync();
        //    dbContext.Winners.RemoveRange(list);
        //    await dbContext.SaveChangesAsync();
        //}

        //public static async Task<long> SendPulseToWinners(NotALotteryGameAPIDbContext dbContext, string addressId, long amount)
        //{
        //    var url = "https://rpc.v4.testnet.pulsechain.com";
        //    BigInteger chainId = 943;
        //    var privateKey = "6a73ef66f7c10141d8c171616cc07df0741c7f325748e63af7bbedeec0c7fd38"; // address 3 account 1
        //    //var privateKey = "45b3870d4d893a9842af06b453d6400849ee9354a00e7462b68cd5d7054fed1d"; // address 3 account 2

        //    var account = new Account(privateKey, chainId);
        //    var web3 = new Web3(account, url);
        //    //var web3 = new Web3("https://mainnet.infura.io/v3/ae64e7ad3f09450d86afa1ae9873cdd3"); // ethereum


        //    //var balance = await web3.Eth.GetBalance.SendRequestAsync(ticket.AccountNum);
        //    //var etherAmount = Web3.Convert.FromWei(balance.Value);


        //    var transaction = await web3.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(addressId, amount);
        //    //await web3.Eth.GetEtherTransferService().TransferEtherAsync(addressId, amount);

        //    return await dbContext.Winners.Where(x => x.AddressId == addressId).SumAsync(x => x.AmountPulse);
        //}


    }
}
