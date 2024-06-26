﻿using EventSphere.DTOs;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrganizerDTO>>> GetAllOrganizers()
        {
            var organizers = await _organizerService.GetAllOrganizers();
            return Ok(organizers);
        }

        [HttpGet("{id}")]
        [Authorize]
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
        [Authorize(Roles="Admin")]
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
        [Authorize(Roles="Admin")]
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
        [Authorize(Roles="Admin")]
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
