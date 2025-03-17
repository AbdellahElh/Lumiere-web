using Microsoft.AspNetCore.Components;
using Rise.Shared.Tenturncards;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Rise.Client.Tenturncards;

public partial class TenturncardPage : ComponentBase
{
    private List<TenturncardDto> RittenKaarten { get; set; } = new();
    private bool isModalOpen = false;

    [Inject]
    public required ITenturncardService TenturncardService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            RittenKaarten = await TenturncardService.GetTenturncardsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tenturncards: {ex.Message}");

        }
    }

   
    private void ShowModal()
    {
        isModalOpen = true;
    }

    private void CloseModal()
    {
        isModalOpen = false;
    }

    private void Accept()
    {
        isModalOpen = false;
        // Add additional logic here if needed when accepting
    }
}