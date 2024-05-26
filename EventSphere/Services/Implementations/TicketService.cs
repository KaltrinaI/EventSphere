using AutoMapper;
using EventSphere.DTOs;
using EventSphere.Repositories.Interfaces;
using EventSphere.Services.Interfaces;

namespace EventSphere.Services.Implementations
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }
        public async Task AddTicket(TicketRequestDTO request)
        {
            await _ticketRepository.AddTicket(request);
        }

        public async Task<decimal> CalculateRevenueForEvent(int eventId)
        {
            return await _ticketRepository.CalculateTotalRevenueFromEvent(eventId);
        }

        public async Task<IEnumerable<TicketDTO>> CheckTicketAvailability(int eventId)
        {
            var tickets = await _ticketRepository.GetAvailableTickets(eventId);
            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public async Task DeleteTicket(int ticketId)
        {
            var existingTicket = await _ticketRepository.GetTicketById(ticketId);
            if (existingTicket == null)
            {
                throw new KeyNotFoundException("Ticket not found");
            }

            await _ticketRepository.DeleteTicket(ticketId);
        }

        public async Task<TicketDTO> GetTicketById(int ticketId)
        {
            var ticket = await _ticketRepository.GetTicketById(ticketId);
            if (ticket == null)
            {
                throw new KeyNotFoundException("Ticket not found");
            }
            return _mapper.Map<TicketDTO>(ticket);
        }

        public async Task<IEnumerable<TicketDTO>> GetTicketsByEventId(int eventId)
        {
            var tickets = await _ticketRepository.GetTicketsByEventId(eventId);
            return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
        }



        public async Task SellTicket(int ticketId, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero");

            var ticket = await _ticketRepository.GetTicketById(ticketId);
            if (ticket == null) throw new KeyNotFoundException("Ticket not found");
            if (ticket.QuantityAvailable < quantity) throw new InvalidOperationException("Not enough tickets available");

            ticket.QuantityAvailable -= quantity;
            var updatedTicketQuantity = _mapper.Map<TicketRequestDTO>(ticket);

            await _ticketRepository.UpdateTicket(updatedTicketQuantity, ticketId);
        }

        public async Task RefundTicket(int ticketId, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero");

            var ticket = await _ticketRepository.GetTicketById(ticketId);
            if (ticket == null) throw new KeyNotFoundException("Ticket not found");

            ticket.QuantityAvailable += quantity;
            var updatedTicketQuantity = _mapper.Map<TicketRequestDTO>(ticket);
            await _ticketRepository.UpdateTicket(updatedTicketQuantity, ticketId);
        }

        public async Task UpdateTicket(TicketRequestDTO ticketDto, int ticketId)
        {
            var existingTicket = await _ticketRepository.GetTicketById(ticketId);
            if (existingTicket == null)
            {
                throw new KeyNotFoundException("Organizer not found");
            }

            var ticket = _mapper.Map<TicketRequestDTO>(ticketDto);
            await _ticketRepository.UpdateTicket(ticket, ticketId);
        }
    }

}

