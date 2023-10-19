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

    }
}
