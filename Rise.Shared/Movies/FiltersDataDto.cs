
namespace Rise.Shared.Movies;

public class FiltersDataDto
{
    public DateTime? SelectedDate { get; set; }
    public List<string>? SelectedCinemas { get; set; } = new List<string> { "Brugge", "Antwerpen", "Mechelen", "Cinema Cartoons" };
    public string? Title { get; set; }
    public int? pageNumber { get; set; }
    public int? pageSize { get; set; }
}
