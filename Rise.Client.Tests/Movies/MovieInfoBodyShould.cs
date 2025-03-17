using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Rise.Shared.Movies;
using Xunit;
using System;
using System.Collections.Generic;
using Rise.Client.Movies.components;
using Rise.Client.Tickets.services;
using Rise.Shared.Tickets;
using Microsoft.AspNetCore.Components.Authorization;
using Rise.Client.Tests.Account;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Rise.Client.Movies;


public class MovieInfoBodyTests : TestContext
{
      public MovieInfoBodyTests()
    {
        Services.AddScoped<ITicketService, TicketService>();

        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, "testuser2@test.be")
            }, "mock"))));

        Services.AddSingleton<AuthenticationStateProvider>(provider => new MockAuthenticationStateProvider(authState));
    }

    [Fact]
    public void MovieInfoBody_RendersCinemaNamesAndShowtimes()
    {

        var movie = new MovieDto
        {
            Id = 1,
            Title = "Sample Movie",
            Description = "This is a sample movie description.",
            CoverImageUrl = "https://placekitten.com/180/270",
            Genre = "Action",
            Duration = 120,
            Director = "Sample Director",
            VideoPlaceholderUrl = "https://placekitten.com/640/360",
            Cast = new List<string> { "Actor 1", "Actor 2" },
            ReleaseDate = new DateTime(2023, 10, 29),
            MovieLink = "https://sample-movie-link.com",
            Cinemas = new List<CinemaDto>
            {
                new CinemaDto
                {
                    Name = "Brugge",
                    Showtimes = new List<DateTime>
                    {
                        new DateTime(2023, 10, 29, 10, 0, 0),
                        new DateTime(2023, 10, 29, 13, 0, 0)
                    }
                },
                new CinemaDto
                {
                    Name = "Mechelen",
                    Showtimes = new List<DateTime>()
                },
                new CinemaDto
                {
                    Name = "Antwerpen",
                    Showtimes = new List<DateTime>
                    {
                        new DateTime(2023, 10, 29, 15, 0, 0),
                        new DateTime(2023, 10, 29, 18, 0, 0)
                    }
                },
                new CinemaDto
                {
                    Name = "Cinema Cartoons",
                    Showtimes = new List<DateTime>()
                }
            }
        };


        var cut = RenderComponent<MovieInfoBody>(parameters => parameters
            .Add(p => p.Movie, movie)
        );

        cut.Find("tr:nth-child(1) .cinema-name").MarkupMatches("<td class=\"cinema-name font-bold text-left\">Brugge</td>");
        cut.Find("tr:nth-child(2) .cinema-name").MarkupMatches("<td class=\"cinema-name font-bold text-left\">Mechelen</td>");
        cut.Find("tr:nth-child(3) .cinema-name").MarkupMatches("<td class=\"cinema-name font-bold text-left\">Antwerpen</td>");
        cut.Find("tr:nth-child(4) .cinema-name").MarkupMatches("<td class=\"cinema-name font-bold text-left\">Cinema Cartoons</td>");

        var bruggeShowtimes = cut.Find("tr:nth-child(1) .cinema-showtimes");
        var mechelenShowtimes = cut.Find("tr:nth-child(2) .cinema-showtimes");
        var antwerpenShowtimes = cut.Find("tr:nth-child(3) .cinema-showtimes");
        var cartoonsShowtimes = cut.Find("tr:nth-child(4) .cinema-showtimes");

        bruggeShowtimes.MarkupMatches(@"
            <td class=""cinema-showtimes min-w-[200px] text-left"">
                <span class=""showtime-buttons"">
                    <button class=""showtime-button shadow-md rounded-md px-2 py-1 m-1 text-sm cursor-pointer transition duration-300 ease-in-out bg-red-800 text-white hover:opacity-80"">
                        <span>10:00</span>
                    </button>
                    <button class=""showtime-button shadow-md rounded-md px-2 py-1 m-1 text-sm cursor-pointer transition duration-300 ease-in-out bg-red-800 text-white hover:opacity-80"">
                        <span>13:00</span>
                    </button>
                </span>
            </td>
        ");

        mechelenShowtimes.MarkupMatches("<td class=\"cinema-showtimes min-w-[200px] text-left\"><span class=\"no-showtimes ml-1 text-gray-500 italic text-sm\">- Niet beschikbaar</span></td>");

        antwerpenShowtimes.MarkupMatches(@"
            <td class=""cinema-showtimes min-w-[200px] text-left"">
                <span class=""showtime-buttons"">
                    <button class=""showtime-button shadow-md rounded-md px-2 py-1 m-1 text-sm cursor-pointer transition duration-300 ease-in-out bg-black text-white hover:opacity-80"">
                        <span>15:00</span>
                    </button>
                    <button class=""showtime-button shadow-md rounded-md px-2 py-1 m-1 text-sm cursor-pointer transition duration-300 ease-in-out bg-black text-white hover:opacity-80"">
                        <span>18:00</span>
                    </button>
                </span>
            </td>
        ");

        cartoonsShowtimes.MarkupMatches("<td class=\"cinema-showtimes min-w-[200px] text-left\"><span class=\"no-showtimes ml-1 text-gray-500 italic text-sm\">- Niet beschikbaar</span></td>");
    }
}
