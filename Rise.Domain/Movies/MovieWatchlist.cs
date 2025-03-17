using Rise.Domain.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.MovieWatchlists;

public class MovieWatchlist : Entity
{
    public required int WatchlistId { get; set; }
    public required int MovieId { get; set; }

    public Watchlist Watchlist { get; set; } = null!;

    public Movie Movie { get; set; } = null!;
}
