﻿using AutoMapper;
using EventSphere.DTOs;
using EventSphere.Repositories.Interfaces;
using EventSphere.Services.Interfaces;

namespace EventSphere.Services.Implementations
{
    public class OrganizerService : IOrganizerService
    {
        private readonly IOrganizerRepository _organizerRepository;
        private readonly IMapper _mapper;

        public OrganizerService(IOrganizerRepository organizerRepository, IMapper mapper)
        {
            _organizerRepository = organizerRepository;
            _mapper = mapper;
        }

        public async Task CreateOrganizer(OrganizerRequestDTO organizerDto)
        {
            await _organizerRepository.AddOrganizer(organizerDto);
        }

        public async Task DeleteOrganizer(int organizerId)
        {
            var existingOrganizer = await _organizerRepository.GetOrganizerById(organizerId);
            if (existingOrganizer == null)
            {
                throw new KeyNotFoundException("Organizer not found");
            }

            await _organizerRepository.DeleteOrganizer(organizerId);
        }

        public async Task<IEnumerable<OrganizerDTO>> GetAllOrganizers()
        {
            var organizers = await _organizerRepository.GetAllOrganizers();
            return _mapper.Map<IEnumerable<OrganizerDTO>>(organizers);
        }

        public async Task<OrganizerDTO> GetOrganizerById(int organizerId)
        {
            var organizer = await _organizerRepository.GetOrganizerById(organizerId);
            if (organizer == null)
            {
                throw new KeyNotFoundException("Organizer not found");
            }
            return _mapper.Map<OrganizerDTO>(organizer);
        }

        public async Task UpdateOrganizer(OrganizerRequestDTO organizerDto, int organizerId)
        {
            var existingOrganizer = await _organizerRepository.GetOrganizerById(organizerId);
            if (existingOrganizer == null)
            {
                throw new KeyNotFoundException("Organizer not found");
            }

            var organizer = _mapper.Map<OrganizerRequestDTO>(organizerDto);
            await _organizerRepository.UpdateOrganizer(organizer, organizerId);
        }
    }
}
