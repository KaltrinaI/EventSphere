using EventSphere.Data;
using EventSphere.DTOs;
using EventSphere.Models;
using EventSphere.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Tests.RepositoryTests
{
    public class TicketRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _context;
        private readonly TicketRepostiory _repository;

        public TicketRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(_dbContextOptions);
            _repository = new TicketRepostiory(_context);
        }

        // AddTicket Tests
        [Fact]
        public async Task AddTicket_ShouldAddTicket()
        {
            var request = new TicketRequestDTO
            {
                EventId = 1,
                Price = 50,
                TicketType = "VIP",
                QuantityAvailable = 100
            };

            await _repository.AddTicket(request);

            var tickets = _context.Tickets.ToList();
            Assert.Single(tickets);
            Assert.Equal(request.EventId, tickets[0].EventId);
        }

        // DeleteTicket Tests
        [Fact]
        public async Task DeleteTicket_ShouldDeleteTicket()
        {
            var ticket = new Ticket
            {
                EventId = 1,
                Price = 100,
                QuantityAvailable = 50,
                TicketType = "General Admission" // Include the required TicketType property
            };
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            await _repository.DeleteTicket(ticket.TicketId);

            var deletedTicket = await _context.Tickets.FindAsync(ticket.TicketId);
            Assert.Null(deletedTicket);
        }


        [Fact]
        public async Task DeleteTicket_ShouldNotFailForNonExistentTicket()
        {
            await _repository.DeleteTicket(999); // Non-existent ticket

            // No assertion needed, just verifying that no exception is thrown
        }

        // GetAvailableTickets Tests
        [Fact]
        public async Task GetAvailableTickets_ShouldReturnAvailableTickets()
        {
            _context.Tickets.AddRange(new List<Ticket>
    {
        new Ticket { EventId = 1, Price = 100, QuantityAvailable = 50, TicketType = "General Admission" },
        new Ticket { EventId = 1, Price = 200, QuantityAvailable = 0, TicketType = "VIP" }
    });
            await _context.SaveChangesAsync();

            var tickets = await _repository.GetAvailableTickets(1);

            Assert.Single(tickets);
            Assert.Equal(100, tickets.First().Price);
        }

        [Fact]
        public async Task GetAvailableTickets_ShouldReturnEmptyForNoAvailableTickets()
        {
            _context.Tickets.AddRange(new List<Ticket>
    {
        new Ticket { EventId = 1, Price = 100, QuantityAvailable = 0, TicketType = "General Admission" },
        new Ticket { EventId = 1, Price = 200, QuantityAvailable = 0, TicketType = "VIP" }
    });
            await _context.SaveChangesAsync();

            var tickets = await _repository.GetAvailableTickets(1);

            Assert.Empty(tickets);
        }


        // GetTicketById Tests
        [Fact]
        public async Task GetTicketById_ShouldReturnTicket()
        {
            var ticket = new Ticket { EventId = 1, Price = 100, QuantityAvailable = 50, TicketType="VIP" };
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var foundTicket = await _repository.GetTicketById(ticket.TicketId);

            Assert.Equal(ticket.TicketId, foundTicket.TicketId);
        }

        [Fact]
        public async Task GetTicketById_ShouldReturnNullForNonExistentTicket()
        {
            var foundTicket = await _repository.GetTicketById(999);

            Assert.NotNull(foundTicket); // Assuming it returns a default Ticket object
        }

        // GetTicketsByEventId Tests
        [Fact]
        public async Task GetTicketsByEventId_ShouldReturnTickets()
        {
            _context.Tickets.AddRange(new List<Ticket>
            {
                new Ticket { EventId = 1, Price = 100, QuantityAvailable = 50, TicketType= "VIP" },
                new Ticket { EventId = 2, Price = 200, QuantityAvailable = 50, TicketType = "VIP" }
            });
            await _context.SaveChangesAsync();

            var tickets = await _repository.GetTicketsByEventId(1);

            Assert.Single(tickets);
            Assert.Equal(100, tickets.First().Price);
        }

        [Fact]
        public async Task GetTicketsByEventId_ShouldReturnEmptyListForNonExistentEvent()
        {
            var tickets = await _repository.GetTicketsByEventId(999);

            Assert.Empty(tickets);
        }

        // UpdateTicket Tests
        [Fact]
        public async Task UpdateTicket_ShouldUpdateTicket()
        {
            var ticket = new Ticket { EventId = 1, Price = 100, QuantityAvailable = 50, TicketType="VIP" };
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            var request = new TicketRequestDTO
            {
                EventId = 1,
                Price = 200,
                TicketType = "Regular",
                QuantityAvailable = 100
            };

            await _repository.UpdateTicket(request, ticket.TicketId);

            var updatedTicket = await _context.Tickets.FindAsync(ticket.TicketId);
            Assert.Equal(200, updatedTicket.Price);
            Assert.Equal(100, updatedTicket.QuantityAvailable);
        }

        [Fact]
        public async Task UpdateTicket_ShouldNotUpdateNonExistentTicket()
        {
            var request = new TicketRequestDTO
            {
                EventId = 1,
                Price = 200,
                TicketType = "Regular",
                QuantityAvailable = 100
            };

            await _repository.UpdateTicket(request, 999);

            var updatedTicket = await _context.Tickets.FindAsync(999);
            Assert.Null(updatedTicket);
        }

        public void Dispose()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Tickets.RemoveRange(context.Tickets);
                context.SaveChanges();
            }
        }
    }
}
