//-----------------------------------------------------------------------------
// Filename: MetadataClient.cs
//
// Description: An API client to call the MoneyMoov Metadata API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 26 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;
using System.Net.Http.Json;

namespace NoFrixion.MoneyMoov;

public class MetadataClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public MetadataClient(IMoneyMoovApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public MetadataClient(IMoneyMoovApiClient apiClient, ILogger<MetadataClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov Metadata get version endpoint.
    /// </summary>
    /// <returns>The MoneyMoov version information.</returns>
    public Task<MoneyMoovApiResponse<NoFrixionVersion>> GetVersionAsync()
    {
        return _apiClient.GetAsync<NoFrixionVersion>(MoneyMoovUrlBuilder.VersionUrl(_apiClient.GetBaseUri().ToString()));
    }

    /// <summary>
    /// Calls the MoneyMoov WhoAmI endpoint to check whether a User access token is valid.
    /// </summary>
    /// <param name="accessToken">A User JWT access token.</param>
    /// <returns>If the token is valid the authenticated User's details are returned.</returns>
    public Task<MoneyMoovApiResponse<User>> WhoamiAsync(string accessToken)
    {
        var url = MoneyMoovUrlBuilder.WhoamiUrl(_apiClient.GetBaseUri().ToString());

        var prob = CheckAccessToken(accessToken, nameof(WhoamiMerchantAsync));

        return !prob.IsEmpty ?
            Task.FromResult(new MoneyMoovApiResponse<User>(HttpStatusCode.PreconditionFailed, new Uri(url), prob)) :
            _apiClient.GetAsync<User>(url, accessToken);
    }

    /// <summary>
    /// Calls the MoneyMoov WhoAmI Merchant endpoint to check whether a Merchant access token is valid.
    /// </summary>
    /// <param name="accessToken">A Merchant JWT access token.</param>
    /// <returns>If the token is valid the authenticated Merchant's details are returned.</returns>
    public Task<MoneyMoovApiResponse<Merchant>> WhoamiMerchantAsync(string accessToken)
    {
        var url = MoneyMoovUrlBuilder.WhoamiMerchantUrl(_apiClient.GetBaseUri().ToString());

        var prob = CheckAccessToken(accessToken, nameof(WhoamiMerchantAsync));

        return !prob.IsEmpty ?
            Task.FromResult(new MoneyMoovApiResponse<Merchant>(HttpStatusCode.PreconditionFailed, new Uri(url), prob)) :
            _apiClient.GetAsync<Merchant>(url, accessToken);
    }

    /// <summary>
    /// Calls the MoneyMoov Metadata get version endpoint with a form URL encoded payload.
    /// </summary>
    /// <returns>A string echo response message.</returns>
    public Task<MoneyMoovApiResponse<string>> EchoAsync(string name, string message)
    {
        var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { new ( "message", message ), new ( "name", name ) });
        return _apiClient.PostAsync<string>(MoneyMoovUrlBuilder.EchoUrl(_apiClient.GetBaseUri().ToString()), content);
    }

    /// <summary>
    /// Calls the MoneyMoov Metadata get version endpoint with a JSON encoded payload.
    /// </summary>
    /// <returns>A JSON echo response message.</returns>
    public Task<MoneyMoovApiResponse<dynamic>> EchoJsonAsync(string name, string message)
    {
        var content = JsonContent.Create(new { name = name, message = message });
        return _apiClient.PostAsync<dynamic>(MoneyMoovUrlBuilder.EchoUrl(_apiClient.GetBaseUri().ToString()), content);
    }

    private NoFrixionProblemDetails CheckAccessToken(string accessToken, string callerName)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            _logger.LogWarning($"{callerName} requires an access token but one was not provided.");
            return NoFrixionProblemDetails.MissingAccessToken(HttpStatusCode.PreconditionFailed, nameof(callerName));
        }
        else
        {
            return NoFrixionProblemDetails.Empty;
        }
    }
}
