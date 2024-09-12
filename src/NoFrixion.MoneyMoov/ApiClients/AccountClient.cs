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
using System.Net.Http.Headers;

namespace NoFrixion.MoneyMoov;

public interface IAccountClient
{
    Task<RestApiResponse<PaymentAccount>> GetAccountAsync(string userAccessToken, Guid accountID);

    Task<RestApiResponse<PaymentAccount>> CreateAccountAsync(string userAccessToken, PaymentAccountCreate accountCreate);
    
    Task<RestApiFileResponse> GetStatementAsync(string accessToken, Guid accountID, Guid statementID);
    
    Task<RestApiResponse> ClearStatementsAsync(string accessToken);
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

    /// <summary>
    /// Calls the MoneyMoov statements endpoint to get a statement file.
    /// </summary>
    /// <param name="accessToken">A User or Merchant scoped JWT access token.</param>
    /// <param name="accountID">The account ID</param>
    /// <param name="statementID">The ID of the report to retrieve the result for.</param>
    /// <returns>If successful, the statement result.</returns>
    public async Task<RestApiFileResponse> GetStatementAsync(string accessToken, Guid accountID, Guid statementID)
    {
        var url = MoneyMoovUrlBuilder.AccountsApi.StatementsUrl(_apiClient.GetBaseUri().ToString(), accountID, statementID);
        var prob = _apiClient.CheckAccessToken(accessToken, nameof(GetStatementAsync));

        if (!prob.IsEmpty)
        {
            return new RestApiFileResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob);
        }

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/pdf";
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('\"') ?? "downloaded_file";

                return new RestApiFileResponse(HttpStatusCode.OK, new Uri(url), response.Headers, content, contentType, fileName);
            }
            else
            {
                return new RestApiFileResponse(
                    response.StatusCode, 
                    new Uri(url), 
                    new NoFrixionProblem(response.ReasonPhrase ?? "File download failed.", (int)response.StatusCode));
            }
        }
    }

    /// <summary>
    /// Calls the MoneyMoov Statements endpoint to clear the user's cached statements. This allows them to be re-generated, for example
    /// if the statement was for the current month and the month has not yet completed.
    /// </summary>
    /// <param name="accessToken">The user token deleting the payout.</param>
    public Task<RestApiResponse> ClearStatementsAsync(string accessToken)
    {
        var url = MoneyMoovUrlBuilder.AccountsApi.StatementsUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(ClearStatementsAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.DeleteAsync(url, accessToken),
            _ => Task.FromResult(new RestApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
