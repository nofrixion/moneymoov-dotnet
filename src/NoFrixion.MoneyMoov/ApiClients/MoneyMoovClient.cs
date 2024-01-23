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

using System.Net.Http;

namespace NoFrixion.MoneyMoov;

public interface IMoneyMoovClient
{
    IAccountClient AccountClient();

    IMerchantClient MerchantClient();

    IMetadataClient MetadataClient();

    IPaymentRequestClient PaymentRequestClient();

    IPayoutClient PayoutClient();

    IReportClient ReportClient();

    IRuleClient RuleClient();

    IUserClient UserClient();

    IUserInviteClient UserInviteClient();

    IWebhookClient WebhookClient();
    
    IBeneficiaryClient BeneficiaryClient();

    Uri? GetBaseUrl();
}

public class MoneyMoovClient : IMoneyMoovClient
{
    public const string MONEYMOOV_HTTP_CLIENT_NAME = "moneymoov";

    private readonly IHttpClientFactory _httpClientFactory;

    public Uri? GetBaseUrl() => _httpClientFactory.CreateClient(MONEYMOOV_HTTP_CLIENT_NAME).BaseAddress;

    public MoneyMoovClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Default constructor that uses the production MoneyMoov API.
    /// </summary>
    public MoneyMoovClient()
    {
        _httpClientFactory = new RestHttpClientFactory(
            new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL));
    }

    public MoneyMoovClient(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out var baseUri))
        {
            _httpClientFactory = new RestHttpClientFactory(baseUri);
        }
        else
        {
            throw new ArgumentException("The url string supplied to the MoneyMoovApi client was not recognised as a valid URL.", nameof(url));
        }        
    }

    public IAccountClient AccountClient()
        => new AccountClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));

    public IBeneficiaryClient BeneficiaryClient()
        => new BeneficiaryClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));

    public IMerchantClient MerchantClient()
        => new MerchantClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));

    public IMetadataClient MetadataClient()
        => new MetadataClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));

    public IPaymentRequestClient PaymentRequestClient()
        => new PaymentRequestClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));

    public IPayoutClient PayoutClient()
        => new PayoutClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));

    public IReportClient ReportClient()
        => new ReportClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));

    public IRuleClient RuleClient()
        => new RuleClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));

    public IUserClient UserClient()
        => new UserClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));

    public IUserInviteClient UserInviteClient()
        => new UserInviteClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));

    public IWebhookClient WebhookClient()
        => new WebhookClient(new RestApiClient(_httpClientFactory, MONEYMOOV_HTTP_CLIENT_NAME));
}
