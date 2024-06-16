using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventSphere.Controllers;
using EventSphere.Services.Interfaces;
using EventSphere.DTOs;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace EventSphere
{
    public class AttendeeControllerTests
    {
        private readonly Mock<IAttendeeService> _serviceMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly AttendeeController _controller;

        public AttendeeControllerTests()
        {
            _serviceMock = new Mock<IAttendeeService>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _controller = new AttendeeController(_serviceMock.Object, _memoryCacheMock.Object);
        }

        [Fact]
        public async Task GetAllAttendees_ShouldReturnOkResult_WithListOfAttendees()
        {
            // Arrange
            var attendees = new List<AttendeeDTO>
            {
                new AttendeeDTO { AttendeeId = 1, Name = "John Doe" },
                new AttendeeDTO { AttendeeId = 2, Name = "Jane Smith" }
            };

            object cacheEntry;
            _memoryCacheMock.Setup(m => m.TryGetValue("AllAttendees", out cacheEntry)).Returns(false);
            _serviceMock.Setup(s => s.GetAllAttendees()).ReturnsAsync(attendees);
            _memoryCacheMock.Setup(m => m.CreateEntry("AllAttendees")).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = await _controller.GetAllAttendees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<AttendeeDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _serviceMock.Verify(s => s.GetAllAttendees(), Times.Once);
        }

        [Fact]
        public async Task GetAllAttendees_ShouldReturnCachedAttendees_WhenCacheExists()
        {
            // Arrange
            var attendees = new List<AttendeeDTO>
            {
                new AttendeeDTO { AttendeeId = 1, Name = "John Doe" },
                new AttendeeDTO { AttendeeId = 2, Name = "Jane Smith" }
            };

            object cacheEntry = attendees;
            _memoryCacheMock.Setup(m => m.TryGetValue("AllAttendees", out cacheEntry)).Returns(true);

            // Act
            var result = await _controller.GetAllAttendees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<AttendeeDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _serviceMock.Verify(s => s.GetAllAttendees(), Times.Never);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldReturnOkResult_WithAttendee_WhenAttendeeExists()
        {
            // Arrange
            var attendeeId = 1;
            var attendeeDto = new AttendeeDTO { AttendeeId = attendeeId, Name = "John Doe" };

            object cacheEntry;
            _memoryCacheMock.Setup(m => m.TryGetValue("attendee", out cacheEntry)).Returns(false);
            _serviceMock.Setup(s => s.GetAttendeeById(attendeeId)).ReturnsAsync(attendeeDto);
            _memoryCacheMock.Setup(m => m.CreateEntry("attendee")).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = await _controller.GetAttendeeById(attendeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<AttendeeDTO>(okResult.Value);
            Assert.Equal(attendeeId, returnValue.AttendeeId);
            _serviceMock.Verify(s => s.GetAttendeeById(attendeeId), Times.Once);
        }

        [Fact]
        public async Task GetAttendeeById_ShouldReturnCachedAttendee_WhenCacheExists()
        {
            // Arrange
            var attendeeId = 1;
            var attendeeDto = new AttendeeDTO { AttendeeId = attendeeId, Name = "John Doe" };

            object cacheEntry = attendeeDto;
            _memoryCacheMock.Setup(m => m.TryGetValue("attendee", out cacheEntry)).Returns(true);

            // Act
            var result = await _controller.GetAttendeeById(attendeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<AttendeeDTO>(okResult.Value);
            Assert.Equal(attendeeId, returnValue.AttendeeId);
            _serviceMock.Verify(s => s.GetAttendeeById(attendeeId), Times.Never);
        }

        [Fact]
        public async Task AddAttendee_ShouldReturnOkResult_WhenAttendeeIsAdded()
        {
            // Arrange
            var attendeeDto = new AttendeeDTO { AttendeeId = 1, Name = "John Doe" };

            // Act
            var result = await _controller.AddAttendee(attendeeDto);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.AddAttendee(attendeeDto), Times.Once);
        }

        [Fact]
        public async Task AddAttendee_ShouldThrowArgumentNullException_WhenAttendeeDtoIsNull()
        {
            // Arrange
            AttendeeDTO attendeeDto = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.AddAttendee(attendeeDto));
            _serviceMock.Verify(s => s.AddAttendee(It.IsAny<AttendeeDTO>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAttendee_ShouldReturnOkResult_WhenAttendeeIsDeleted()
        {
            // Arrange
            var attendeeId = 1;

            // Act
            var result = await _controller.DeleteAttendee(attendeeId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.DeleteAttendee(attendeeId), Times.Once);
        }

        [Fact]
        public async Task DeleteAttendee_ShouldReturnNotFound_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var attendeeId = 1;

            _serviceMock.Setup(s => s.DeleteAttendee(attendeeId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteAttendee(attendeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(s => s.DeleteAttendee(attendeeId), Times.Once);
        }

        [Fact]
        public async Task UpdateAttendee_ShouldReturnOkResult_WhenAttendeeIsUpdated()
        {
            // Arrange
            var attendeeId = 1;
            var attendeeDto = new AttendeeDTO { AttendeeId = attendeeId, Name = "John Doe" };

            // Act
            var result = await _controller.UpdateAttendee(attendeeDto, attendeeId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.UpdateAttendee(attendeeDto, attendeeId), Times.Once);
        }

        

        [Fact]
        public async Task UpdateAttendee_ShouldReturnNotFound_WhenAttendeeDoesNotExist()
        {
            // Arrange
            var attendeeId = 1;
            var attendeeDto = new AttendeeDTO { AttendeeId = attendeeId, Name = "John Doe" };

            _serviceMock.Setup(s => s.UpdateAttendee(attendeeDto, attendeeId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.UpdateAttendee(attendeeDto, attendeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(s => s.UpdateAttendee(attendeeDto, attendeeId), Times.Once);
        }

       

        [Fact]
        public async Task GetAttendeesByEvent_ShouldReturnOkResult_WithListOfAttendees()
        {
            // Arrange
            var eventId = 1;
            var attendees = new List<AttendeeDTO>
            {
                new AttendeeDTO { AttendeeId = 1, Name = "John Doe" },
                new AttendeeDTO { AttendeeId = 2, Name = "Jane Smith" }
            };

            object cacheEntry;
            _memoryCacheMock.Setup(m => m.TryGetValue("attendeesByEvent", out cacheEntry)).Returns(false);
            _serviceMock.Setup(s => s.GetAttendeesByEvent(eventId)).ReturnsAsync(attendees);
            _memoryCacheMock.Setup(m => m.CreateEntry("attendeesByEvent")).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = await _controller.GetAttendeesByEvent(eventId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<AttendeeDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _serviceMock.Verify(s => s.GetAttendeesByEvent(eventId), Times.Once);
        }

        [Fact]
        public async Task GetAttendeesByEvent_ShouldReturnCachedAttendees_WhenCacheExists()
        {
            // Arrange
            var eventId = 1;
            var attendees = new List<AttendeeDTO>
            {
                new AttendeeDTO { AttendeeId = 1, Name = "John Doe" },
                new AttendeeDTO { AttendeeId = 2, Name = "Jane Smith" }
            };

            object cacheEntry = attendees;
            _memoryCacheMock.Setup(m => m.TryGetValue("attendeesByEvent", out cacheEntry)).Returns(true);

            // Act
            var result = await _controller.GetAttendeesByEvent(eventId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<AttendeeDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _serviceMock.Verify(s => s.GetAttendeesByEvent(eventId), Times.Never);
        }
    }
}
