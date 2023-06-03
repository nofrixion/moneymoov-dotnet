//-----------------------------------------------------------------------------
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
    Task<MoneyMoovApiResponse<UserInvite>> GetUserInviteAsync(Guid userInviteID);

    Task<MoneyMoovApiResponse<UserInvite>> GetUserInviteAsync(string accessToken, Guid userInviteID);

    Task<MoneyMoovApiResponse<UserInvite>> SendInviteAsync(string userAccessToken, Guid merchantID, string inviteeEmailAddress, string inviteRegistrationUrl,
        bool sendInviteEmail, string inviteeFirstName, string inviteeLastName);

    Task<MoneyMoovApiResponse> ResendUserInviteAsync(Guid userInviteID);

    Task<MoneyMoovApiResponse> ResendUserInviteAsync(string accessToken, Guid userInviteID);
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
    public Task<MoneyMoovApiResponse<UserInvite>> SendInviteAsync(string userAccessToken, Guid merchantID, 
        string inviteeEmailAddress, 
        string inviteRegistrationUrl, 
        bool sendInviteEmail,
        string inviteeFirstName,
        string inviteeLastName)
    {
        var url = MoneyMoovUrlBuilder.UserInvitesApi.UserInvitesUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(SendInviteAsync));

        UserInviteCreate userInviteCreate = new UserInviteCreate
        {
            MerchantID = merchantID,
            InviteeEmailAddress = inviteeEmailAddress,
            RegistrationUrl = inviteRegistrationUrl,
            SendInviteEmail = sendInviteEmail,
            InviteeFirstName = inviteeFirstName,
            InviteeLastName = inviteeLastName
        };

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<UserInvite>(url, userAccessToken, new FormUrlEncodedContent(userInviteCreate.ToDictionary())),
            _ => Task.FromResult(new MoneyMoovApiResponse<UserInvite>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov user invite endpoint to request a resend of an expired user invite.
    /// When called anonymously no personally the invite won't be automatically resent. Instead an email will
    /// be sent to the original inviter informing them of the request so they can decide whether to resend or not.
    /// </summary>
    /// <param name="userInviteID">The ID of the user invite to request a resend for.</param>
    public Task<MoneyMoovApiResponse> ResendUserInviteAsync(Guid userInviteID)
    {
        var url = MoneyMoovUrlBuilder.UserInvitesApi.UserInviteUrl(_apiClient.GetBaseUri().ToString(), userInviteID);
        return _apiClient.PutAsync(url);
    }

    /// <summary>
    /// Calls the MoneyMoov user invite endpoint to request a resend of an expired user invite.
    /// </summary>
    /// <param name="accessToken">A User scoped JWT access token.</param>
    /// <param name="userInviteID">The ID of the user invite to resend.</param>
    public Task<MoneyMoovApiResponse> ResendUserInviteAsync(string accessToken, Guid userInviteID)
    {
        var url = MoneyMoovUrlBuilder.UserInvitesApi.UserInviteUrl(_apiClient.GetBaseUri().ToString(), userInviteID);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(ResendUserInviteAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PutAsync(url, accessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
