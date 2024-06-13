﻿using EventSphere.DTOs;
using EventSphere.Services.Implementations;
using EventSphere.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace EventSphere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _service;
        private readonly IMemoryCache _memoryCache;
        public EventController(IEventService service, IMemoryCache memoryCache)
        {
            _service = service;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
        {
            if(_memoryCache.TryGetValue("events",out IEnumerable<EventDTO>? events)) {
                return Ok(events);
            }
            var response = await _service.GetAllEvents();
            _memoryCache.Set("events",response, TimeSpan.FromMinutes(10));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEventById(int eventid)
        {
            if(_memoryCache.TryGetValue("eventById", out EventDTO? eventById)){
                return Ok(eventById);
            }
            var events = await _service.GetEventById(eventid);
            _memoryCache.Set("eventById",events,TimeSpan.FromMinutes(10));
            return Ok(events);
        }

        [HttpGet("{organizerId}")]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsByOrganizerId(int organizerId)
        {
            if(_memoryCache.TryGetValue("eventsByOrganizer", out IEnumerable<EventDTO>? eventsByOrganizer)){
                return Ok(eventsByOrganizer);
            }
            var response = await _service.GetEventsByOrganizerId(organizerId);
            _memoryCache.Set("eventsByOrganizer", response, TimeSpan.FromMinutes(10));
            return Ok(response);

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
            if(_memoryCache.TryGetValue("popularEvents", out IEnumerable<EventDTO>? upcomingEvents))
            {
                return Ok(upcomingEvents);
            }
            var events = await _service.GetUpcomingEventsSortedByPopularity();
            _memoryCache.Set("popularEvents", events, TimeSpan.FromMinutes(10));
            return Ok(events);

        }
    }
}