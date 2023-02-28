using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Kymeta.Cloud.Services.EnterpriseBroker.sdk.Models;
using Kymeta.Cloud.Services.Toolbox.Extensions;

namespace Kymeta.Cloud.Services.EnterpriseBroker.sdk.Clients.Salesforce;

/// <summary>
/// Delegating handler that injects a client access token into an outgoing request
/// </summary>
public class SalesforceAccessTokenHandler : DelegatingHandler
{
    private readonly SalesforceAuthClient _salesforceAuthClient;

    public SalesforceAccessTokenHandler(SalesforceAuthClient salesforceAuthClient) => _salesforceAuthClient = salesforceAuthClient;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
    {
        await SetTokenAsync(request, false, token);

        var response = await base.SendAsync(request, token);

        // retry if 401
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            response.Dispose();

            await SetTokenAsync(request, true, token);
            return await base.SendAsync(request, token);
        }

        return response;
    }

    private async Task SetTokenAsync(HttpRequestMessage request, bool forceRefresh, CancellationToken token)
    {
        SalesforceAuthenticationResponse? authDetails = await _salesforceAuthClient.GetAuthToken(token, forceRefresh);
        string accessToken = authDetails?.AccessToken?.ToNullIfEmpty() ?? throw new HttpRequestException("Failed to get access token", null, HttpStatusCode.Unauthorized);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }
}