using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Rise.Shared.Movies;
using System.Collections.Generic;
using System;
using Xunit;
using Rise.Client.Movies.components;
namespace Rise.Client.Movies;
public class MovieVideoComponentTests : TestContext
{
    [Fact]
    public void MovieVideoComponent_DisplaysPlaceholderInitially()
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
            ReleaseDate = DateTime.Now,
            MovieLink = "https://sample-movie-link.com",
                      
            Cast = new List<string> { "Actor 1", "Actor 2" },
            Cinemas = new List<CinemaDto> { new CinemaDto { Name = "Cinema 1" }, new CinemaDto { Name = "Cinema 2" } }
        };


        var cut = RenderComponent<MovieVideo>(parameters => parameters
            .Add(p => p.Movie, movie)
        );


        cut.Find("img").MarkupMatches($@"
            <img src=""{movie.VideoPlaceholderUrl}"" alt=""Video Placeholder"" class=""w-full h-104 object-cover"" />");

        cut.Find("i.fas.fa-play").MarkupMatches(@"<i class=""fas fa-play text-white text-4xl transition-transform duration-200 transform hover:scale-110""></i>");
    }

    [Fact]
    public void MovieVideoComponent_ShowsVideoOnPlayButtonClick()
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
            ReleaseDate = DateTime.Now,
            MovieLink = "https://sample-movie-link.com",
          
            Cast = new List<string> { "Actor 1", "Actor 2" },
            Cinemas = new List<CinemaDto> { new CinemaDto { Name = "Cinema 1" }, new CinemaDto { Name = "Cinema 2" } }
        };

        var cut = RenderComponent<MovieVideo>(parameters => parameters
            .Add(p => p.Movie, movie)
        );

        var playButton = cut.Find(".cursor-pointer");
        playButton.Click();

        cut.Find("iframe").MarkupMatches($@"
            <iframe class=""w-full h-full""
                    src=""{movie.MovieLink}?controls=1&autoplay=1&playsinline=1&rel=0""
                    title=""Movie video player""
                    frameborder=""0""
                    allow=""accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture""
                    allowfullscreen>
            </iframe>");
    }
}
