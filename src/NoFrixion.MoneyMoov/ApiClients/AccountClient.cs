//-----------------------------------------------------------------------------
// Filename: AccountClient.cs
//
// Description: An API client to call the MoneyMoov Account API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 11 Dec 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;

namespace NoFrixion.MoneyMoov;

public interface IAccountClient
{
    Task<RestApiResponse<PaymentAccount>> GetAccountAsync(string userAccessToken, Guid accountID);

    Task<RestApiResponse<PaymentAccount>> CreateAccountAsync(string userAccessToken, PaymentAccountCreate accountCreate);
}

public class AccountClient : IAccountClient
{
    private readonly ILogger _logger;
    private readonly IRestApiClient _apiClient;

    public AccountClient(IRestApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public AccountClient(IRestApiClient apiClient, ILogger<AccountClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov Accounts endpoint to get the details for a single payment account.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="accountID">The ID of the account to retrieve.</param>
    /// <returns>If successful, a the details for the payment account.</returns>
    public Task<RestApiResponse<PaymentAccount>> GetAccountAsync(string userAccessToken, Guid accountID)
    {
        var url = MoneyMoovUrlBuilder.AccountsApi.AccountUrl(_apiClient.GetBaseUri().ToString(), accountID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetAccountAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<PaymentAccount>(url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse<PaymentAccount>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov account endpoint to create a new payment account.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="accountCreate">The details of the token to create.</param>
    /// <returns>If successful, the newly created merchant token.</returns>
    public Task<RestApiResponse<PaymentAccount>> CreateAccountAsync(string userAccessToken, PaymentAccountCreate accountCreate)
    {
        var url = MoneyMoovUrlBuilder.AccountsApi.AccountsUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(CreateAccountAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<PaymentAccount>(url, userAccessToken, new FormUrlEncodedContent(accountCreate.ToDictionary())),
            _ => Task.FromResult(new RestApiResponse<PaymentAccount>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
