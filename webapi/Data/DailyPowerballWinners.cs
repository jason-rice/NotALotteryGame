namespace webapi.Data
{
    public class DailyPowerballWinners
    {
        public long Id { get; set; }
        public string? AddressId { get; set; }
        public int NumOne { get; set; }
        public int NumTwo { get; set; }
        public int NumThree { get; set; }
        public long AmountPulse { get; set; }
        public DateTime DateAndTime { get; set; }
        public string? TransactionId { get; set; }
    }
}
