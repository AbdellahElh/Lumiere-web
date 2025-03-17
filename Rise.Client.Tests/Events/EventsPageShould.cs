using Bunit;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Rise.Client.events;
using Rise.Client.events.components;
using Rise.Shared.Events;
using Rise.Shared.Movies;

namespace Rise.Client.Tests.Events
{
    public class EventsPageShould : TestContext
    {
        private readonly Mock<IEventService> _eventServiceMock;

        public EventsPageShould()
        {
            _eventServiceMock = new Mock<IEventService>();
            
            Services.AddSingleton<IEventService>(_eventServiceMock.Object);
        }

        

        

        [Fact]
        public void ShowLoadingIndicator_WhenEventsAreNull()
        {
            _eventServiceMock.Setup(service => service.GetEventAsync(It.IsAny<Rise.Shared.Events.FiltersDataDto>()))
            .ReturnsAsync((List<EventDto>?)null);

            var cut = RenderComponent<Rise.Client.events.Events>();

            var loader = cut.Find("[data-cy='loader']");
             loader.ToMarkup().ShouldContain("Loading events...");
        }

        
        [Fact]
        public void ShowNoEventsMessage_WhenNoEventsAreAvailable()
        {
            _eventServiceMock.Setup(service => service.GetEventAsync(It.IsAny<Rise.Shared.Events.FiltersDataDto>()))
                            .ReturnsAsync(new List<EventDto>()); 

            var cut = RenderComponent<Rise.Client.events.Events>();

            cut.Markup.ShouldContain("Geen Events beschikbaar.");
        }
        [Fact]
        public void ShowEventsList()
        {
            var eventItems  = new List<EventDto>
            {
                new EventDto
                {
                    Id = 1,
                    Title = "The Matrix",
                    Genre = "Fantasy",
                    Type = "Film Marathon",
                    Price = "12.34",
                    Description = "Een computerhacker leert van mysterieuze rebellen over de ware aard van zijn werkelijkheid en zijn rol in de oorlog tegen de beheerders ervan.",
                    Duration = "335",
                    Director = "Lana Wachowski",
                    Cast = ["Lana Wachowski", "Lilly Wachowski"],
                
                    VideoPlaceholderImage = "https://riseopslag2425.blob.core.windows.net/images/ParasiteVideoImage.webp",
                    Cover = "https://riseopslag2425.blob.core.windows.net/images/ParasiteCover.webp",
                    Video = "https://riseopslag2425.blob.core.windows.net/images/ParasiteVideo.mp4",
                    Cinemas = new List<CinemaDto>
                    {
                        new CinemaDto
                        {
                            Name = "Cinema 1",
                            Showtimes = new List<DateTime>
                            {
                                DateTime.Now.AddHours(1),
                                DateTime.Now.AddHours(3)
                            }
                        }
                    }
                },
                new EventDto
                {
                    Id = 2,
                    Title = "The Matrix",
                    Genre = "Fantasy",
                    Type = "Film Marathon",
                    Price = "12.34",
                    Description = "Een computerhacker leert van mysterieuze rebellen over de ware aard van zijn werkelijkheid en zijn rol in de oorlog tegen de beheerders ervan.",
                    Duration = "335",
                    Director = "Lana Wachowski",
                    Cast = ["Lana Wachowski", "Lilly Wachowski"],
                
                    VideoPlaceholderImage = "https://riseopslag2425.blob.core.windows.net/images/ParasiteVideoImage.webp",
                    Cover = "https://riseopslag2425.blob.core.windows.net/images/ParasiteCover.webp",
                    Video = "https://riseopslag2425.blob.core.windows.net/images/ParasiteVideo.mp4",
                    Cinemas = new List<CinemaDto>
                    {
                        new CinemaDto
                        {
                            Name = "Cinema 2",
                            Showtimes = new List<DateTime>
                            {
                                DateTime.Now.AddHours(1),
                                DateTime.Now.AddHours(3)
                            }
                        }
                    }
                },
            };

            var cut = RenderComponent<Rise.Client.events.Events>(parameters => parameters.Add(p => p.EventsList, eventItems));
             var eventItemsx = cut.FindAll(".event-item");
            eventItems.Count.ShouldBe(2);

           
        }

       
    }
}
