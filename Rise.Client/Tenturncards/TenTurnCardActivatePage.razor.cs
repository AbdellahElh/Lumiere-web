using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Rise.Domain.Exceptions;
using Rise.Shared.Tenturncards;
using System;
using System.Threading.Tasks;

namespace YourNamespace
{
    public class ActivateTenturncardBase : ComponentBase
    {
        [Inject]
        public ITenturncardService? TenturncardService { get; set; }

        protected TenturncardCodeModel tenturncardCodeModel = new TenturncardCodeModel();

        [Parameter]
        public EventCallback OnActivationSuccess { get; set; }

        [Parameter]
        public string? SuccessMessage { get; set; }

        [Parameter]
        public string? ErrorMessage { get; set; }

        protected async Task ActivateTenturncardAsync()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            try
            {
                await TenturncardService?.AddTenturncard(tenturncardCodeModel.TenturncardCode!)!;
                SuccessMessage = $"De tienbeurtenkaart met de code {tenturncardCodeModel.TenturncardCode} is geactiveerd";

                // Trigger the success callback
                if (OnActivationSuccess.HasDelegate)
                {
                    await OnActivationSuccess.InvokeAsync();
                }
            }
            catch(EntityAlreadyExistsException ex)
            {
                ErrorMessage = $"De tienbeurtenkaart met de code {tenturncardCodeModel.TenturncardCode} behoort al tot iemand";
            }
            catch (EntityNotFoundException ex)
            {
                ErrorMessage = $"Er was geen tienbeurtenkaart met de code {tenturncardCodeModel.TenturncardCode} gevonden";
            }
        }

        public class TenturncardCodeModel
        {
            public string? TenturncardCode { get; set; }
        }
    }
}
