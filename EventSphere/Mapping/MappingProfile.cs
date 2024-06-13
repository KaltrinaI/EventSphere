using AutoMapper;
using EventSphere.DTOs;
using EventSphere.Models;

namespace EventSphere.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrganizerDTO, Organizer>();
            CreateMap<Organizer, OrganizerDTO>();
            CreateMap<OrganizerRequestDTO, Organizer>();
            CreateMap<Organizer, OrganizerRequestDTO>();
            CreateMap<TicketDTO, Ticket>();
            CreateMap<Ticket, TicketDTO>();
            CreateMap<TicketRequestDTO, Ticket>();
            CreateMap<Ticket, TicketRequestDTO>();
            CreateMap<AttendeeDTO, Attendee>();
            CreateMap<Attendee, AttendeeDTO>();

            CreateMap<EventDTO, Event>();
            CreateMap<Event, EventDTO>();

            CreateMap<EventRequestDTO, Event>();
            CreateMap<Event, EventRequestDTO>();

            CreateMap<AttendeeRequestDTO, Attendee>();
            CreateMap<Attendee, AttendeeRequestDTO>();
        }
    }
}
