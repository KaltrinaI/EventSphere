using EventSphere.DTOs;
using EventSphere.Models;

namespace EventSphere.Repositories.Interfaces
{
    public interface IAttendeeRepository

    {

        Task<Attendee> GetAttendeeById(int attendeeId);
        Task<IEnumerable<Attendee>> GetAllAttendees();
        Task AddAttendee(AttendeeDTO attendee);
        Task UpdateAttendee(AttendeeDTO attendee, int attendeeId);
        Task DeleteAttendee(int attendeeId);
        Task<IEnumerable<Attendee>> GetAttendeesForEvent(int eventId);
    }
}


