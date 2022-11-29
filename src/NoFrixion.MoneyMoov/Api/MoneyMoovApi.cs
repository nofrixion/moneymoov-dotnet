//-----------------------------------------------------------------------------
// Filename: MoneyMoovApi.cs
//
// Description: Main class for accessing the MoneyMoov API resources.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 06 Jul 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NoFrixion.MoneyMoov;

public interface IMoneyMoovApi
{
    IMerchantClient MerchantClient();

    IMetadataClient MetadataClient();
}

public class MoneyMoovApi : IMoneyMoovApi
{
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private Uri _moneyMoovBaseUri;

    public MoneyMoovApi(
        ILogger<MoneyMoovApi> logger,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _config = configuration;
        _httpClientFactory = httpClientFactory;

        string baseUrlStr = configuration[MoneyMoovConfigKeys.NOFRIXION_MONEYMOOV_API_BASE_URL];

        if (string.IsNullOrEmpty(baseUrlStr))
        {
            _moneyMoovBaseUri = new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
            _logger.LogDebug($"{nameof(MoneyMoovApi)} created with default base URI of {_moneyMoovBaseUri}.");
        }
        else if (Uri.TryCreate(baseUrlStr, UriKind.Absolute, out var baseUri))
        {
            _moneyMoovBaseUri = baseUri;
            _logger.LogDebug($"{nameof(MoneyMoovApi)} created with base URI of {_moneyMoovBaseUri}.");
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
            _logger.LogDebug($"{nameof(MoneyMoovApi)} updated base URI to {_moneyMoovBaseUri}.");
            return true;
        }
        else
        {
            _logger.LogWarning($"The base URI supplied was not a valid URI. It should be in the format {MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL}. Base URI was not updated.");
            return false;
        }
    }

    private HttpClient GetHttpClient()
    {
        var httpClient = _httpClientFactory.CreateClient(MoneyMoovApiClient.HTTP_CLIENT_NAME);
        httpClient.BaseAddress = _moneyMoovBaseUri;
        return httpClient;
    }

    public IMerchantClient MerchantClient()
     => new MerchantClient(new MoneyMoovApiClient(GetHttpClient()));

    public IMetadataClient MetadataClient()
     => new MetadataClient(new MoneyMoovApiClient(GetHttpClient()));
}
