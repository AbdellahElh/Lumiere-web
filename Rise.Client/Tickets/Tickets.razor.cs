using Microsoft.AspNetCore.Components;
using Rise.Shared.Tickets;

namespace Rise.Client.Tickets;

public partial class Tickets : ComponentBase
{
    [Parameter] public  List<TicketDto> TicketsList { get; set; } = new();
    private bool isModalOpen = false;


    [Inject]
    public required ITicketService TicketService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            TicketsList = await TicketService.GetTicketsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tickets: {ex.Message}");

        }
        
    }


    private void ShowModal()
    {
        isModalOpen = true;
    }

    private void Accept()
    {
        isModalOpen = false;
    }
   

    
}