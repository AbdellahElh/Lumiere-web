using Microsoft.AspNetCore.Components;
using Rise.Shared.Events;
using Rise.Client.events;
namespace Rise.Client.events;

public partial class Events
{
    [Parameter] public List<EventDto> EventsList { get; set; } = [];

    [Inject] public required IEventService EventService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var filters = new FiltersDataDto
        {
            SelectedCinemas = new List<string> { "Brugge", "Antwerpen", "Mechelen", "Cinema Cartoons" }
        };

        EventsList = await EventService.GetEventAsync(filters);
    }
    private async Task LoadEventWithFilters(FiltersDataDto args)
    {
        EventsList = await EventService.GetEventAsync(args);
    }
}