using Rise.Domain.Events;

namespace Rise.Domain.Movies;

public class Showtime : Entity
{
    public required int CinemaId { get; set; }
    public  int? MovieId { get; set; }

    public  int? EventId { get; set; }

    public required DateTime ShowTime { get; set; }
    public Cinema Cinema { get; set; } = null!;
    public Movie? Movie { get; set; } = null!;
    public Event? Event { get; set; } = null!;


}
