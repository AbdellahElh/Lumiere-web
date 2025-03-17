using Rise.Domain.Events;
using Rise.Domain.MovieWatchlists;
using Rise.Domain.Tickets;


namespace Rise.Domain.Movies;

public class Movie : Entity
{

    public int? EventId { get; set; } 
    public Event? Event { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public required string Description { get; set; }

    public required int Duration { get; set; }
  
    public required string Director { get; set; }

    public List<string> Cast { get; set; } = [];

    public required DateTime ReleaseDate { get; set; }

    public string? VideoPlaceholderUrl { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? BannerImageUrl { get; set; }
  
    public string? PosterImageUrl { get; set; }

    public required string MovieLink { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public List<Showtime> Showtimes { get; set; } = [];

    public List<Cinema> Cinemas { get; set; } = [];
 
    public List<MovieWatchlist> MovieWatchlists { get; set; } = [];
}
