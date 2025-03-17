using Microsoft.AspNetCore.Components;
using Rise.Shared.Events;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Rise.Client.events;

public partial class EventPage : ComponentBase
{
    [Parameter]
    public int IdEvent { get; set; }

    private EventDto? foundEvent;
    private bool isLoading = true;

    [Inject]
    public required IEventService EventService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        foundEvent = await EventService.GetEventByIdAsync(IdEvent);
        isLoading = false;
    }
}
