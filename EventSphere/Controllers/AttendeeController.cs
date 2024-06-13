﻿using EventSphere.DTOs;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EventSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        private readonly IAttendeeService _service;
        private readonly IMemoryCache _memoryCache;

        public AttendeeController(IAttendeeService service, IMemoryCache memoryCache)
        {
            _service = service;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendeeDTO>>> GetAllAttendees()
        {
            if (_memoryCache.TryGetValue("AllAttendees", out IEnumerable<AttendeeDTO>? attendees))
            {
                return Ok(attendees);
            }
            
            var response = await _service.GetAllAttendees();
            _memoryCache.Set("AllAttendees", response, TimeSpan.FromMinutes(10));
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AttendeeDTO>> GetAttendeeById(int attendeeId)
        {
            if(_memoryCache.TryGetValue("attendee", out AttendeeDTO? attendee))
            {
                return Ok(attendee);
            }
            var response = await _service.GetAttendeeById( attendeeId);
            _memoryCache.Set("attendee",response, TimeSpan.FromMinutes(10));
            return Ok(response);
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
            if(_memoryCache.TryGetValue("attendeesByEvent", out IEnumerable<AttendeeDTO>? attendeeList))
            {
                return Ok(attendeeList);
            }
            var response = await _service.GetAttendeesByEvent(eventId);
            _memoryCache.Set("attendeesByEvent", response, TimeSpan.FromMinutes(10));
            return Ok(response);

        }
    }
}