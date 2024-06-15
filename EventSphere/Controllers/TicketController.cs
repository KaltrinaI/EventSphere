using EventSphere.DTOs;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EventSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IMemoryCache _memoryCache;
        public TicketController(ITicketService ticketService, IMemoryCache memoryCache)
        {
            _ticketService = ticketService;
            _memoryCache = memoryCache;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDTO>> GetTicketById(int id)
        {
            try
            {
                if (_memoryCache.TryGetValue("ticketById", out TicketDTO? ticketById))
                {
                    return Ok(ticketById);
                }
                var ticket = await _ticketService.GetTicketById(id);
                _memoryCache.Set("ticketById", ticket, TimeSpan.FromMinutes(10));
                return Ok(ticket);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ticket not found" });
            }
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTicketsByEventId(int eventId)
        {
            if (_memoryCache.TryGetValue("ticketsByEvent", out IEnumerable<TicketDTO>? ticketList))
            {
                return Ok(ticketList);
            }
            var tickets = await _ticketService.GetTicketsByEventId(eventId);
            _memoryCache.Set("ticketsByEvent", tickets, TimeSpan.FromMinutes(10));
            return Ok(tickets);
        }

        [HttpPost]
        public async Task<ActionResult> AddTicket(TicketRequestDTO ticketDto)
        {
            if (ticketDto == null)
            {
                return BadRequest("TicketRequestDTO cannot be null");
            }

            await _ticketService.AddTicket(ticketDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            try
            {
                await _ticketService.DeleteTicket(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ticket not found" });
            }
        }

        [HttpGet("{eventId}/available")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> CheckTicketAvailability(int eventId)
        {
            if (_memoryCache.TryGetValue("availableTickets", out IEnumerable<TicketDTO>? ticketList))
            {
                return Ok(ticketList);
            }
            var tickets = await _ticketService.CheckTicketAvailability(eventId);
            _memoryCache.Set("availableTickets", tickets, TimeSpan.FromMinutes(3));
            return Ok(tickets);
        }

        [HttpGet("{eventId}/revenue")]
        public async Task<ActionResult<decimal>> CalculateRevenueForEvent(int eventId)
        {
            var revenue = await _ticketService.CalculateRevenueForEvent(eventId);
            return Ok(revenue);
        }

        [HttpPost("{id},{quantity}/sell")]
        public async Task<ActionResult> SellTicket(int id, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero");
            }

            try
            {
                await _ticketService.SellTicket(id, quantity);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ticket not found" });
            }
        }

        [HttpPost("{id},{quantity}/refund")]
        public async Task<ActionResult> RefundTicket(int id, int quantity)
        {
            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero");
            }

            try
            {
                await _ticketService.RefundTicket(id, quantity);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ticket not found" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTicket(int id, TicketRequestDTO ticketDto)
        {
            if (ticketDto == null)
            {
                return BadRequest("TicketRequestDTO cannot be null");
            }

            try
            {
                await _ticketService.UpdateTicket(ticketDto, id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Ticket not found" });
            }
        }
    }
}
