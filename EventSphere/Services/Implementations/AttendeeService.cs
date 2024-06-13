using AutoMapper;
using EventSphere.DTOs;
using EventSphere.Models;
using EventSphere.Repositories.Interfaces;
using EventSphere.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSphere.Services.Implementations
{
    public class AttendeeService : IAttendeeService
    {
        private readonly IAttendeeRepository _repository;
        private readonly IMapper _mapper;

        public AttendeeService(IAttendeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AttendeeDTO> GetAttendeeById(int attendeeId)
        {
            var attendee = await _repository.GetAttendeeById(attendeeId);
            if (attendee == null)
            {
                throw new KeyNotFoundException("Attendee not found");
            }
            return _mapper.Map<AttendeeDTO>(attendee);
        }

        public async Task<IEnumerable<AttendeeDTO>> GetAllAttendees()
        {
            var attendees = await _repository.GetAllAttendees();
            return attendees?.Select(attendee => _mapper.Map<AttendeeDTO>(attendee));
        }

        public async Task AddAttendee(AttendeeDTO attendeeDto)
        {
            await _repository.AddAttendee(attendeeDto);
        }

        public async Task UpdateAttendee(AttendeeDTO attendeeDto, int attendeeId)
        {
            await _repository.UpdateAttendee(attendeeDto, attendeeId);
        }

        public async Task DeleteAttendee(int attendeeId)
        {
            await _repository.DeleteAttendee(attendeeId);
        }

        public async Task<IEnumerable<AttendeeDTO>> GetAttendeesByEvent(int eventId)
        {
            var attendees = await _repository.GetAttendeesForEvent(eventId);
            return attendees?.Select(attendee => _mapper.Map<AttendeeDTO>(attendee));
        }
    }
}
