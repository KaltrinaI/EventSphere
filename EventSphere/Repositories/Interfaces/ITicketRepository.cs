using EventSphere.DTOs;
using EventSphere.Models;

namespace EventSphere.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket> GetTicketById(int ticketId);
        Task<IEnumerable<Ticket>> GetTicketsByEventId(int eventId);
        Task AddTicket(TicketRequestDTO request);
        Task UpdateTicket(TicketRequestDTO request, int ticketId);
        Task DeleteTicket(int ticketId);
        Task<IEnumerable<Ticket>> GetAvailableTickets(int eventId);
    }
}
