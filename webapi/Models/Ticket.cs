namespace webapi.Models
{
    public class TicketOrder
    {
        public int? TicketNum { get; set; }
        public string? AccountNum { get; set; }
        public int Type { get; set; }
        public string? TxHash { get; set; }
        public int? NumOne { get; set; }
        public int? NumTwo { get; set; }
        public int? NumThree { get; set; }
    }
}
