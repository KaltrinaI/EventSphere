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
        //[AllowAnonymous]
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
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTicketsByEventId(int eventId)
        {
            if(_memoryCache.TryGetValue("ticketsByEvent", out IEnumerable<TicketDTO>? ticketList))
            {
                return Ok(ticketList);
            }
            var tickets = await _ticketService.GetTicketsByEventId(eventId);
            _memoryCache.Set("ticketsByEvent",tickets, TimeSpan.FromMinutes(10));
            return Ok(tickets);

        }

        [HttpPost]
        //[Authorize (Roles ="Admin,Organizer")]
        public async Task<ActionResult> AddTicket(TicketRequestDTO ticketDto)
        {
            await _ticketService.AddTicket(ticketDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        //[Authorize (Roles ="Admin,Organizer")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            await _ticketService.DeleteTicket(id);
            return Ok();
        }

        [HttpGet("{eventId}/available")]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> CheckTicketAvailability(int eventId)
        {
            if(_memoryCache.TryGetValue("availableTickets", out IEnumerable<TicketDTO>? ticketList))
            {
                return Ok(ticketList);
            }
            var tickets = await _ticketService.CheckTicketAvailability(eventId);
            _memoryCache.Set("availableTickets", tickets, TimeSpan.FromMinutes(3));
            return Ok(tickets);
        }

        [HttpPost("{id},{quantity}/sell")]
        //[Authorize (Roles ="Admin,Organizer")]
        public async Task<ActionResult> SellTicket(int id, int quantity)
        {
            await _ticketService.SellTicket(id, quantity);
            return Ok();
        }

        [HttpPost("{id},{quantity}/refund")]
        //[Authorize (Roles ="Admin,Organizer")]
        public async Task<ActionResult> RefundTicket(int id, int quantity)
        {
            await _ticketService.RefundTicket(id, quantity);
            return Ok();
        }

        [HttpPut("{id}")]
        //[Authorize (Roles ="Admin,Organizer")]
        public async Task<ActionResult> UpdateTicket(int id, TicketRequestDTO ticketDto)
        {
            await _ticketService.UpdateTicket(ticketDto, id);
            return Ok();
        }

    }
}
