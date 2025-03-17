using System;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Rise.Domain.Exceptions;
using Rise.Services.Auth;

namespace Rise.Server.Auth;

public class HttpContextAuthProvider(IHttpContextAccessor httpContextAccessor) : IAuthContextProvider
{
    public ClaimsPrincipal? User => httpContextAccessor!.HttpContext?.User;

    public int GetAccountIdFromMetadata(ClaimsPrincipal user)
    {
        const string metadataClaimType = "https://lumiere.com/user_metadata";

        // Find the metadata claim
        var metadataClaim = user.FindFirstValue(metadataClaimType);

        if (string.IsNullOrEmpty(metadataClaim))
        {
            throw new EntityNotFoundException("Metadata claim not found.");
        }

        // Parse the metadata JSON to extract the account id
        var metadata = JObject.Parse(metadataClaim);
        if (metadata["lumiereUserId"] != null && int.TryParse(metadata["lumiereUserId"]?.ToString(), out int accountId))
        {
            return accountId;
        }

        throw new InvalidOperationException("Account ID not found or invalid.");
    }

}
