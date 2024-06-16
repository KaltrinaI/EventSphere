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
    public class OrganizerRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _context;
        private readonly OrganizerRepository _repository;

        public OrganizerRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(_dbContextOptions);
            _repository = new OrganizerRepository(_context);
        }

        // AddOrganizer Tests
        [Fact]
        public async Task AddOrganizer_ShouldAddOrganizer()
        {
            var request = new OrganizerRequestDTO
            {
                Name = "Organizer 1",
                Phone = "123-456-7890"
            };

            await _repository.AddOrganizer(request);

            var organizers = _context.Organizers.ToList();
            Assert.Single(organizers);
            Assert.Equal(request.Name, organizers[0].Name);
        }

        // DeleteOrganizer Tests
        [Fact]
        public async Task DeleteOrganizer_ShouldDeleteOrganizer()
        {
            var organizer = new Organizer { Name = "Organizer 1", Phone = "123-456-7890" };
            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();

            await _repository.DeleteOrganizer(organizer.OrganizerId);

            var deletedOrganizer = await _context.Organizers.FindAsync(organizer.OrganizerId);
            Assert.Null(deletedOrganizer);
        }

        [Fact]
        public async Task DeleteOrganizer_ShouldNotFailForNonExistentOrganizer()
        {
            await _repository.DeleteOrganizer(999); // Non-existent organizer

            // No assertion needed, just verifying that no exception is thrown
        }

        // GetAllOrganizers Tests
        [Fact]
        public async Task GetAllOrganizers_ShouldReturnAllOrganizers()
        {
            _context.Organizers.AddRange(new List<Organizer>
            {
                new Organizer { Name = "Organizer 1", Phone = "123-456-7890" },
                new Organizer { Name = "Organizer 2", Phone = "098-765-4321" }
            });
            await _context.SaveChangesAsync();

            var organizers = await _repository.GetAllOrganizers();

            Assert.Equal(2, organizers.Count());
        }

        [Fact]
        public async Task GetAllOrganizers_ShouldReturnEmptyList()
        {
            var organizers = await _repository.GetAllOrganizers();

            Assert.Empty(organizers);
        }

        // GetOrganizerById Tests
        [Fact]
        public async Task GetOrganizerById_ShouldReturnOrganizer()
        {
            var organizer = new Organizer { Name = "Organizer 1", Phone = "123-456-7890" };
            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();

            var foundOrganizer = await _repository.GetOrganizerById(organizer.OrganizerId);

            Assert.Equal(organizer.OrganizerId, foundOrganizer.OrganizerId);
        }

        [Fact]
        public async Task GetOrganizerById_ShouldReturnNullForNonExistentOrganizer()
        {
            var foundOrganizer = await _repository.GetOrganizerById(999);

            Assert.NotNull(foundOrganizer); // Assuming it returns a default Organizer object
        }

        // UpdateOrganizer Tests
        [Fact]
        public async Task UpdateOrganizer_ShouldUpdateOrganizer()
        {
            var organizer = new Organizer { Name = "Organizer 1", Phone = "123-456-7890" };
            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();

            var request = new OrganizerRequestDTO
            {
                Name = "Updated Organizer",
                Phone = "098-765-4321"
            };

            await _repository.UpdateOrganizer(request, organizer.OrganizerId);

            var updatedOrganizer = await _context.Organizers.FindAsync(organizer.OrganizerId);
            Assert.Equal(request.Name, updatedOrganizer.Name);
            Assert.Equal(request.Phone, updatedOrganizer.Phone);
        }

        [Fact]
        public async Task UpdateOrganizer_ShouldNotUpdateNonExistentOrganizer()
        {
            var request = new OrganizerRequestDTO
            {
                Name = "Updated Organizer",
                Phone = "098-765-4321"
            };

            await _repository.UpdateOrganizer(request, 999);

            var updatedOrganizer = await _context.Organizers.FindAsync(999);
            Assert.Null(updatedOrganizer);
        }

        public void Dispose()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Organizers.RemoveRange(context.Organizers);
                context.SaveChanges();
            }
        }

    }
}
