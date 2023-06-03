﻿//-----------------------------------------------------------------------------
// Filename: UserClient.cs
//
// Description: An API client to call the MoneyMoov User API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 30 Dec 2022  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;

namespace NoFrixion.MoneyMoov;

public interface IUserClient
{
    Task<MoneyMoovApiResponse<User>> CreateUserAsync(UserCreate userCreate);

    Task<MoneyMoovApiResponse<User>> UpdateUserAsync(string accessToken, Guid userID, UserUpdate UserUpdate);

    Task<MoneyMoovApiResponse<UserToken>> UpdateUserTokenAsync(string accessToken, Guid userTokenID, UserTokenUpdate UserTokenUpdate);
}

public class UserClient : IUserClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public UserClient()
    {
        _apiClient = new MoneyMoovApiClient(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        _logger = NullLogger.Instance;
    }

    public UserClient(string baseUri)
    {
        _apiClient = new MoneyMoovApiClient(baseUri);
        _logger = NullLogger.Instance;
    }

    public UserClient(IMoneyMoovApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public UserClient(IMoneyMoovApiClient apiClient, ILogger<AccountClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant get user endpoint create a new user record.
    /// </summary>
    /// <param name="userCreate">The model containing the data about the new user to create.</param>
    /// <returns>If successful, a user object.</returns>
    public Task<MoneyMoovApiResponse<User>> CreateUserAsync(UserCreate userCreate)
    {
        var url = MoneyMoovUrlBuilder.UserApi.UserApiUrl(_apiClient.GetBaseUri().ToString());
        return _apiClient.PostAsync<User>(url, new FormUrlEncodedContent(userCreate.ToDictionary()));
    }

    /// <summary>
    /// Calls the MoneyMoov User endpoint to update an existing user record.
    /// </summary>
    /// <param name="accessToken">The user access token updating the user, typically will be the same as the user being updated.</param>
    /// <param name="userID">The ID of the user to update.</param>
    /// <param name="userUpdate">A model with the details of the user fields being updated.</param>
    /// <returns>An API response indicating the result of the update attempt.</returns>
    public Task<MoneyMoovApiResponse<User>> UpdateUserAsync(string userAccessToken, Guid userID, UserUpdate userUpdate)
    {
        var url = MoneyMoovUrlBuilder.UserApi.UserApiUrl(_apiClient.GetBaseUri().ToString(), userID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(UpdateUserAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PutAsync<User>(url, userAccessToken, new FormUrlEncodedContent(userUpdate.ToDictionary())),
            _ => Task.FromResult(new MoneyMoovApiResponse<User>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov User endpoint to update an existing user token.
    /// </summary>
    /// <param name="accessToken">The user access token updating the application user token.</param>
    /// <param name="userTokenID">The ID of the user token to update.</param>
    /// <param name="userTokenUpdate">A model with the details of the user token fields being updated.</param>
    /// <returns>An API response indicating the result of the update attempt.</returns>
    public Task<MoneyMoovApiResponse<UserToken>> UpdateUserTokenAsync(string userAccessToken, Guid userTokenID, UserTokenUpdate userTokenUpdate)
    {
        var url = MoneyMoovUrlBuilder.UserApi.UserTokenApiUrl(_apiClient.GetBaseUri().ToString(), userTokenID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(UpdateUserTokenAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PutAsync<UserToken>(url, userAccessToken, new FormUrlEncodedContent(userTokenUpdate.ToDictionary())),
            _ => Task.FromResult(new MoneyMoovApiResponse<UserToken>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
