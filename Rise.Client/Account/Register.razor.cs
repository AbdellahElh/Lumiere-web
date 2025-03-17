using Microsoft.AspNetCore.Components;
using Rise.Shared.Accounts;

namespace Rise.Client.Account;

public partial class Register : ComponentBase
{
    private RegisterModel registerModel = new RegisterModel();

    [Parameter] public string? ErrorMessage { get; set; }

    [Inject] private IAccountService AccountService { get; set; } = null!;

    private async void HandleRegister()
    {
        try
        {
            ErrorMessage = string.Empty;

            if (registerModel.Password != registerModel.RepeatPassword)
            {
                throw new Exception("Wachtwoorden moeten overeen komen");
            }

            RegisterDto registerDto = new RegisterDto
            {
                Email = registerModel.Email,
                Password = registerModel.Password
            };
            await AccountService.AddUserAsync(registerDto);
            navigation.NavigateTo("/");

        }
        catch (Exception ex)
        {
            ErrorMessage = string.Empty;
            ErrorMessage = ex.Message;
            StateHasChanged();
        }
    }

    public class RegisterModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RepeatPassword { get; set; } = string.Empty;
    }

}