using EventSphere.DTOs;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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

        [HttpGet("ticketsByEvent/{eventId}")]
        [Authorize]
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
        [Authorize (Roles ="Admin,Organizer")]
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
        [Authorize (Roles ="Admin,Organizer")]
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

        [HttpGet("available/{eventId}")]
        [Authorize]
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

        [HttpPatch("sell/{id},{quantity}")]
        [Authorize (Roles ="Admin")]
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

        [HttpPatch("refund/{id},{quantity}")]
        [Authorize (Roles ="Admin")]
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
        [Authorize (Roles ="Admin")]
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
