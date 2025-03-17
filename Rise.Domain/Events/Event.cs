using NodaTime;
using Rise.Domain.MovieWatchlists;
using Rise.Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rise.Shared.Movies;
using Rise.Domain.Movies;

namespace Rise.Domain.Events;

public class Event : Entity
{
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public required string Type { get; set; }

    public required string Description { get; set; }
    public required string Price { get; set; }

    public required int Duration { get; set; }
  
    public required string Director { get; set; }

    public List<string> Cast { get; set; } = [];

    public required DateTime ReleaseDate { get; set; }

    public string? VideoPlaceholderUrl { get; set; }
    public string? CoverImageUrl { get; set; }


    public required string EventLink { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public List<Showtime> Showtimes { get; set; } = [];

    public List<Cinema> Cinemas { get; set; } = [];
    public List<Movie> Movies { get; set; } = [];


}
