namespace EventSphere.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int EventId { get; set; }
        public decimal Price { get; set; }
        public string TicketType { get; set; }
        public int QuantityAvailable { get; set; }
        public Event Event { get; set; }
    }
}
