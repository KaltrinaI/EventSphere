﻿using EventSphere.DTOs;
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