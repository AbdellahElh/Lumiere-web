using Rise.Shared.Movies;

public class MovieEventDto
{
    public MovieDto Movie { get; set; }
    public Rise.Shared.Events.EventDto? Event { get; set; }
}