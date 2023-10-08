namespace webapi.Models
{
    public class DailyLottery
    {
        public Guid Id { get; set; }
        public required string AddressId { get; set; }
    }
}
