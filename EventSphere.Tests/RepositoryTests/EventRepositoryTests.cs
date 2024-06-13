using EventSphere.Data;
using EventSphere.DTOs;
using EventSphere.Models;
using EventSphere.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.Tests.RepositoryTests
{
    public class EventRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _context;
        private readonly EventRepository _repository;

        public EventRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(_dbContextOptions);
            _repository = new EventRepository(_context);
        }

        // GetEventById Tests
        [Fact]
        public async Task GetEventById_ShouldReturnEvent()
        {
            var @event = new Event
            {
                Name = "Event 1",
                Description = "Description 1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(2),
                Location = "Test Location",
                Capacity = 100,
                OrganizerId = 1 // Ensure that there is a corresponding Organizer with this ID or add one in the test setup
            };
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            var foundEvent = await _repository.GetEventById(@event.EventId);

            Assert.Equal(@event.EventId, foundEvent.EventId);
        }


        [Fact]
        public async Task GetEventById_ShouldReturnNullForNonExistentEvent()
        {
            var foundEvent = await _repository.GetEventById(999);

            Assert.Null(foundEvent);
        }

        // GetAllEvents Tests
        [Fact]
        public async Task GetAllEvents_ShouldReturnAllEvents()
        {
            var organizer = new Organizer
            {
                Name = "Organizer 1",
                Phone = "987-654-3210"
            };
            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();

            _context.Events.AddRange(new List<Event>
    {
        new Event
        {
            Name = "Event 1",
            Description = "Description 1",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(2),
            Location = "Location 1",
            Capacity = 100,
            OrganizerId = organizer.OrganizerId
        },
        new Event
        {
            Name = "Event 2",
            Description = "Description 2",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(3),
            Location = "Location 2",
            Capacity = 200,
            OrganizerId = organizer.OrganizerId
        }
    });
            await _context.SaveChangesAsync();

            var events = await _repository.GetAllEvents();

            Assert.Equal(2, events.Count());
        }


        [Fact]
        public async Task GetAllEvents_ShouldReturnEmptyList()
        {
            var events = await _repository.GetAllEvents();

            Assert.Empty(events);
        }

        // GetEventsByOrganizerId Tests
        [Fact]
        public async Task GetEventsByOrganizerId_ShouldReturnEvents()
        {
            var organizer1 = new Organizer
            {
                Name = "Organizer 1",
                Phone = "987-654-3210"
            };
            var organizer2 = new Organizer
            {
                Name = "Organizer 2",
                Phone = "123-456-7890"
            };
            _context.Organizers.AddRange(organizer1, organizer2);
            await _context.SaveChangesAsync();

            _context.Events.AddRange(new List<Event>
    {
        new Event
        {
            Name = "Event 1",
            Description = "Description 1",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(2),
            Location = "Location 1",
            Capacity = 100,
            OrganizerId = organizer1.OrganizerId
        },
        new Event
        {
            Name = "Event 2",
            Description = "Description 2",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(3),
            Location = "Location 2",
            Capacity = 200,
            OrganizerId = organizer2.OrganizerId
        }
    });
            await _context.SaveChangesAsync();

            var events = await _repository.GetEventsByOrganizerId(organizer1.OrganizerId);

            Assert.Single(events);
            Assert.Equal("Event 1", events.First().Name);
        }


        [Fact]
        public async Task GetEventsByOrganizerId_ShouldReturnEmptyListForNonExistentOrganizer()
        {
            var events = await _repository.GetEventsByOrganizerId(999);

            Assert.Empty(events);
        }

        // AddEvent Tests
        [Fact]
        public async Task AddEvent_ShouldAddEvent()
        {
            var request = new EventRequestDTO
            {
                Name = "Event 1",
                Description = "Description 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Location = "Location 1",
                Capacity = 100,
                OrganizerId = 1
            };

            await _repository.AddEvent(request);

            var events = _context.Events.ToList();
            Assert.Single(events);
            Assert.Equal(request.Name, events[0].Name);
        }

        // UpdateEvent Tests
        [Fact]
        public async Task UpdateEvent_ShouldUpdateEvent()
        {
            var organizer = new Organizer
            {
                Name = "Organizer 1",
                Phone = "987-654-3210"
            };
            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();

            var @event = new Event
            {
                Name = "Event 1",
                Description = "Description 1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(2),
                Location = "Initial Location",
                Capacity = 100,
                OrganizerId = organizer.OrganizerId
            };
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            var request = new EventRequestDTO
            {
                Name = "Updated Event",
                Description = "Updated Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Location = "Updated Location",
                Capacity = 200,
                OrganizerId = organizer.OrganizerId
            };

            await _repository.UpdateEvent(request, @event.EventId);

            var updatedEvent = await _context.Events.FindAsync(@event.EventId);
            Assert.Equal(request.Name, updatedEvent.Name);
            Assert.Equal(request.Description, updatedEvent.Description);
            Assert.Equal(request.Location, updatedEvent.Location);
            Assert.Equal(request.Capacity, updatedEvent.Capacity);
            Assert.Equal(request.StartDate, updatedEvent.StartDate);
            Assert.Equal(request.EndDate, updatedEvent.EndDate);
        }


        [Fact]
        public async Task UpdateEvent_ShouldNotUpdateNonExistentEvent()
        {
            var request = new EventRequestDTO
            {
                Name = "Updated Event",
                Description = "Updated Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                Location = "Updated Location",
                Capacity = 200,
                OrganizerId = 1
            };

            await _repository.UpdateEvent(request, 999);

            var updatedEvent = await _context.Events.FindAsync(999);
            Assert.Null(updatedEvent);
        }

        // DeleteEvent Tests
        [Fact]
        public async Task DeleteEvent_ShouldDeleteEvent()
        {
            var organizer = new Organizer
            {
                Name = "Organizer 1",
                Phone = "987-654-3210"
            };
            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();

            var @event = new Event
            {
                Name = "Event 1",
                Description = "Description 1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(2),
                Location = "Initial Location",
                Capacity = 100,
                OrganizerId = organizer.OrganizerId
            };
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            await _repository.DeleteEvent(@event.EventId);

            var deletedEvent = await _context.Events.FindAsync(@event.EventId);
            Assert.Null(deletedEvent);
        }


        [Fact]
        public async Task DeleteEvent_ShouldNotFailForNonExistentEvent()
        {
            await _repository.DeleteEvent(999); // Non-existent event

            // No assertion needed, just verifying that no exception is thrown
        }

        // GetUpcomingEventsSortedByPopularity Tests
        [Fact]
        public async Task GetUpcomingEventsSortedByPopularity_ShouldReturnEventsSortedByPopularity()
        {
            var currentDate = DateTime.Now;

            var organizer = new Organizer
            {
                Name = "Organizer 1",
                Phone = "987-654-3210"
            };
            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();

            _context.Events.AddRange(new List<Event>
    {
        new Event
        {
            Name = "Event 1",
            Description = "Description 1",
            StartDate = currentDate.AddDays(1),
            EndDate = currentDate.AddDays(2),
            Location = "Location 1",
            Capacity = 100,
            OrganizerId = organizer.OrganizerId,
            Tickets = new List<Ticket> { new Ticket { TicketType = "General", Price = 100, QuantityAvailable = 50 }, new Ticket { TicketType = "VIP", Price = 200, QuantityAvailable = 30 } }
        },
        new Event
        {
            Name = "Event 2",
            Description = "Description 2",
            StartDate = currentDate.AddDays(1),
            EndDate = currentDate.AddDays(2),
            Location = "Location 2",
            Capacity = 200,
            OrganizerId = organizer.OrganizerId,
            Tickets = new List<Ticket> { new Ticket { TicketType = "General", Price = 100, QuantityAvailable = 50 } }
        }
    });
            await _context.SaveChangesAsync();

            var events = await _repository.GetUpcomingEventsSortedByPopularity();

            Assert.Equal(2, events.Count());
            Assert.Equal("Event 1", events.First().Name); // Event 1 should be first due to more tickets
        }


        [Fact]
        public async Task GetUpcomingEventsSortedByPopularity_ShouldReturnEmptyForNoUpcomingEvents()
        {
            var events = await _repository.GetUpcomingEventsSortedByPopularity();

            Assert.Empty(events);
        }

        public void Dispose()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Events.RemoveRange(context.Events);
                context.SaveChanges();
            }
        }
    }
}
