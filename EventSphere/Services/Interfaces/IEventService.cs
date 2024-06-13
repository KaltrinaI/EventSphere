using EventSphere.DTOs;

namespace EventSphere.Services.Interfaces
{
    public interface IEventService
    {
        Task<EventDTO> GetEventById(int eventId);
        Task<IEnumerable<EventDTO>> GetAllEvents();
        Task<IEnumerable<EventDTO>> GetEventsByOrganizerId(int organizerId);
        Task<IEnumerable<EventDTO>> GetUpcomingEventsSortedByPopularity();
        Task AddEvent(EventRequestDTO eventDto);
        Task UpdateEvent(EventRequestDTO eventDto, int eventId);
        Task DeleteEvent(int eventId);
    }
}
