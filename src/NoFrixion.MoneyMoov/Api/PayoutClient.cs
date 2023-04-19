//-----------------------------------------------------------------------------
// Filename: PayoutClient.cs
//
// Description: An API client to call the MoneyMoov Payout API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 05 Feb 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 19 Apr 2023  Aaron Clauson   Added batch payout methods.
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

public interface IPayoutClient
{
    Task<MoneyMoovApiResponse<Payout>> CreatePayoutAsync(string userAccessToken, PayoutCreate payoutCreate);

    Task<MoneyMoovApiResponse<Payout>> GetPayoutAsync(string userAccessToken, Guid payoutID);

    Task<MoneyMoovApiResponse<Payout>> GetPayoutByInvoiceIDAsync(string merchantAccessToken, string invoiceID);

    Task<MoneyMoovApiResponse> SubmitPayoutAsync(string strongUserAccessToken, Guid payoutID);

    Task<MoneyMoovApiResponse<Payout>> UpdatePayoutAsync(string accessToken, Guid payoutID, PayoutUpdate payoutUpdate);

    Task<MoneyMoovApiResponse<BatchPayout>> GetBatchPayoutAsync(string userAccessToken, Guid batchPayoutID);

    Task<MoneyMoovApiResponse<BatchPayout>> CreateBatchPayoutAsync(string userAccessToken, List<Guid> payoutIDs);
}

public class PayoutClient : IPayoutClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public PayoutClient(IMoneyMoovApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public PayoutClient(IMoneyMoovApiClient apiClient, ILogger<RuleClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov Payout endpoint to create a new payout. The payout will be in a state
    /// of pending approval and will require an approver to complete a strong authentication and confirm
    /// the details before the payout will be submitted.
    /// </summary>
    /// <param name="userAccessToken">The access token of the user creating the payout.</param>
    /// <param name="payoutCreate">A model with the details of the payout to create.</param>
    /// <returns>An API response indicating the result of the create attempt.</returns>
    public Task<MoneyMoovApiResponse<Payout>> CreatePayoutAsync(string userAccessToken, PayoutCreate payoutCreate)
    {
        var url = MoneyMoovUrlBuilder.PayoutsApi.PayoutsUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(CreatePayoutAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<Payout>(url, userAccessToken, new FormUrlEncodedContent(payoutCreate.ToDictionary())),
            _ => Task.FromResult(new MoneyMoovApiResponse<Payout>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov payout endpoint to get a single payout by ID.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="payoutID">The ID of the payout to retrieve.</param>
    /// <returns>If successful, a payout object.</returns>
    public Task<MoneyMoovApiResponse<Payout>> GetPayoutAsync(string userAccessToken, Guid payoutID)
    {
        var url = MoneyMoovUrlBuilder.PayoutsApi.PayoutUrl(_apiClient.GetBaseUri().ToString(), payoutID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetPayoutAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<Payout>(url, userAccessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse<Payout>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov payout endpoint to get a single payout by invoice ID. This method
    /// requires a merchant access token as invoice ID's are only unique across a single merchant.
    /// </summary>
    /// <param name="invoiceID">The invoice ID of the payout to retrieve.</param>
    /// <returns>If successful, a payout object.</returns>
    public Task<MoneyMoovApiResponse<Payout>> GetPayoutByInvoiceIDAsync(string merchantAccessToken, string invoiceID)
    {
        var url = MoneyMoovUrlBuilder.PayoutsApi.GetByInvoiceIDUrl(_apiClient.GetBaseUri().ToString(), invoiceID);

        var prob = _apiClient.CheckAccessToken(merchantAccessToken, nameof(GetPayoutByInvoiceIDAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<Payout>(url, merchantAccessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse<Payout>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Payout endpoint to submit an existing payout for processing. This call initiates
    /// the movement of money.
    /// </summary>
    /// <param name="strongUserAccessToken">The strong user access token authorised to submit payout. Strong
    /// tokens can only be acquired from a strong customer authentication flow, are short lived (typically 5 minute expiry)
    /// and are specific to the payout being submitted.</param>
    /// <param name="payoutID">The ID of the payout to submit for processing.</param>
    /// <returns>An API response indicating the result of the submit attempt.</returns>
    public Task<MoneyMoovApiResponse> SubmitPayoutAsync(string strongUserAccessToken, Guid payoutID)
    {
        var url = MoneyMoovUrlBuilder.PayoutsApi.SubmitPayoutUrl(_apiClient.GetBaseUri().ToString(), payoutID);

        var prob = _apiClient.CheckAccessToken(strongUserAccessToken, nameof(UpdatePayoutAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync(url, strongUserAccessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Payout endpoint to update an existing payout.
    /// </summary>
    /// <param name="accessToken">The user, or merchant, access token updating the payout.</param>
    /// <param name="payoutID">The ID of the payout to update.</param>
    /// <param name="payoutUpdate">A model with the details of the payout fields being updated.</param>
    /// <returns>An API response indicating the result of the update attempt.</returns>
    public Task<MoneyMoovApiResponse<Payout>> UpdatePayoutAsync(string userAccessToken, Guid payoutID, PayoutUpdate payoutUpdate)
    {
        var url = MoneyMoovUrlBuilder.PayoutsApi.PayoutUrl(_apiClient.GetBaseUri().ToString(), payoutID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(UpdatePayoutAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PutAsync<Payout>(url, userAccessToken, new FormUrlEncodedContent(payoutUpdate.ToDictionary())),
            _ => Task.FromResult(new MoneyMoovApiResponse<Payout>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov payout endpoint to get a list of the payouts grouped by a batch ID.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="batchPayoutID">The batch ID of the payouts to retrieve.</param>
    /// <returns>If successful, a list of payout objects.</returns>
    public Task<MoneyMoovApiResponse<BatchPayout>> GetBatchPayoutAsync(string userAccessToken, Guid batchPayoutID)
    {
        var url = MoneyMoovUrlBuilder.PayoutsApi.BatchPayoutUrl(_apiClient.GetBaseUri().ToString(), batchPayoutID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetBatchPayoutAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<BatchPayout>(url, userAccessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse<BatchPayout>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Payout endpoint to create a new batch payout. Batch payouts a re a grouping of payouts from
    /// a single merchant that can be approved and submitted at once.
    /// </summary>
    /// <param name="userAccessToken">The access token of the user creating the batch payout.</param>
    /// <param name="payoutIDs">A model with the list of payout IDs to create a batch from.</param>
    /// <returns>An API response indicating the result of the create attempt.</returns>
    public Task<MoneyMoovApiResponse<BatchPayout>> CreateBatchPayoutAsync(string userAccessToken, List<Guid> payoutIDs)
    {
        var url = MoneyMoovUrlBuilder.PayoutsApi.BatchPayoutUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(CreateBatchPayoutAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<BatchPayout>(url, userAccessToken, JsonContent.Create(payoutIDs)),
            _ => Task.FromResult(new MoneyMoovApiResponse<BatchPayout>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
