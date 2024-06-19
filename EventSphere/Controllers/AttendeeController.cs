using EventSphere.DTOs;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EventSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        private readonly IAttendeeService _service;

        public AttendeeController(IAttendeeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AttendeeDTO>>> GetAllAttendees()
        {
            var response = await _service.GetAllAttendees();  
            return Ok(response);
        }

        [HttpGet("{attendeeId}")]
        [Authorize]
        public async Task<ActionResult<AttendeeDTO>> GetAttendeeById(int attendeeId)
        {
            var response = await _service.GetAttendeeById( attendeeId);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles="Admin,Organizer")]
        public async Task<ActionResult> AddAttendee(AttendeeDTO attendeeDto)
        {
            if (attendeeDto == null)
            {
                throw new ArgumentNullException(nameof(attendeeDto));
            }
            await _service.AddAttendee(attendeeDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles="Admin,Organizer")]
        public async Task<ActionResult> DeleteAttendee(int id)
        {
            try
            {
                await _service.DeleteAttendee(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> UpdateAttendee(AttendeeDTO attendeeDto, int id)
        {
            try
            {
                await _service.UpdateAttendee(attendeeDto, id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

        }
        [HttpGet("attendeesByEvent/{eventId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AttendeeDTO>>> GetAttendeesByEvent(int eventId)
        {
            var response = await _service.GetAttendeesByEvent(eventId);
            return Ok(response);
        }
    }
}
