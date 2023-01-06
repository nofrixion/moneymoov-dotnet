﻿//-----------------------------------------------------------------------------
// Filename: UserInivteClient.cs
//
// Description: An API client to call the MoneyMoov User Invite API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 13 Dec 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;

namespace NoFrixion.MoneyMoov;

public interface IUserInviteClient
{
    Task<MoneyMoovApiResponse> SendInviteAsync(string userAccessToken, Guid merchantID, string inviteeEmailAddress, string inviteRegistrationUrl,
        bool sendInviteEmail, string inviteeName);
}

public class UserInviteClient : IUserInviteClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public UserInviteClient()
    {
        _apiClient = new MoneyMoovApiClient(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        _logger = NullLogger.Instance;
    }

    public UserInviteClient(string baseUri)
    {
        _apiClient = new MoneyMoovApiClient(baseUri);
        _logger = NullLogger.Instance;
    }

    public UserInviteClient(IMoneyMoovApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public UserInviteClient(IMoneyMoovApiClient apiClient, ILogger<AccountClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant get user invite endpoint to get a single user invite by ID.
    /// When called anonymously no personally identifiable information will be returned. Calling
    /// anonymously is useful to check if the user invite exists and that it hasn't expired.
    /// </summary>
    /// <param name="userInviteID">The ID of the user invite to retrieve.</param>
    /// <returns>If successful, a user invite object.</returns>
    public Task<MoneyMoovApiResponse<UserInvite>> GetUserInviteAsync(Guid userInviteID)
    {
        var url = MoneyMoovUrlBuilder.UserInvitesApi.UserInviteUrl(_apiClient.GetBaseUri().ToString(), userInviteID);
        return _apiClient.GetAsync<UserInvite>(url);
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant get user invite endpoint to get a single user invite by ID.
    /// </summary>
    /// <param name="accessToken">A User scoped JWT access token.</param>
    /// <param name="userInviteID">The ID of the user invite to retrieve.</param>
    /// <returns>If successful, a user invite object.</returns>
    public Task<MoneyMoovApiResponse<UserInvite>> GetUserInviteAsync(string accessToken, Guid userInviteID)
    {
        var url = MoneyMoovUrlBuilder.UserInvitesApi.UserInviteUrl(_apiClient.GetBaseUri().ToString(), userInviteID);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(GetUserInviteAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<UserInvite>(url, accessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse<UserInvite>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov account endpoint to create and send an email invite to register
    /// and join a merchant account.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant the user is being invited to join.</param>
    /// <param name="inviteeEmailAddress">The email address of the user to invite.</param>
    public Task<MoneyMoovApiResponse> SendInviteAsync(string userAccessToken, Guid merchantID, 
        string inviteeEmailAddress, 
        string inviteRegistrationUrl, 
        bool sendInviteEmail,
        string inviteeName)
    {
        var url = MoneyMoovUrlBuilder.UserInvitesApi.UserInvitesUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(SendInviteAsync));

        UserInviteCreate userInviteCreate = new UserInviteCreate
        {
            MerchantID = merchantID,
            InviteeEmailAddress = inviteeEmailAddress,
            RegistrationUrl = inviteRegistrationUrl,
            SendInviteEmail = sendInviteEmail,
            InviteeName = inviteeName
        };

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync(url, userAccessToken, new FormUrlEncodedContent(userInviteCreate.ToDictionary())),
            _ => Task.FromResult(new MoneyMoovApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
