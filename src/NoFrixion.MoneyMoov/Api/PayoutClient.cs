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
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;

namespace NoFrixion.MoneyMoov;

public interface IPayoutClient
{
    Task<MoneyMoovApiResponse<Payout>> CreatePayoutAsync(string userAccessToken, PayoutCreate payoutCreate);
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
}
