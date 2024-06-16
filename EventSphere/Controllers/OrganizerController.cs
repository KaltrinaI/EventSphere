using EventSphere.DTOs;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EventSphere.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizerController : ControllerBase
    {
        private readonly IOrganizerService _organizerService;
        private readonly IMemoryCache _memoryCache;


        public OrganizerController(IOrganizerService organizerService, IMemoryCache memoryCache)
        {
            _organizerService = organizerService;
            _memoryCache= memoryCache;

        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<OrganizerDTO>>> GetAllOrganizers()
        {
            if(_memoryCache.TryGetValue("organizers", out IEnumerable<OrganizerDTO>? Allorganizers)) {
                return Ok(Allorganizers);
            }
            var organizers = await _organizerService.GetAllOrganizers();
            _memoryCache.Set("organizers", organizers, TimeSpan.FromMinutes(10));
            return Ok(organizers);
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<OrganizerDTO>> GetOrganizerById(int id)
        {
            try
            {
                if(_memoryCache.TryGetValue("organizerById", out OrganizerDTO? organizerById))
                {
                    return Ok(organizerById);
                }
                var organizer = await _organizerService.GetOrganizerById(id);
                _memoryCache.Set("organizerById", organizer, TimeSpan.FromMinutes(10));
                return Ok(organizer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Organizer not found" });
            }
        }

        [HttpPost]
        //[Authorize(Roles="Admin")]
        public async Task<ActionResult> CreateOrganizer(OrganizerRequestDTO organizerDto)
        {
            if (organizerDto == null)
            {
                return BadRequest("OrganizerRequestDTO object is null");
            }

            await _organizerService.CreateOrganizer(organizerDto);
            return Ok();
        }

        [HttpPut("{id}")]
        //[Authorize(Roles="Admin")]
        public async Task<ActionResult> UpdateOrganizer(int id, OrganizerRequestDTO organizerDto)
        {
            try
            {
                await _organizerService.UpdateOrganizer(organizerDto, id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Organizer not found" });
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles="Admin")]
        public async Task<ActionResult> DeleteOrganizer(int id)
        {
            try
            {
                await _organizerService.DeleteOrganizer(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Organizer not found" });
            }
        }


    }
}
