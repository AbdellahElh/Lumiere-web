using Rise.Domain.Events;

namespace Rise.Domain.Movies;
public class Cinema : Entity
{
    public required string Name { get; set; }
    public required string Location { get; set; }
    public List<Showtime> Showtimes { get; set; } = [];
    public List<Movie> Movies { get; set; } = [];
    public List<Event> Events { get; set; } = [];

}
