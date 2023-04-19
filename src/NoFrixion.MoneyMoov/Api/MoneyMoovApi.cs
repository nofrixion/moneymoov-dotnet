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
using Microsoft.Extensions.Logging.Abstractions;

namespace NoFrixion.MoneyMoov;

public interface IMoneyMoovApi
{
    //bool SetBaseUrl(string url);

    IAccountClient AccountClient();

    IMerchantClient MerchantClient();

    IMetadataClient MetadataClient();

    IPaymentRequestClient PaymentRequestClient();

    IPayoutClient PayoutClient();

    IRuleClient RuleClient();

    IUserClient UserClient();

    IUserInviteClient UserInviteClient();

    IWebhookClient WebhookClient();
}

public class MoneyMoovApi : IMoneyMoovApi
{
    private sealed class MoneyMoovHttpClientFactory : IHttpClientFactory, IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed;

        public MoneyMoovHttpClientFactory(Uri baseUri)
        {
            var handler = new HttpClientHandler();
            _httpClient = new HttpClient(handler, disposeHandler: true);
            _httpClient.BaseAddress = baseUri;
        }

        public HttpClient CreateClient(string name)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(MoneyMoovHttpClientFactory));
            return _httpClient;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _httpClient.Dispose();
                _disposed = true;
            }
        }
    }

    private readonly IHttpClientFactory _httpClientFactory;

    public MoneyMoovApi(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

        //string baseUrlStr = configuration[MoneyMoovConfigKeys.NOFRIXION_MONEYMOOV_API_BASE_URL];

        //if (string.IsNullOrEmpty(baseUrlStr))
        //{
        //    _moneyMoovBaseUri = new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        //    //_logger.LogDebug($"{nameof(MoneyMoovApi)} created with default base URI of {_moneyMoovBaseUri}.");
        //}
        //else if (Uri.TryCreate(baseUrlStr, UriKind.Absolute, out var baseUri))
        //{
        //    _moneyMoovBaseUri = baseUri;
        //    //_logger.LogDebug($"{nameof(MoneyMoovApi)} created with base URI of {_moneyMoovBaseUri}.");
        //}
        //else
        //{
        //    //_logger.LogError($"The base URI supplied to the MoneyMoov service was not a valid URI, {baseUrlStr}. Using default of {MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL}.");
        //    _moneyMoovBaseUri = new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        //}
    }

    /// <summary>
    /// Default constructor that use the production MoneyMoov API.
    /// </summary>
    public MoneyMoovApi()
    {
        _httpClientFactory = new MoneyMoovHttpClientFactory(
            new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL));
    }

    public MoneyMoovApi(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var baseUri))
        {
            _httpClientFactory = new MoneyMoovHttpClientFactory(baseUri);
        }
        else
        {
            throw new ArgumentException("The url string supplied to the MoneyMoovApi client was not recognised as a valid URL.", nameof(url));
        }        
    }

    /// <summary>
    /// Attempts to update the base URL of the MoneyMoov API for this service to use.
    /// </summary>
    /// <param name="url">The new base URL to set.</param>
    /// <returns>True if the URL was successfully updated, otherwise false.</returns>
    //public bool SetBaseUrl(string url)
    //{
    //    if (!string.IsNullOrEmpty(url) && Uri.TryCreate(url, UriKind.Absolute, out var baseUri))
    //    {
    //        _moneyMoovBaseUri = baseUri;
    //        //_logger.LogDebug($"{nameof(MoneyMoovApi)} updated base URI to {_moneyMoovBaseUri}.");
    //        return true;
    //    }
    //    else
    //    {
    //        _logger.LogWarning($"The base URI supplied was not a valid URI. It should be in the format {MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL}. Base URI was not updated.");
    //        return false;
    //    }
    //}

    public IAccountClient AccountClient()
        => new AccountClient(new MoneyMoovApiClient(_httpClientFactory));

    public IMerchantClient MerchantClient()
        => new MerchantClient(new MoneyMoovApiClient(_httpClientFactory));

    public IMetadataClient MetadataClient()
        => new MetadataClient(new MoneyMoovApiClient(_httpClientFactory));

    public IPaymentRequestClient PaymentRequestClient()
        => new PaymentRequestClient(new MoneyMoovApiClient(_httpClientFactory));

    public IPayoutClient PayoutClient()
        => new PayoutClient(new MoneyMoovApiClient(_httpClientFactory));

    public IRuleClient RuleClient()
        => new RuleClient(new MoneyMoovApiClient(_httpClientFactory));

    public IUserClient UserClient()
        => new UserClient(new MoneyMoovApiClient(_httpClientFactory));

    public IUserInviteClient UserInviteClient()
        => new UserInviteClient(new MoneyMoovApiClient(_httpClientFactory));

    public IWebhookClient WebhookClient()
        => new WebhookClient(new MoneyMoovApiClient(_httpClientFactory));
}
