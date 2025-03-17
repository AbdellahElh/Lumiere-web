
using Rise.Shared.Accounts;
using System.Threading.Tasks;

namespace Rise.Client.MockServices;

public class MockUserService : IAccountService
{
    public Task AddUserAsync(RegisterDto registerDto)
    {
        throw new System.NotImplementedException();
    }
}