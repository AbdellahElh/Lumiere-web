using Auth0.ManagementApi;
using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Accounts;
using System.Security.Claims;

namespace Rise.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
       private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> AddUserAsync([FromBody] RegisterDto account)
        {
            await accountService.AddUserAsync(account);
            return Ok();
        }
    }
}
