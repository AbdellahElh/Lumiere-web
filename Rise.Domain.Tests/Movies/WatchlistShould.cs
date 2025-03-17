using Rise.Domain.Accounts;
using Rise.Domain.Movies;
using Rise.Domain.MovieWatchlists;
using Shouldly;

namespace Rise.Domain.Tests.Movies
{
    public class WatchlistShould
    {
        [Fact]
        public void CreateWithValidProperties_Sucess()
        {
            var account = new Account { Email = "fake@user.com", Watchlist= null! };

            var watchlist = new Watchlist
            {
                UserId = 1,
                Account = account
            };


            watchlist.UserId.ShouldBe(1);
            watchlist.Account.ShouldBe(account);
        }

        [Fact]
        public void CreateWithoutAnAccount_Error()
        {
            Action act = () =>
            {
                var watchlist = new Watchlist
                {
                    UserId = 1,
                    Account = null!
                };
            };

            act.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public void AddMovieWatchlists_Sucess()
        {
            var account = new Account { Email = "fake@user.com", Watchlist = null! };
            var watchlist = new Watchlist
            {
                UserId = 1,
                Account = account
            };

            var movieWatchlist = new MovieWatchlist
            {
                MovieId = 1,
                WatchlistId = watchlist.Id,
                Movie = new Movie
                {
                    Title = "The Matrix 4",
                    Genre = "Sci-Fi",
                    Description = "A new chapter in The Matrix saga.",
                    Duration = 120,
                    Director = "Lana Wachowski",
                    ReleaseDate = new DateTime(2024, 12, 11),
                    MovieLink = "https://example.com/the-matrix-4"
                }
            };

            watchlist.MovieWatchlists.Add(movieWatchlist);

            watchlist.MovieWatchlists.Count.ShouldBe(1);
            watchlist.MovieWatchlists[0].Movie.Title.ShouldBe("The Matrix 4");
        }
    }
}
