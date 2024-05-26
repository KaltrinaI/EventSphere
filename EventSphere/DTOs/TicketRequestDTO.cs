namespace EventSphere.DTOs
{
    public class TicketRequestDTO
    {
        public int EventId { get; set; }
        public decimal Price { get; set; }
        public string TicketType { get; set; }
        public int QuantityAvailable { get; set; }
    }
}
