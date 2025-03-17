using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Rise.Shared.Movies;
using System.Collections.Generic;
using System;
using Xunit;
using Rise.Client.Movies.components;
namespace Rise.Client.Movies;
public class MovieDetailTests : TestContext
{
    [Fact]
    public void MovieComponent_RendersCorrectly()
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
        ReleaseDate = new DateTime(2023, 1, 1),
        MovieLink = "https://sample-movie-link.com",
        Cast = new List<string> { "Actor 1", "Actor 2" },
        Cinemas = new List<CinemaDto> { new CinemaDto { Name = "Cinema 1" }, new CinemaDto { Name = "Cinema 2" } }
        };


        var cut = RenderComponent<MovieInfoBanner>(parameters => parameters
            .Add(p => p.Movie, movie)
        );


        cut.Find("h5").MarkupMatches("<h5 class=\"text-lg font-bold underline mb-2\">Sample Movie</h5>");
        cut.Find("p").MarkupMatches("<p class=\"text-sm leading-6\">This is a sample movie description.</p>");
        cut.Find("img").MarkupMatches("<img src=\"https://placekitten.com/180/270\" alt=\"Sample Movie\" class=\"max-w-[180px] h-auto mb-4 sm:mb-0 sm:mr-4 mx-auto sm:mx-0 shadow-md\" />");
    }
    [Fact]
    public void BuyTicketsButton_NavigatesToShowtimesSection()
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
            ReleaseDate = new DateTime(2023, 1, 1),
            MovieLink = "https://sample-movie-link.com",
            Cast = new List<string> { "Actor 1", "Actor 2" },
            Cinemas = new List<CinemaDto> { new CinemaDto { Name = "Cinema 1" }, new CinemaDto { Name = "Cinema 2" } }
        };

        var navigationManager = Services.GetRequiredService<NavigationManager>();

        var cut = RenderComponent<MovieInfoBanner>(parameters => parameters
            .Add(p => p.Movie, movie)
        );


        var buyTicketsButton = cut.Find("button");
        buyTicketsButton.Click();


        Assert.Contains("#showtimes-section", navigationManager.Uri);
    }

}
