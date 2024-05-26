using EventSphere.Data;
using EventSphere.DTOs;
using EventSphere.Models;
using EventSphere.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.Repositories.Implementations
{
    public class TicketRepostiory : ITicketRepository
    {

        private readonly AppDbContext _context;
        public TicketRepostiory(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddTicket(TicketRequestDTO request)
        {
            Ticket requestBody = new Ticket();
            requestBody.EventId = request.EventId;
            requestBody.Price = request.Price;
            requestBody.TicketType = request.TicketType;
            requestBody.QuantityAvailable = request.QuantityAvailable;

            _context.Tickets.Add(requestBody);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateTotalRevenueFromEvent(int eventId)
        {
            return await _context.Tickets
                             .Where(t => t.EventId == eventId)
                             .SumAsync(t => t.Price * (t.QuantityAvailable - t.QuantityAvailable));
        }

        public async Task DeleteTicket(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Ticket>> GetAvailableTickets(int eventId)
        {
            return await _context.Tickets
                             .Where(t => t.EventId == eventId && t.QuantityAvailable > 0)
                             .ToListAsync();
        }

        public async Task<Ticket> GetTicketById(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            return ticket ?? new Ticket();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByEventId(int eventId)
        {
            return await _context.Tickets
                         .Where(t => t.EventId == eventId)
                         .ToListAsync();
        }

        public async Task UpdateTicket(TicketRequestDTO request, int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);

            if (ticket != null)
            {
                ticket.EventId = request.EventId;
                ticket.Price = request.Price;
                ticket.TicketType = request.TicketType;
                ticket.QuantityAvailable = request.QuantityAvailable;

                _context.Entry(ticket).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}

