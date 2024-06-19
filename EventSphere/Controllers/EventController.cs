using EventSphere.DTOs;
using EventSphere.Services.Implementations;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace EventSphere.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;
        public EventController(IEventService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
        {
            var response = await _service.GetAllEvents();
            return Ok(response);
        }

        [HttpGet("eventById/{id}")]
        [Authorize]
        public async Task<ActionResult<EventDTO>> GetEventById(int id)
        {
            var events = await _service.GetEventById(id);
            return Ok(events);
        }

        [HttpGet("eventByOrganizer/{organizerId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsByOrganizerId(int organizerId)
        {

            var response = await _service.GetEventsByOrganizerId(organizerId);
            return Ok(response);

        }

        [HttpPost]
        [Authorize(Roles="Admin,Organizer")]
        public async Task<ActionResult> AddEvent(EventRequestDTO eventDto)
        {
            if (eventDto == null)
            {
                return BadRequest("EventRequestDTO object is null");
            }

            await _service.AddEvent(eventDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles="Admin,Organizer")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            try
            {
                await _service.DeleteEvent(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
                
            }
        }
        [HttpPut("{eventId}")]
        [Authorize(Roles="Admin,Organizer")]
        public async Task<ActionResult> UpdateEvent(EventRequestDTO eventDto, int eventId)
        {
            try
            {
                await _service.UpdateEvent(eventDto, eventId);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

        }

        [HttpGet("upcoming/popularity")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetUpcomingEventsSortedByPopularity()
        {
            var events = await _service.GetUpcomingEventsSortedByPopularity();
            return Ok(events);

        }
    }
}
