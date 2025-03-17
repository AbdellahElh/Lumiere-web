using Rise.Domain.Movies;
using Shouldly;
namespace Rise.Domain.Tests.Movies;

public class MovieShould
{
    [Fact]
    public void BeCreatedWithValidProperties()
    {

        var movie = new Movie
        {
            Title = "The Matrix 4",
            Genre = "Sci-Fi",
            Description = "A new chapter in The Matrix saga.",
            Duration = 120,
            Director = "Lana Wachowski",
            ReleaseDate = new DateTime(2024, 12, 11),
            MovieLink = "https://example.com/the-matrix-4"
        };

        movie.Title.ShouldBe("The Matrix 4");
        movie.Genre.ShouldBe("Sci-Fi");
        movie.Description.ShouldBe("A new chapter in The Matrix saga.");
        movie.Duration.ShouldBe(120);
        movie.Director.ShouldBe("Lana Wachowski");
        movie.ReleaseDate.ShouldBe(new DateTime(2024, 12, 11));
        movie.MovieLink.ShouldBe("https://example.com/the-matrix-4");
    }


    [Fact]
    public void BeAbleToAddShowtimes()
    {
        var movie = new Movie
        {
            Title = "The Matrix 4",
            Genre = "Sci-Fi",
            Description = "A new chapter in The Matrix saga.",
            Duration = 120,
            Director = "Lana Wachowski",
            ReleaseDate = new DateTime(2024, 12, 11),
            MovieLink = "https://example.com/the-matrix-4"
        };

        var cinema = new Cinema { Location = "Brugge", Name="Brugge" };

        movie.Showtimes.Add(new Showtime { ShowTime = new DateTime(2024, 12, 11, 19, 0, 0),CinemaId=cinema.Id });

        movie.Showtimes.Count.ShouldBe(1);
        movie.Showtimes[0].ShowTime.ShouldBe(new DateTime(2024, 12, 11, 19, 0, 0));
    }

}

