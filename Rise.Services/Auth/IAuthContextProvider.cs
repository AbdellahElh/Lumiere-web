using System;
using System.Security.Claims;

namespace Rise.Services.Auth;

public interface IAuthContextProvider
{
    ClaimsPrincipal? User { get; }

    public int GetAccountIdFromMetadata(ClaimsPrincipal user);
}