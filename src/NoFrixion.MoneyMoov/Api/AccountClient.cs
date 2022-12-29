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
    //Task<MoneyMoovApiResponse<IEnumerable<PaymentAccount>>> GetAccountsAsync(string userAccessToken, Guid merchantID);

    Task<MoneyMoovApiResponse<PaymentAccount>> CreateAccountAsync(string userAccessToken, PaymentAccountCreate token);
}

public class AccountClient : IAccountClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public AccountClient(IMoneyMoovApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public AccountClient(IMoneyMoovApiClient apiClient, ILogger<AccountClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant get accounts endpoint to get the list of all the merchant's
    /// payment accounts.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant to get the payment accounts for.</param>
    /// <returns>If successful, a list of payment accounts for the merchant.</returns>
    //public Task<MoneyMoovApiResponse<IEnumerable<PaymentAccount>>> GetAccountsAsync(string userAccessToken, Guid merchantID)
    //{
    //    var url = MoneyMoovUrlBuilder.AccountsApi.GetAccountsApiUrl(_apiClient.GetBaseUri().ToString(), merchantID);

    //    var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetAccountsAsync));

    //    return prob switch
    //    {
    //        var p when p.IsEmpty => _apiClient.GetAsync<IEnumerable<PaymentAccount>>(url, userAccessToken),
    //        _ => Task.FromResult(new MoneyMoovApiResponse<IEnumerable<PaymentAccount>>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
    //    };
    //}

    /// <summary>
    /// Calls the MoneyMoov account endpoint to create a new payment account.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="account">The details of the token to create.</param>
    /// <returns>If successful, the newly created merchant token.</returns>
    public Task<MoneyMoovApiResponse<PaymentAccount>> CreateAccountAsync(string userAccessToken, PaymentAccountCreate account)
    {
        var url = MoneyMoovUrlBuilder.AccountsApi.AccountsUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(CreateAccountAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<PaymentAccount>(url, userAccessToken, new FormUrlEncodedContent(account.ToDictionary())),
            _ => Task.FromResult(new MoneyMoovApiResponse<PaymentAccount>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
