using EventSphere.Data;
using EventSphere.DTOs;
using EventSphere.Models;
using EventSphere.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSphere.Repositories.Implementations
{
    public class AttendeeRepository : IAttendeeRepository
    {
        private readonly AppDbContext _context;

        public AttendeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAttendee(AttendeeDTO request)
        {
            // Find the event by EventId
            var eventEntity = await _context.Events.FindAsync(request.EventId);
            if (eventEntity == null)
            {
                throw new Exception("Event not found");
            }

            // Create the attendee and add the event to the attendee's events list
            var attendee = new Attendee
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                Events = new List<Event> { eventEntity }
            };

            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAttendee(int attendeeId)
        {
            var attendee = await _context.Attendees.Include(a => a.Events).FirstOrDefaultAsync(a => a.AttendeeId == attendeeId);
            if (attendee != null)
            {
                _context.Attendees.Remove(attendee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Attendee>> GetAllAttendees()
        {
            return await _context.Attendees.ToListAsync();
        }

        public async Task<Attendee> GetAttendeeById(int attendeeId)
        {
            return await _context.Attendees.FindAsync(attendeeId);
        }

        public async Task<IEnumerable<Attendee>> GetAttendeesForEvent(int eventId)
        {
            return await _context.Attendees
                .Where(a => a.Events.Any(e => e.EventId == eventId))
                .ToListAsync();
        }

        public async Task UpdateAttendee(AttendeeDTO request, int attendeeId)
        {
            var existingAttendee = await _context.Attendees.Include(a => a.Events).FirstOrDefaultAsync(a => a.AttendeeId == attendeeId);
            if (existingAttendee != null)
            {
                existingAttendee.Name = request.Name;
                existingAttendee.Email = request.Email;
                existingAttendee.Phone = request.Phone;

                var eventEntity = await _context.Events.FindAsync(request.EventId);
                if (eventEntity != null && !existingAttendee.Events.Contains(eventEntity))
                {
                    existingAttendee.Events.Add(eventEntity);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
