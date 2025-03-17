using Microsoft.AspNetCore.Components;
using Rise.Shared.Events;

namespace Rise.Client.events.components;

public partial class EventsFilter
{
    [Parameter] public EventCallback<FiltersDataDto> OnCinemaChanged { get; set; }


    private bool isDropdownVisibleCinemas = false;

    private List<string> selectedCinemas = new List<string>() { "Brugge", "Antwerpen", "Mechelen", "Cinema Cartoons" };

 

    private void ToggleDropdownCinemas()
    {
        isDropdownVisibleCinemas = !isDropdownVisibleCinemas;
    }

   

    private async Task HandleCinemaChange(string cinema, object? isChecked)
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
        var args = new FiltersDataDto
        {
            SelectedCinemas = selectedCinemas
        };
        await OnCinemaChanged.InvokeAsync(args);
    }


}
