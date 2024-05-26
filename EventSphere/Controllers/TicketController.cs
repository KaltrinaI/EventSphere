using EventSphere.DTOs;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDTO>> GetTicketById(int id)
        {
            var ticket = await _ticketService.GetTicketById(id);
            return Ok(ticket);
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTicketsByEventId(int eventId)
        {
            var tickets = await _ticketService.GetTicketsByEventId(eventId);
            return Ok(tickets);

        }

        [HttpPost]
        public async Task<ActionResult> AddTicket(TicketRequestDTO ticketDto)
        {
            await _ticketService.AddTicket(ticketDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            await _ticketService.DeleteTicket(id);
            return Ok();
        }

        [HttpGet("{eventId}/available")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> CheckTicketAvailability(int eventId)
        {
            var tickets = await _ticketService.CheckTicketAvailability(eventId);
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
            await _ticketService.SellTicket(id, quantity);
            return Ok();
        }

        [HttpPost("{id},{quantity}/refund")]
        public async Task<ActionResult> RefundTicket(int id, int quantity)
        {
            await _ticketService.RefundTicket(id, quantity);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTicket(int id, TicketRequestDTO ticketDto)
        {
            await _ticketService.UpdateTicket(ticketDto, id);
            return Ok();

        }

    }
}
