namespace webapi.Data
{
    public class DailyPowerball
    {
        public long Id { get; set; }
        public required string AddressId { get; set; }
        public int NumOne { get; set; }
        public int NumTwo { get; set; }
        public int NumThree { get; set; }
    }
}
