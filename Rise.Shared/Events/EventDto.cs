using System;
using Rise.Shared.Movies;

namespace Rise.Shared.Events;

public class EventDto
{
    public required int Id { get; set; }
    public string Cover { get; set; }

    public string Price { get; set; }

    public string Title { get; set; }
    public string Genre { get; set; }

    public string Type { get; set; }

    public string Duration { get; set; }
    public string Director { get; set; }
    public string Description { get; set; }

    public string Video { get; set; }
    public string VideoPlaceholderImage { get; set; }
    public string Date { get; set; }

    public string Location { get; set; }

    public List<string> Cast { get; set; } = new();
    public List<CinemaDto> Cinemas { get; set; } = new();

    public List<MoviePosterDto> Movies { get; set; } = new();
}
