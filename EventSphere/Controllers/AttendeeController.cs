using EventSphere.DTOs;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<AttendeeDTO>>> GetAllAttendees()
        {
            var response = await _service.GetAllAttendees();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttendeeDTO>> GetAttendeeById(int attendeeId)
        {
            var events = await _service.GetAttendeeById( attendeeId);
            return Ok(events);
        }

        [HttpPost]
        public async Task<ActionResult> AddAttendee(AttendeeDTO attendeeDto)
        {
            await _service.AddAttendee(attendeeDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAttendee(int id)
        {
            await _service.DeleteAttendee(id);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAttendee(AttendeeDTO attendeeDto, int id)
        {
            await _service.UpdateAttendee(attendeeDto ,id);
            return Ok();

        }
        [HttpGet("{eventId}")]
        public async Task<ActionResult<IEnumerable<AttendeeDTO>>> GetAttendeesByEvent(int eventId)
        {
            var tickets = await _service.GetAttendeesByEvent(eventId);
            return Ok(tickets);

        }
    }
}
