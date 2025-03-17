using Bunit;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Rise.Client.Tickets;
using Rise.Shared.Tickets;
using Rise.Domain.Tickets;
using Rise.Shared.Movies;

namespace Rise.Client.Tickets;

public class TicketListShould : TestContext
{
        private readonly Mock<ITicketService> _ticketServiceMock;

        public TicketListShould()
        {
            _ticketServiceMock = new Mock<ITicketService>();

            Services.AddSingleton<ITicketService>(_ticketServiceMock.Object);
        }





        [Fact]
        public void ShowLoadingIndicator_WhenTicketsAreNull()
        {
            _ticketServiceMock.Setup(service => service.GetTicketsAsync())
            .ReturnsAsync((List<TicketDto>?)null);

            var cut = RenderComponent<Rise.Client.Tickets.Tickets>();

            cut.Markup.ShouldContain("Loading tickets...");

        }


        [Fact]
        public void ShowNoTicketMessage_WhenNoTicketsAreAvailable()
        {
            _ticketServiceMock.Setup(service => service.GetTicketsAsync())
                            .ReturnsAsync(new List<TicketDto>()); 

            var cut = RenderComponent<Rise.Client.Tickets.Tickets>();

            cut.Markup.ShouldContain("Je hebt nog geen tickets");
        }
        [Fact]
        public void ShowTicketList()
        {
            var ticketItems  = new List<TicketDto>
            {
                new TicketDto
                {
                DateTime = DateTime.Now,
                Location = "Cinema 1",
                Movie = new MovieDto
                {
                    Id = 1,
                    Title = "Parasite",
                    Genre = "Drama",
                    Duration = 120,
                    Description = "A poor family, the Kims, con their way into becoming the servants of a rich family, the Parks. But their easy life gets complicated when their deception is threatened with exposure.",
                    Director = "Bong Joon Ho",
                    ReleaseDate = new DateTime(2019, 10, 11),
                    MovieLink = "https://www.imdb.com/title/tt6751668/",
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
                }
                },
                new TicketDto
                {
                DateTime = DateTime.Now,
                Location = "Cinema 1",
                Movie = new MovieDto
                {
                    Id = 2,
                    Title = "Parasite",
                    Genre = "Drama",
                    Duration = 120,
                    Description = "A poor family, the Kims, con their way into becoming the servants of a rich family, the Parks. But their easy life gets complicated when their deception is threatened with exposure.",
                    Director = "Bong Joon Ho",
                    ReleaseDate = new DateTime(2019, 10, 11),
                    MovieLink = "https://www.imdb.com/title/tt6751668/",
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
                }
                },
            };

             _ticketServiceMock.Setup(service => service.GetTicketsAsync())
                .ReturnsAsync(ticketItems);

            var cut = RenderComponent<Rise.Client.Tickets.Tickets>();

            cut.WaitForState(() => cut.FindAll(".ticket-item").Count == 2, TimeSpan.FromSeconds(5));

            var ticketItemsx = cut.FindAll(".ticket-item");
            ticketItemsx.Count.ShouldBe(2);


        }


    
}
