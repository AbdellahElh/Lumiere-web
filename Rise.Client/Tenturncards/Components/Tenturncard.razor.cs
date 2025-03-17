using Microsoft.AspNetCore.Components;
using Rise.Shared.Accounts;
using Rise.Shared.Tenturncards;

namespace Rise.Client.Tenturncards.Components
{
    public partial class Tenturncard : ComponentBase
    {
        [Inject]
        public required ITenturncardService tenturncardService { get; set; }

        [Parameter, EditorRequired]
        public required TenturncardDto KaartData { get; set; }

        public Boolean isEditing = false;
        private string errorMessage = string.Empty;

        public TenturncardDto cardDto = new();

        private void OnEditButtonClick()
        {
            isEditing = true;
            Console.WriteLine("Bewerk-knop geklikt voor kaart: " + KaartData.ExpirationDate);
        }

        private async Task EditTenturncardSubmit()
        {
            errorMessage = string.Empty;
            try
            {
                if(KaartData.AmountLeft.Equals(0))
                {
                    throw new Exception("Deze kaart is op");
                }
                if (cardDto.AmountLeft < 0)
                {
                    throw new Exception("Het aantal resterende beurten mag niet negatief zijn");
                }
                cardDto.Id = KaartData.Id;
                await tenturncardService.EditTenturncardAsync(cardDto);
                isEditing = false;
                KaartData.AmountLeft = cardDto.AmountLeft;
            }
            catch (Exception ex)
            {
                errorMessage = string.Empty;
                errorMessage = ex.Message;
            }
        }
    }
    
}
