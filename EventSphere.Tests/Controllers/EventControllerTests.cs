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
    public class EventControllerTests
    {
        private readonly Mock<IEventService> _serviceMock;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _serviceMock = new Mock<IEventService>();
            _controller = new EventController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnOkResult_WithListOfEvents()
        {
            // Arrange
            var events = new List<EventDTO>
            {
                new EventDTO { EventId = 1, Name = "Event 1" },
                new EventDTO { EventId = 2, Name = "Event 2" }
            };

            _serviceMock.Setup(s => s.GetAllEvents()).ReturnsAsync(events);

            // Act
            var result = await _controller.GetAllEvents();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<EventDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _serviceMock.Verify(s => s.GetAllEvents(), Times.Once);
        }

        [Fact]
        public async Task GetEventById_ShouldReturnOkResult_WithEvent_WhenEventExists()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventDTO { EventId = eventId, Name = "Event 1" };

            _serviceMock.Setup(s => s.GetEventById(eventId)).ReturnsAsync(eventDto);

            // Act
            var result = await _controller.GetEventById(eventId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<EventDTO>(okResult.Value);
            Assert.Equal(eventId, returnValue.EventId);
            _serviceMock.Verify(s => s.GetEventById(eventId), Times.Once);
        }


        [Fact]
        public async Task AddEvent_ShouldReturnOkResult_WhenEventIsAdded()
        {
            // Arrange
            var eventDto = new EventRequestDTO { Name = "Event 1" };

            // Act
            var result = await _controller.AddEvent(eventDto);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.AddEvent(eventDto), Times.Once);
        }

        [Fact]
        public async Task AddEvent_ShouldReturnBadRequest_WhenEventDtoIsNull()
        {
            // Arrange
            EventRequestDTO eventDto = null;

            // Act
            var result = await _controller.AddEvent(eventDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("EventRequestDTO object is null", badRequestResult.Value);
            _serviceMock.Verify(s => s.AddEvent(It.IsAny<EventRequestDTO>()), Times.Never);
        }

        [Fact]
        public async Task DeleteEvent_ShouldReturnOkResult_WhenEventIsDeleted()
        {
            // Arrange
            var eventId = 1;

            // Act
            var result = await _controller.DeleteEvent(eventId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.DeleteEvent(eventId), Times.Once);
        }

        [Fact]
        public async Task DeleteEvent_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;

            _serviceMock.Setup(s => s.DeleteEvent(eventId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteEvent(eventId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(s => s.DeleteEvent(eventId), Times.Once);
        }

        [Fact]
        public async Task UpdateEvent_ShouldReturnOkResult_WhenEventIsUpdated()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventRequestDTO { Name = "Updated Event" };

            // Act
            var result = await _controller.UpdateEvent(eventDto, eventId);

            // Assert
            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.UpdateEvent(eventDto, eventId), Times.Once);
        }

        [Fact]
        public async Task UpdateEvent_ShouldReturnNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = 1;
            var eventDto = new EventRequestDTO { Name = "Updated Event" };

            _serviceMock.Setup(s => s.UpdateEvent(eventDto, eventId)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.UpdateEvent(eventDto, eventId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _serviceMock.Verify(s => s.UpdateEvent(eventDto, eventId), Times.Once);
        }

        [Fact]
        public async Task GetEventsByOrganizerId_ShouldReturnOkResult_WithListOfEvents()
        {
            // Arrange
            var organizerId = 1;
            var events = new List<EventDTO>
            {
                new EventDTO { EventId = 1, Name = "Event 1" },
                new EventDTO { EventId = 2, Name = "Event 2" }
            };

            _serviceMock.Setup(s => s.GetEventsByOrganizerId(organizerId)).ReturnsAsync(events);

            // Act
            var result = await _controller.GetEventsByOrganizerId(organizerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<EventDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _serviceMock.Verify(s => s.GetEventsByOrganizerId(organizerId), Times.Once);
        }

        [Fact]
        public async Task GetUpcomingEventsSortedByPopularity_ShouldReturnOkResult_WithListOfEvents()
        {
            // Arrange
            var events = new List<EventDTO>
            {
                new EventDTO { EventId = 1, Name = "Event 1" },
                new EventDTO { EventId = 2, Name = "Event 2" }
            };

            _serviceMock.Setup(s => s.GetUpcomingEventsSortedByPopularity()).ReturnsAsync(events);

            // Act
            var result = await _controller.GetUpcomingEventsSortedByPopularity();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<EventDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            _serviceMock.Verify(s => s.GetUpcomingEventsSortedByPopularity(), Times.Once);
        }
       
    }
}
