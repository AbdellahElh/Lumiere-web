using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Shared.Accounts;

public interface IAccountService
{
    public Task AddUserAsync(RegisterDto registerDto);

}
