using AutoMapper;
using EventSphere.DTOs;
using EventSphere.Models;
using EventSphere.Repositories.Interfaces;
using EventSphere.Services.Implementations;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EventSphere.Tests.Services
{
    public class AttendeeServiceTests
    {
        private readonly Mock<IAttendeeRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AttendeeService _service;

        public AttendeeServiceTests()
        {
            _repositoryMock = new Mock<IAttendeeRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new AttendeeService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldReturnAttendee_WhenAttendeeExists()
        {
            // Arrange
            var attendeeId = 1;
            var attendee = new Attendee
            {
                AttendeeId = attendeeId,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890"
            };
            var attendeeDto = new AttendeeDTO
            {
                AttendeeId = attendeeId,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890"
            };

            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeId)).ReturnsAsync(attendee);
            _mapperMock.Setup(m => m.Map<AttendeeDTO>(attendee)).Returns(attendeeDto);

            // Act
            var result = await _service.GetAttendeeById(attendeeId);

            // Assert
            Assert.Equal(attendeeDto.AttendeeId, result.AttendeeId);
            Assert.Equal(attendeeDto.Name, result.Name);
            Assert.Equal(attendeeDto.Email, result.Email);
            Assert.Equal(attendeeDto.Phone, result.Phone);
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeId), Times.Once);
            _mapperMock.Verify(m => m.Map<AttendeeDTO>(attendee), Times.Once);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldThrowKeyNotFoundException_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var attendeeId = 1;
            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeId)).ReturnsAsync((Attendee)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetAttendeeById(attendeeId));
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeId), Times.Once);
            _mapperMock.Verify(m => m.Map<AttendeeDTO>(It.IsAny<Attendee>()), Times.Never);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var attendeeId = 1;
            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAttendeeById(attendeeId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeId), Times.Once);
            _mapperMock.Verify(m => m.Map<AttendeeDTO>(It.IsAny<Attendee>()), Times.Never);
        }


        [Fact]
        public async Task GetAllAttendees_ShouldReturnEmptyList_WhenNoAttendeesExist()
        {
            // Arrange
            var attendees = new List<Attendee>();

            _repositoryMock.Setup(r => r.GetAllAttendees()).ReturnsAsync(attendees);
            _mapperMock.Setup(m => m.Map<AttendeeDTO>(It.IsAny<Attendee>()))
                       .Returns((Attendee source) => new AttendeeDTO
                       {
                           AttendeeId = source?.AttendeeId ?? 0,
                           Name = source?.Name,
                           Email = source?.Email,
                           Phone = source?.Phone
                       });

            // Act
            var result = await _service.GetAllAttendees();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _repositoryMock.Verify(r => r.GetAllAttendees(), Times.Once);
            _mapperMock.Verify(m => m.Map<AttendeeDTO>(It.IsAny<Attendee>()), Times.Never);
        }

        

        [Fact]
        public async Task GetAllAttendees_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAttendees()).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetAllAttendees());
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetAllAttendees(), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<AttendeeDTO>>(It.IsAny<IEnumerable<Attendee>>()), Times.Never);
        }

        [Fact]
        public async Task AddAttendee_ShouldCallRepositoryAddAttendee_WhenAttendeeDtoIsValid()
        {
            // Arrange
            var attendeeDto = new AttendeeDTO { AttendeeId = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" };

            // Act
            await _service.AddAttendee(attendeeDto);

            // Assert
            _repositoryMock.Verify(r => r.AddAttendee(attendeeDto), Times.Once);
        }

        [Fact]
        public async Task AddAttendee_ShouldThrowArgumentNullException_WhenAttendeeDtoIsNull()
        {
            // Arrange
            AttendeeDTO attendeeDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddAttendee(attendeeDto));
            _repositoryMock.Verify(r => r.AddAttendee(It.IsAny<AttendeeDTO>()), Times.Never);
        }

        [Fact]
        public async Task AddAttendee_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var attendeeDto = new AttendeeDTO { AttendeeId = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "1234567890" };
            _repositoryMock.Setup(r => r.AddAttendee(attendeeDto)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.AddAttendee(attendeeDto));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.AddAttendee(attendeeDto), Times.Once);
        }


        [Fact]
        public async Task UpdateAttendee_ShouldThrowArgumentNullException_WhenAttendeeDtoIsNull()
        {
            // Arrange
            AttendeeDTO attendeeDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateAttendee(attendeeDto, 1));
            _repositoryMock.Verify(r => r.UpdateAttendee(It.IsAny<AttendeeDTO>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAttendee_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var attendeeDto = new AttendeeDTO
            {
                AttendeeId = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Phone = "1234567890"
            };
            var attendeeId = attendeeDto.AttendeeId;

            _repositoryMock.Setup(r => r.UpdateAttendee(attendeeDto, attendeeId))
                           .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.UpdateAttendee(attendeeDto, attendeeId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.UpdateAttendee(attendeeDto, attendeeId), Times.Once);
        }


        [Fact]
        public async Task DeleteAttendee_ShouldCallRepositoryDeleteAttendee_WhenAttendeeIdIsValid()
        {
            // Arrange
            var attendeeId = 1;
            var attendee = new Attendee { AttendeeId = attendeeId, Name = "John Doe" };

            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeId)).ReturnsAsync(attendee);

            // Act
            await _service.DeleteAttendee(attendeeId);

            // Assert
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAttendee(attendeeId), Times.Once);
        }

        [Fact]
        public async Task DeleteAttendee_ShouldPropagateException_WhenRepositoryThrowsException()
        {
            // Arrange
            var attendeeId = 1;
            var attendee = new Attendee { AttendeeId = attendeeId, Name = "John Doe" };

            _repositoryMock.Setup(r => r.GetAttendeeById(attendeeId)).ReturnsAsync(attendee);
            _repositoryMock.Setup(r => r.DeleteAttendee(attendeeId)).ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.DeleteAttendee(attendeeId));
            Assert.Equal("Database error", exception.Message);
            _repositoryMock.Verify(r => r.GetAttendeeById(attendeeId), Times.Once);
            _repositoryMock.Verify(r => r.DeleteAttendee(attendeeId), Times.Once);
        }
    }
}
