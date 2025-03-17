using System;
using System.Collections.Generic;
using Rise.Shared.Events;

namespace Rise.Shared.Movies
{
    public class MovieDto
    {
        public required int Id { get; set; }
        public  int? EventId { get; set; }
        public EventDto? Event { get; set; }
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public required string Description { get; set; }
        public required int Duration { get; set; }
        public required string Director { get; set; }
        public List<string> Cast { get; set; } = new();
        public required DateTime ReleaseDate { get; set; }
        public string? VideoPlaceholderUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? BannerImageUrl { get; set; }
        public string? PosterImageUrl { get; set; }
        public required string MovieLink { get; set; }
        public List<CinemaDto> Cinemas { get; set; } = new();
    }
}
