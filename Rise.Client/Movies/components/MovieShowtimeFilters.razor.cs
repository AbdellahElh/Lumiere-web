using Microsoft.AspNetCore.Components;
using Rise.Shared.Movies;

namespace Rise.Client.Movies.components;

public partial class MovieShowtimeFilters
{
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public DateTime? SelectedDate { get; set; } = DateTime.Today;

  
    [Parameter]
    [EditorRequired]
    public string? SelectedCinemas { get; set; }

    private bool isDropdownVisibleCinemas = false;

    private DateTime selectedDate = DateTime.Today;

    private List<string> selectedCinemas = new List<string>() { "Brugge", "Antwerpen", "Mechelen", "Cinema Cartoons" };

    // private string todayString = DateTime.Today.ToString("yyyy-MM-dd");
    // private string maxDateString = DateTime.Now.AddDays(14).ToString("yyyy-MM-dd");
    // never used

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrEmpty(SelectedCinemas))
        {
            selectedCinemas = SelectedCinemas.Split(',').ToList();
        }

        if (SelectedDate.HasValue)
        {
            selectedDate = SelectedDate.Value;
        }
    }


    private void ToggleDropdownCinemas()
    {
        isDropdownVisibleCinemas = !isDropdownVisibleCinemas;
    }

    private void HandleDateChange(ChangeEventArgs e)
    {

        if (DateTime.TryParse(e.Value?.ToString(), out var date))
        {
            selectedDate = date;

            FilterMovies();
        }
    }

    private void HandleCinemaChange(string cinema, object? isChecked)
    {
        bool isSelected = isChecked as bool? ?? false;

        if (isSelected)
        {
            if (!selectedCinemas.Contains(cinema))
                selectedCinemas.Add(cinema);
        }
        else
        {
            selectedCinemas.Remove(cinema);
        }

        FilterMovies();
    }


    private void FilterMovies() {
        Dictionary<string, object?> parameters = new()
        {
            { nameof(SelectedDate), selectedDate.ToString("yyyy-MM-dd") },
            { nameof(SelectedCinemas), string.Join(",", selectedCinemas) }
        };

        var uri = NavigationManager.GetUriWithQueryParameters(parameters);
        NavigationManager.NavigateTo(uri);
    }




}
