using Bunit;
using Xunit;
using Rise.Shared.Events;
using Rise.Shared.Movies;
using Rise.Client.events.components;
using System.Collections.Generic;
using System;

namespace Rise.Client.Tests.Events
{
    public class EventInfoBannerTests : TestContext
    {
        [Fact]
        public void Renders_Event_Info_Correctly()
        {
            // Arrange: Dummy event data met correcte MoviePosterDto
            var eventDto = new EventDto
            {
                Id = 1,
                Title = "Film Festival 2024",
                Genre = "Drama",
                Type = "Public Event",
                Description = "Een festival vol prachtige films en meer!",
                Cover = "https://example.com/event-cover.jpg",
                Price = "€20",
                Movies = new List<MoviePosterDto>
                {
                    new MoviePosterDto
                    {
                        Id = 101,
                        Cover = "https://example.com/movie1-cover.jpg",
                        ReleaseDate = new DateTime(2024, 12, 1)
                    },
                    new MoviePosterDto
                    {
                        Id = 102,
                        Cover = "https://example.com/movie2-cover.jpg",
                        ReleaseDate = new DateTime(2024, 12, 2)
                    }
                }
            };

            // Act: Render de component
            var cut = RenderComponent<EventInfoBanner>(parameters => parameters
                .Add(p => p.Event, eventDto)
            );

            // Assert: Controleer de algemene gegevens van het event
            var markup = cut.Markup;
            Assert.Contains("Film Festival 2024", markup);
            Assert.Contains("Drama", markup);
            Assert.Contains("Public Event", markup);
            Assert.Contains("Een festival vol prachtige films en meer!", markup);

            // Assert: Controleer dat de movie covers worden weergegeven
            foreach (var movie in eventDto.Movies)
            {
                Assert.Contains(movie.Cover, markup);
            }
        }
    }
}
