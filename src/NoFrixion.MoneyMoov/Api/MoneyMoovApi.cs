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
    }

    /// <summary>
    /// Default constructor that uses the production MoneyMoov API.
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
