using EventSphere.Data;
using EventSphere.DTOs;
using EventSphere.Models;
using EventSphere.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Tests.RepositoryTests
{
    public class AttendeeRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _context;
        private readonly AttendeeRepository _repository;

        public AttendeeRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(_dbContextOptions);
            _repository = new AttendeeRepository(_context);
        }

        // AddAttendee Tests
        [Fact]
        public async Task AddAttendee_ShouldAddAttendee()
        {
            var eventEntity = new Event
            {
                Name = "Event 1",
                Description = "Description 1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(2),
                Location = "Test Location",
                Capacity = 100,
                OrganizerId = 1 // Ensure that there is a corresponding Organizer with this ID or add one in the test setup
            };
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            var request = new AttendeeDTO
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "123-456-7890",
                EventId = eventEntity.EventId
            };

            await _repository.AddAttendee(request);

            var attendees = _context.Attendees.Include(a => a.Events).ToList();
            Assert.Single(attendees);
            Assert.Equal(request.Name, attendees[0].Name);
            Assert.Single(attendees[0].Events);
            Assert.Equal(eventEntity.EventId, attendees[0].Events.First().EventId);
        }

        [Fact]
        public async Task AddAttendee_ShouldAddAttendeeWithoutEvent()
        {
            var request = new AttendeeDTO
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "123-456-7890",
                EventId = 999 // Non-existent event
            };

            await _repository.AddAttendee(request);

            var attendees = _context.Attendees.Include(a => a.Events).ToList();
            Assert.Single(attendees);
            Assert.Equal(request.Name, attendees[0].Name);
            Assert.Empty(attendees[0].Events);
        }

        // DeleteAttendee Tests
        [Fact]
        public async Task DeleteAttendee_ShouldDeleteAttendee()
        {
            var attendee = new Attendee { Name = "John Doe", Email = "john.doe@example.com", Phone = "123-456-7890" };
            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            await _repository.DeleteAttendee(attendee.AttendeeId);

            var deletedAttendee = await _context.Attendees.FindAsync(attendee.AttendeeId);
            Assert.Null(deletedAttendee);
        }

        [Fact]
        public async Task DeleteAttendee_ShouldNotFailForNonExistentAttendee()
        {
            await _repository.DeleteAttendee(999); // Non-existent attendee

            // No assertion needed, just verifying that no exception is thrown
        }

        // GetAllAttendees Tests
        [Fact]
        public async Task GetAllAttendees_ShouldReturnAllAttendees()
        {
            _context.Attendees.AddRange(new List<Attendee>
            {
                new Attendee { Name = "John Doe", Email = "john.doe@example.com", Phone = "123-456-7890" },
                new Attendee { Name = "Jane Doe", Email = "jane.doe@example.com", Phone = "098-765-4321" }
            });
            await _context.SaveChangesAsync();

            var attendees = await _repository.GetAllAttendees();

            Assert.Equal(2, attendees.Count());
        }

        [Fact]
        public async Task GetAllAttendees_ShouldReturnEmptyList()
        {
            var attendees = await _repository.GetAllAttendees();

            Assert.Empty(attendees);
        }

        // GetAttendeeById Tests
        [Fact]
        public async Task GetAttendeeById_ShouldReturnAttendee()
        {
            var attendee = new Attendee { Name = "John Doe", Email = "john.doe@example.com", Phone = "123-456-7890" };
            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            var foundAttendee = await _repository.GetAttendeeById(attendee.AttendeeId);

            Assert.Equal(attendee.AttendeeId, foundAttendee.AttendeeId);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldReturnNullForNonExistentAttendee()
        {
            var foundAttendee = await _repository.GetAttendeeById(999);

            Assert.Null(foundAttendee);
        }

        // GetAttendeesForEvent Tests
        [Fact]
        public async Task GetAttendeesForEvent_ShouldReturnAttendees()
        {
            var eventEntity = new Event
            {
                Name = "Event 1",
                Description = "Description 1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(2),
                Location = "Test Location",
                Capacity = 100,
                OrganizerId = 1 // Assuming an organizer with ID 1 exists or you can add an organizer in the test setup
            };
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            var attendee = new Attendee
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "123-456-7890"
            };
            attendee.Events = new List<Event> { eventEntity };
            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            // Ensure the relationship is correctly established
            await _context.Entry(attendee).Collection(a => a.Events).LoadAsync();
            await _context.Entry(eventEntity).Collection(e => e.Attendees).LoadAsync();

            var attendees = await _repository.GetAttendeesForEvent(eventEntity.EventId);

            Assert.Single(attendees);
            Assert.Equal(attendee.Name, attendees.First().Name);
        }

        [Fact]
        public async Task GetAttendeesForEvent_ShouldReturnEmptyListForNonExistentEvent()
        {
            var attendees = await _repository.GetAttendeesForEvent(999);

            Assert.Empty(attendees);
        }

        // UpdateAttendee Tests
        [Fact]
        public async Task UpdateAttendee_ShouldUpdateAttendee()
        {
            var attendee = new Attendee { Name = "John Doe", Email = "john.doe@example.com", Phone = "123-456-7890" };
            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            var request = new AttendeeDTO
            {
                Name = "Updated John Doe",
                Email = "updated.john.doe@example.com",
                Phone = "111-222-3333",
                EventId = 999 // No new event
            };

            await _repository.UpdateAttendee(request, attendee.AttendeeId);

            var updatedAttendee = await _context.Attendees.FindAsync(attendee.AttendeeId);
            Assert.Equal(request.Name, updatedAttendee.Name);
            Assert.Equal(request.Email, updatedAttendee.Email);
            Assert.Equal(request.Phone, updatedAttendee.Phone);
        }

        [Fact]
        public async Task UpdateAttendee_ShouldNotUpdateNonExistentAttendee()
        {
            var request = new AttendeeDTO
            {
                Name = "Updated John Doe",
                Email = "updated.john.doe@example.com",
                Phone = "111-222-3333",
                EventId = 999 // No new event
            };

            await _repository.UpdateAttendee(request, 999);

            var updatedAttendee = await _context.Attendees.FindAsync(999);
            Assert.Null(updatedAttendee);
        }

        public void Dispose()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Attendees.RemoveRange(context.Attendees);
                context.SaveChanges();
            }
        }
    }
}
