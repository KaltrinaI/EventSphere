using EventSphere.DTOs;

namespace EventSphere.Services.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDTO> GetTicketById(int ticketId);
        Task<IEnumerable<TicketDTO>> GetTicketsByEventId(int eventId);
        Task AddTicket(TicketRequestDTO request);
        Task DeleteTicket(int ticketId);
        Task<IEnumerable<TicketDTO>> CheckTicketAvailability(int eventId);
        Task<decimal> CalculateRevenueForEvent(int eventId);
        Task SellTicket(int ticketId, int quantity);
        Task RefundTicket(int ticketId, int quantity);
        Task UpdateTicket(TicketRequestDTO ticketDto, int ticketId);
    }
}
