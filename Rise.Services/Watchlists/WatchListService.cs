using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Movies;
using Rise.Domain.Movies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rise.Services.Auth;
using Rise.Domain.Accounts;
using Rise.Domain.Exceptions;

namespace Rise.Services.Watchlists
{
    public class WatchListService : IWatchlistService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IAuthContextProvider authContextProvider;

        public WatchListService(ApplicationDbContext dbContext, IAuthContextProvider authContextProvider)
        {
            this.dbContext = dbContext;
            this.authContextProvider = authContextProvider;
        }
         private Account GetLoggedInUser()
        {
        var accountId = authContextProvider.GetAccountIdFromMetadata(authContextProvider.User!);
      
        var account = dbContext.Accounts.FirstOrDefault(a => a.Id == accountId);
        if (account is null)
            throw new ArgumentNullException($"{nameof(WatchListService)} requires a {nameof(account)}");
        return account;
        }
        public async Task<List<MovieDto>> LoadWatchlistAsync()
        {
                   var account = GetLoggedInUser();


            var today = DateTime.Today;

            var watchlist = await dbContext.Watchlists
                .Include(w => w.MovieWatchlists)
                    .ThenInclude(mw => mw.Movie)
                        .ThenInclude(m => m.Cinemas)
                            .ThenInclude(c => c.Showtimes)
                .FirstOrDefaultAsync(w => w.UserId == account.Id);

            if (watchlist?.MovieWatchlists == null)
            {
                return new List<MovieDto>();
            }

            var movieDtos = watchlist.MovieWatchlists
                .Select(mw => mw.Movie)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    CoverImageUrl = m.CoverImageUrl,
                    Genre = m.Genre,
                    Director = m.Director,
                    Cast = m.Cast,
                    ReleaseDate = m.ReleaseDate,
                    MovieLink = m.MovieLink,
                    Duration = m.Duration,
                    Cinemas = m.Cinemas
                        .Select(c => new CinemaDto
                        {
                            Name = c.Name,
                            Showtimes = c.Showtimes
                                .Where(st => st.MovieId == m.Id && st.ShowTime.Date == today)
                                .Select(st => st.ShowTime)
                                .ToList()
                        })
                        .Where(c => c.Showtimes.Any())
                        .ToList()
                })
                .Where(movie => movie.Cinemas.Any(c => c.Showtimes.Any()))
                .ToList();

            return movieDtos;
        }


        public async Task AddToWatchlistAsync(MovieDto movie)
        {
            var accountId = authContextProvider.GetAccountIdFromMetadata(authContextProvider.User!);
            var account = await dbContext.Accounts.FirstAsync(a => a.Id == accountId);


            account = await dbContext.Accounts
            .Include(a => a.Watchlist)
            .ThenInclude(w => w.Movies)
            .FirstOrDefaultAsync(a => a.Id == account.Id)
            ?? throw new EntityNotFoundException("Account");

            // if (account.Watchlist == null!)
            // {   
            //     account.Watchlist = new Watchlist { UserId = account.Id, Account = account };
            //     dbContext.Watchlists.Add(account.Watchlist);
            // }

            if (account.Watchlist.Movies.Any(m => m.Id == movie.Id))
            {
                throw new EntityAlreadyExistsException("Movie");
            }

            var movieEntity = await dbContext.Movies.FindAsync(movie.Id)
                              ?? throw new EntityNotFoundException("Movie");

            account.Watchlist.Movies.Add(movieEntity);
            await dbContext.SaveChangesAsync();
        }


        public async Task RemoveFromWatchlistAsync(int movieId)
        {
            var accountId = authContextProvider.GetAccountIdFromMetadata(authContextProvider.User!);
            var account = await dbContext.Accounts.FirstAsync(a => a.Id == accountId);


            account = await dbContext.Accounts
            .Include(a => a.Watchlist)
            .ThenInclude(w => w.Movies)
            .FirstOrDefaultAsync(a => a.Id == account.Id)
            ?? throw new EntityNotFoundException("Account");
            if (account.Watchlist == null!)
            {
                throw new EntityNotFoundException("Watchlist");
            }

            var movieWatchlist = await dbContext.MovieWatchlist
                .FirstOrDefaultAsync(mw => mw.MovieId == movieId && mw.WatchlistId == account.Watchlist.Id) ?? throw new EntityNotFoundException("Movie in Watchlist");

            if (movieWatchlist == null!)
            {
                Console.WriteLine($"Movie with ID {movieId} not found in the watchlist. Skipping removal.");
                return;
            }

            dbContext.MovieWatchlist.Remove(movieWatchlist);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsMovieStarredAsync(int movieId)
        {
            var account = GetLoggedInUser();

            if (!account.Watchlist.Movies.Any())
            {
                throw new EntityNotFoundException("Watchlist");
            }

            return await Task.Run(() => account.Watchlist.Movies.Any(m => m.Id == movieId));
        }
    }
}
