//-----------------------------------------------------------------------------
// Filename: MoneyMoovService.cs
//
// Description: Service to generate an authorised client and interact with the
// NoFrixion MoneyMoov service.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 06 Jul 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov.Services;

public class MoneyMoovService : IMoneyMoovService
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private Uri _moneyMoovBaseUri;

    public MoneyMoovService(
        ILogger<MoneyMoovService> logger,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _config = configuration;
        _httpClientFactory = httpClientFactory;

        string baseUrlStr = configuration[MoneyMoovConfigKeys.MONEYMOOV_BASE_URL];

        if (string.IsNullOrEmpty(baseUrlStr))
        {
            _moneyMoovBaseUri = new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
            _logger.LogDebug($"{nameof(MoneyMoovService)} created with default base URI of {_moneyMoovBaseUri}.");
        }
        else if (Uri.TryCreate(baseUrlStr, UriKind.Absolute, out var baseUri))
        {
            _moneyMoovBaseUri = baseUri;
            _logger.LogDebug($"{nameof(MoneyMoovService)} created with base URI of {_moneyMoovBaseUri}.");
        }
        else
        {
            _logger.LogError($"The base URI supplied to the MoneyMoov service was not a valid URI, {baseUrlStr}. Using default of {MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL}.");
            _moneyMoovBaseUri = new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        }
    }

    /// <summary>
    /// Attempts to update the base URL of the MoneyMoov API for this service to use.
    /// </summary>
    /// <param name="url">The new base URL to set.</param>
    /// <returns>True if the URL was successfully updated, otherwise false.</returns>
    public bool SetBaseUrl(string url)
    {
        if (!string.IsNullOrEmpty(url) && Uri.TryCreate(url, UriKind.Absolute, out var baseUri))
        {
            _moneyMoovBaseUri = baseUri;
            _logger.LogDebug($"{nameof(MoneyMoovService)} updated base URI to {_moneyMoovBaseUri}.");
            return true;
        }
        else
        {
            _logger.LogWarning($"The base URI supplied was not a valid URI. It should be in the format {MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL}. Base URI was not updated.");
            return false;
        }
    }

    public Task<MoneyMoovApiResponse<NoFrixionVersion>> VersionAsync()
    {
        var metadataClient = new MetadataClient(new MoneyMoovApiClient(GetHttpClient()));
        return metadataClient.GetVersionAsync();
    }

    public Task<MoneyMoovApiResponse<User>> WhoamiAsync(string accessToken)
    {
        var metadataClient = new MetadataClient(new MoneyMoovApiClient(GetHttpClient()));
        return metadataClient.WhoamiAsync(accessToken);
    }

    private HttpClient GetHttpClient()
    {
        var httpClient = _httpClientFactory.CreateClient(MoneyMoovApiClient.HTTP_CLIENT_NAME);
        httpClient.BaseAddress = _moneyMoovBaseUri;
        return httpClient;
    }
}
