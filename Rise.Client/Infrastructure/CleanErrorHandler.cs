﻿using Rise.Shared.Infrastructure;
using System.Net.Http.Json;

namespace Rise.Client.Infrastructure;

public class CleanErrorHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = null;
        try
        {
            response = await base.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (Exception)
        {
            var error = await response!.Content.ReadFromJsonAsync<ErrorDetails>(cancellationToken: cancellationToken);
            throw new Exception(error!.Message);
        }
    }
}
