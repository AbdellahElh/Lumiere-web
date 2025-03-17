using Microsoft.Extensions.DependencyInjection;
using Rise.Shared.Events;
using Rise.Services.Events;
using Rise.Server.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Rise.Server.Tests.Services
{
    public class EventServiceTest : IntegrationTest
    {
        private readonly IEventService _eventService;

        public EventServiceTest(CustomWebApplicationFactory fixture)
            : base(fixture)
        {
            _eventService = fixture.Services.GetRequiredService<IEventService>();
        }

        // Test filtering by event title and cinema
        [Theory]
        [InlineData("Test Event 1", "Cinema 1", 2024, 12, 5)]
        public async Task GetEvents_AllFiltersApplied_Success(string title, string cinema, int year, int month, int day)
        {
            // Creating a filter to apply title and cinema for the event
            var filters = new FiltersDataDto
            {
                SelectedDate = new DateTime(year, month, day),
                SelectedCinemas = new List<string> { cinema },
              
            };

            var result = await _eventService.GetEventAsync(filters);

            Assert.NotNull(result);
            Assert.All(result, @event =>
            {
                // Verifying that the event title contains the expected title
                Assert.Contains(title, @event.Title, StringComparison.OrdinalIgnoreCase);
                // Verifying that the event's location contains the expected cinema
                Assert.Contains(@event.Location, cinema, StringComparison.OrdinalIgnoreCase);
                // Verifying that the event date matches the expected date
                Assert.Contains(@event.Date, $"{year}-{month:D2}-{day:D2}");
            });
        }

        // Test fetching event by ID
        [Theory]
        [InlineData(1)] // Assuming event with ID 1 exists
        public async Task GetEventByIdAsync_ReturnsEvent_WhenEventExists(int eventId)
        {
            var result = await _eventService.GetEventByIdAsync(eventId);

            Assert.NotNull(result);
            // Verifying that the event ID matches the expected ID
            Assert.Equal(eventId, result.Id);
            // Assuming the title of the event with ID 1 is known
            Assert.Equal("Kerstklassiekers Marathon", result.Title);
        }

        // Test for event not found (non-existent event)
        [Theory]
        [InlineData(999)] // Assuming event with ID 999 doesn't exist
        public async Task GetEventByIdAsync_ThrowsException_WhenEventNotFound(int eventId)
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _eventService.GetEventByIdAsync(eventId));
        }
    }
}
