using EventSphere.DTOs;

namespace EventSphere.Services.Interfaces
{
    public interface IOrganizerService
    {
        Task<OrganizerDTO> GetOrganizerById(int organizerId);
        Task<IEnumerable<OrganizerDTO>> GetAllOrganizers();
        Task CreateOrganizer(OrganizerRequestDTO organizerDto);
        Task UpdateOrganizer(OrganizerRequestDTO organizerDto, int organizerId);
        Task DeleteOrganizer(int organizerId);
    }
}
