//-----------------------------------------------------------------------------
// Filename: WebHooksClient.cs
//
// Description: An API client to call the MoneyMoov WebHooks API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 12 Mar 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;

namespace NoFrixion.MoneyMoov;

public interface IWebhookClient
{
    Task<RestApiResponse<Webhook>> CreateWebhookAsync(string userAccessToken, WebhookCreate webhookCreate);

    Task<RestApiResponse> DeleteWebhookAsync(string accessToken, Guid webhookID);

    Task<RestApiResponse<IEnumerable<Webhook>>> GetWebhooksAsync(string userAccessToken, Guid merchantID);
}

public class WebhookClient : IWebhookClient
{
    private readonly ILogger _logger;
    private readonly IRestApiClient _apiClient;

    public WebhookClient()
    {
        _apiClient = new RestApiClient(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        _logger = NullLogger.Instance;
    }

    public WebhookClient(string baseUri)
    {
        _apiClient = new RestApiClient(baseUri);
        _logger = NullLogger.Instance;
    }

    public WebhookClient(IRestApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public WebhookClient(IRestApiClient apiClient, ILogger<AccountClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant webhooks endpoint create a new webhook.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="webHookCreate">The model containing the data about the new webhook to create.</param>
    /// <returns>If successful, a webhook object.</returns>
    public Task<RestApiResponse<Webhook>> CreateWebhookAsync(string userAccessToken, WebhookCreate webHookCreate)
    {
        var url = MoneyMoovUrlBuilder.WebhooksApi.WebhooksUrl(_apiClient.GetBaseUri().ToString());
        
        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(CreateWebhookAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<Webhook>(url, userAccessToken, new FormUrlEncodedContent(webHookCreate.ToDictionary())),
            _ => Task.FromResult(new RestApiResponse<Webhook>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant webhooks endpoint to retrieve all a merchant's existing webhooks.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="merhcantID">The merchant ID to get the webhooks for.</param>
    /// <returns>If successful, a list of webhooks.</returns>
    public Task<RestApiResponse<IEnumerable<Webhook>>> GetWebhooksAsync(string userAccessToken, Guid merchantID)
    {
        var url = MoneyMoovUrlBuilder.WebhooksApi.AllWebhooksUrl(_apiClient.GetBaseUri().ToString(), merchantID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetWebhooksAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<IEnumerable<Webhook>> (url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse<IEnumerable<Webhook>>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant delete webhook endpoint to delete an existing webhook.
    /// </summary>
    /// <param name="accessToken">A User scoped JWT access token.</param>
    /// <param name="webhookID">The ID of the webhook to delete.</param>
    public Task<RestApiResponse> DeleteWebhookAsync(string accessToken, Guid webhookID)
    {
        var url = MoneyMoovUrlBuilder.WebhooksApi.WebhookUrl(_apiClient.GetBaseUri().ToString(), webhookID);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(DeleteWebhookAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.DeleteAsync(url, accessToken),
            _ => Task.FromResult(new RestApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
