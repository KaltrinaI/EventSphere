using EventSphere.DTOs;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizerController : ControllerBase
    {
        private readonly IOrganizerService _organizerService;


        public OrganizerController(IOrganizerService organizerService)
        {
            _organizerService = organizerService;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizerDTO>>> GetAllOrganizers()
        {
            var organizers = await _organizerService.GetAllOrganizers();
            return Ok(organizers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizerDTO>> GetOrganizerById(int id)
        {
            try
            {
                var organizer = await _organizerService.GetOrganizerById(id);
                return Ok(organizer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Organizer not found" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrganizer(OrganizerRequestDTO organizerDto)
        {
            await _organizerService.CreateOrganizer(organizerDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrganizer(int id, OrganizerRequestDTO organizerDto)
        {
            await _organizerService.UpdateOrganizer(organizerDto, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrganizer(int id)
        {

            await _organizerService.DeleteOrganizer(id);
            return Ok();
        }

    }
}
