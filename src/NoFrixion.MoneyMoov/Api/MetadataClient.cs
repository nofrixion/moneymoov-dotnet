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

using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;
using System.Net.Http.Json;

namespace NoFrixion.MoneyMoov;

public interface IMetadataClient
{
    Task<MoneyMoovApiResponse<NoFrixionVersion>> GetVersionAsync();

    Task<MoneyMoovApiResponse<User>> WhoamiAsync(string userAccessToken);

    Task<MoneyMoovApiResponse<Merchant>> WhoamiMerchantAsync(string merchantAccessToken);

    Task<MoneyMoovApiResponse<string>> EchoAsync(string name, string message);

    Task<MoneyMoovApiResponse<dynamic>> EchoJsonAsync(string name, string message);
}

public class MetadataClient : IMetadataClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public MetadataClient()
    {
        _apiClient = new MoneyMoovApiClient(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        _logger = NullLogger.Instance;
    }

    public MetadataClient(string baseUri)
    {
        _apiClient = new MoneyMoovApiClient(baseUri);
        _logger = NullLogger.Instance;
    }

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
        return _apiClient.GetAsync<NoFrixionVersion>(MoneyMoovUrlBuilder.MetadataApi.VersionUrl(_apiClient.GetBaseUri().ToString()));
    }

    /// <summary>
    /// Calls the MoneyMoov WhoAmI endpoint to check whether a User access token is valid.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <returns>If the token is valid the authenticated User's details are returned.</returns>
    public Task<MoneyMoovApiResponse<User>> WhoamiAsync(string userAccessToken)
    {
        var url = MoneyMoovUrlBuilder.MetadataApi.WhoamiUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(WhoamiAsync));

        return !prob.IsEmpty ?
            Task.FromResult(new MoneyMoovApiResponse<User>(HttpStatusCode.PreconditionFailed, new Uri(url), prob)) :
            _apiClient.GetAsync<User>(url, userAccessToken);
    }

    /// <summary>
    /// Calls the MoneyMoov WhoAmI Merchant endpoint to check whether a Merchant access token is valid.
    /// </summary>
    /// <param name="merchantAccessToken">A Merchant scoped JWT access token.</param>
    /// <returns>If the token is valid the authenticated Merchant's details are returned.</returns>
    public Task<MoneyMoovApiResponse<Merchant>> WhoamiMerchantAsync(string merchantAccessToken)
    {
        var url = MoneyMoovUrlBuilder.MetadataApi.WhoamiMerchantUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(merchantAccessToken, nameof(WhoamiMerchantAsync));

        return !prob.IsEmpty ?
            Task.FromResult(new MoneyMoovApiResponse<Merchant>(HttpStatusCode.PreconditionFailed, new Uri(url), prob)) :
            _apiClient.GetAsync<Merchant>(url, merchantAccessToken);
    }

    /// <summary>
    /// Calls the MoneyMoov Metadata get version endpoint with a form URL encoded payload.
    /// </summary>
    /// <returns>A string echo response message.</returns>
    public Task<MoneyMoovApiResponse<string>> EchoAsync(string name, string message)
    {
        var content = new FormUrlEncodedContent(Map.create(("message", message), ("name", name)).ToDictionary());
        return _apiClient.PostAsync<string>(MoneyMoovUrlBuilder.MetadataApi.EchoUrl(_apiClient.GetBaseUri().ToString()), content);
    }

    /// <summary>
    /// Calls the MoneyMoov Metadata get version endpoint with a JSON encoded payload.
    /// </summary>
    /// <returns>A JSON echo response message.</returns>
    public Task<MoneyMoovApiResponse<dynamic>> EchoJsonAsync(string name, string message)
    {
        var content = JsonContent.Create(new { name, message });
        return _apiClient.PostAsync<dynamic>(MoneyMoovUrlBuilder.MetadataApi.EchoUrl(_apiClient.GetBaseUri().ToString()), content);
    }
}
