namespace webapi.Models
{
    public class Winners
    {
        public Guid Id { get; set; }
        public required string AddressId { get; set; }
        public long AmountPulse { get; set; }
        public int LottoType { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
