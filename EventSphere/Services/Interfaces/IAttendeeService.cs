using EventSphere.DTOs;

namespace EventSphere.Services.Interfaces
{
    public interface IAttendeeService
    {
        Task<AttendeeDTO> GetAttendeeById(int attendeeId);
        Task<IEnumerable<AttendeeDTO>> GetAllAttendees();
        Task AddAttendee(AttendeeDTO attendeeDto);
        Task UpdateAttendee(AttendeeDTO attendeeDto, int attendeeId);
        Task DeleteAttendee(int attendeeId);
        Task<IEnumerable<AttendeeDTO>> GetAttendeesByEvent(int eventId);
       

    }
}
