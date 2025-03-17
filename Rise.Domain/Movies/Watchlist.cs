using Rise.Domain.Accounts;
using Rise.Domain.MovieWatchlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.Movies;
public class Watchlist : Entity
{
    public required int UserId { get; set; }

    private Account? _account;
    public Account Account
    {
        get => _account!;
        set
        {
            _account = value ?? throw new ArgumentException("Account cannot be null");
        }
    }

    public List<MovieWatchlist> MovieWatchlists { get; set; } = [];

    public List<Movie> Movies { get; set; } = [];
}
