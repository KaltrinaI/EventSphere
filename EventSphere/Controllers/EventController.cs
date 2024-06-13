using EventSphere.DTOs;
using EventSphere.Services.Implementations;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;
        public EventController(IEventService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
        {
            var response = await _service.GetAllEvents();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEventById(int eventid)
        {
            var events = await _service.GetEventById(eventid);
            return Ok(events);
        }

        [HttpGet("{organizerId}")]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsByOrganizerId(int organizerId)
        {
            var tickets = await _service.GetEventsByOrganizerId(organizerId);
            return Ok(tickets);

        }

        [HttpPost]
        public async Task<ActionResult> AddEvent(EventRequestDTO eventDto)
        {
            await _service.AddEvent(eventDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            await _service.DeleteEvent(id);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent(EventRequestDTO eventDto, int eventId)
        {
            await _service.UpdateEvent(eventDto, eventId);
            return Ok();

        }

        [HttpGet("upcoming/popularity")]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetUpcomingEventsSortedByPopularity()
        {


            var events = await _service.GetUpcomingEventsSortedByPopularity();
            return Ok(events);

        }
    }
}
