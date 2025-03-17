namespace Rise.Shared.Movies;

public class MovieBannerDto
{
    public required int Id { get; set; }
    public required string BannerImageUrl { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
}