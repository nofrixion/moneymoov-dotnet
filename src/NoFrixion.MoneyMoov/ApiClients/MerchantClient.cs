//-----------------------------------------------------------------------------
// Filename: MerchantClient.cs
//
// Description: An API client to call the MoneyMoov Merchant API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 27 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;
using NoFrixion.MoneyMoov.Models.Roles;

namespace NoFrixion.MoneyMoov;

public interface IMerchantClient
{
    Task<RestApiResponse<MerchantToken>> GetMerchantTokenAsync(string userAccessToken, Guid tokenID);
    
    Task<RestApiResponse<MerchantTokenPageResponse>> GetMerchantTokensAsync(string userAccessToken, Guid merchantID);

    Task<RestApiResponse<MerchantToken>> CreateMerchantTokenAsync(string userAccessToken, TokenAdd token);

    [Obsolete($"Use {nameof(ITokensClient)}.{nameof(ITokensClient.ArchiveTokenAsync)} instead.")]
    Task<RestApiResponse> DeleteMerchantTokenAsync(string userAccessToken, Guid tokenID);

    Task<RestApiResponse<IEnumerable<UserRole>>> GetUserRolesAsync(string userAccessToken, Guid merchantID);

    Task<RestApiResponse<IEnumerable<PaymentAccount>>> GetAccountsAsync(string userAccessToken, Guid merchantID);

    Task<RestApiResponse<IEnumerable<UserInvite>>> GetUserInvitesAsync(string userAccessToken, Guid merchantID);
    
    Task<RestApiResponse<IEnumerable<Role>>> GetRolesAsync(string userAccessToken, Guid merchantID);
    
    Task<RestApiResponse<Role>> AddUserToRole(string userAccessToken, RoleUserCreate roleUserCreate, Guid merchantID, Guid roleID);
    
    Task<RestApiResponse> DeleteUserFromRole(string userAccessToken, Guid merchantID, Guid roleID, Guid userID);

    Task<RestApiResponse<User>> AssignRolesToUser(string userAccessToken, RolesUserCreate rolesUserCreate, Guid merchantID, Guid userID);

    Task<RestApiResponse> RemoveRolesFromUser(string userAccessToken, RolesUserDelete rolesUserDelete, Guid merchantID, Guid userID);
}

public class MerchantClient : IMerchantClient
{
    private readonly ILogger _logger;
    private readonly IRestApiClient _apiClient;

    public MerchantClient(string baseUri)
    {
        _apiClient = new RestApiClient(baseUri);
        _logger = NullLogger.Instance;
    }

    public MerchantClient(IRestApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public MerchantClient(IRestApiClient apiClient, ILogger<MerchantClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant get merchant token endpoint to get a specific merchant token.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="tokenID">The ID of the token to retrieve.</param>
    /// <returns>If successful, the merchant token details.</returns>
    public Task<RestApiResponse<MerchantToken>> GetMerchantTokenAsync(string userAccessToken, Guid tokenID)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.MerchantTokenUrl(_apiClient.GetBaseUri().ToString(), tokenID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetMerchantTokenAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<MerchantToken>(url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse<MerchantToken>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant get merchant tokens endpoint to get the list of all the merchant tokens
    /// that have been allocated for the merchant.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant to get the merchant tokens for.</param>
    /// <returns>If successful, a list of tokens for the merchant.</returns>
    public Task<RestApiResponse<MerchantTokenPageResponse>> GetMerchantTokensAsync(string userAccessToken, Guid merchantID)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.MerchantTokensUrl(_apiClient.GetBaseUri().ToString(), merchantID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetMerchantTokensAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<MerchantTokenPageResponse>(url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse<MerchantTokenPageResponse>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant create merchant token endpoint to create anew merchant scoped token.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="token">The details of the token to create.</param>
    /// <returns>If successful, the newly created merchant token.</returns>
    public Task<RestApiResponse<MerchantToken>> CreateMerchantTokenAsync(string userAccessToken, TokenAdd token)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.AllMerchantsTokensUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(CreateMerchantTokenAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<MerchantToken>(url, userAccessToken, new FormUrlEncodedContent(token.ToDictionary())),
            _ => Task.FromResult(new RestApiResponse<MerchantToken>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
    
    /// <summary>
    /// Calls the MoneyMoov Merchant delete merchant tokens endpoint to delete a single access token.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="tokenID">The ID of the token to delete.</param>
    /// <returns>If successful, a list of tokens for the merchant.</returns>
    [Obsolete($"Use {nameof(TokensClient)}.{nameof(TokensClient.ArchiveTokenAsync)} instead.")]
    public Task<RestApiResponse> DeleteMerchantTokenAsync(string userAccessToken, Guid tokenID)
    {
        var url = MoneyMoovUrlBuilder.TokensApi.TokenUrl(_apiClient.GetBaseUri().ToString(), tokenID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(DeleteMerchantTokenAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.DeleteAsync(url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov UserRoles Merchant endpoint to get the list of users who have been
    /// assigned a role on the merchant.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant to get the user roles for.</param>
    /// <returns>If successful, a list of the user role assignments for the merchant.</returns>
    public Task<RestApiResponse<IEnumerable<UserRole>>> GetUserRolesAsync(string userAccessToken, Guid merchantID)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.MerchantUserRolesUrl(_apiClient.GetBaseUri().ToString(), merchantID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetUserRolesAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<IEnumerable<UserRole>>(url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse<IEnumerable<UserRole>>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Accounts Merchant endpoint to get the list of the merchant's payment accounts.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant to get the payment accounts for.</param>
    /// <returns>If successful, a list of the payment accounts for the merchant.</returns>
    public Task<RestApiResponse<IEnumerable<PaymentAccount>>> GetAccountsAsync(string userAccessToken, Guid merchantID)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.MerchantAccountsUrl(_apiClient.GetBaseUri().ToString(), merchantID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetUserRolesAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<IEnumerable<PaymentAccount>>(url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse<IEnumerable<PaymentAccount>>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov UserInvites Merchant endpoint to get the list of invites that have been
    /// sent inviting new users to join the merchant.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant to get the invites for.</param>
    /// <returns>If successful, a list of the user invites for the merchant.</returns>
    public Task<RestApiResponse<IEnumerable<UserInvite>>> GetUserInvitesAsync(string userAccessToken, Guid merchantID)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.MerchantUserInvitesUrl(_apiClient.GetBaseUri().ToString(), merchantID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetUserRolesAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<IEnumerable<UserInvite>>(url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse<IEnumerable<UserInvite>>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    public Task<RestApiResponse<IEnumerable<Role>>> GetRolesAsync(string userAccessToken, Guid merchantID)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.MerchantRolesUrl(_apiClient.GetBaseUri().ToString(), merchantID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetRolesAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<IEnumerable<Role>>(url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse<IEnumerable<Role>>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
    
    public Task<RestApiResponse<Role>> AddUserToRole(string userAccessToken, RoleUserCreate roleUserCreate, Guid merchantID, Guid roleID)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.MerchantRoleUsersUrl(_apiClient.GetBaseUri().ToString(), merchantID, roleID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(AddUserToRole));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<Role>(url, userAccessToken, roleUserCreate.ToJsonContent()),
            _ => Task.FromResult(new RestApiResponse<Role>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
    
    public Task<RestApiResponse> DeleteUserFromRole(string userAccessToken, Guid merchantID, Guid roleID, Guid userID)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.MerchantRoleUsersUrl(_apiClient.GetBaseUri().ToString(), merchantID, roleID, userID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(DeleteUserFromRole));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.DeleteAsync(url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Roles Merchant endpoint to assign existing roles to a user.
    /// </summary>
    /// <param name="userAccessToken">A user scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant.</param>
    /// <param name="rolesUserCreate">The roles to be added to the user.</param>
    /// <param name="userID">The ID of the user.</param>
    /// <returns>If successful, updated user with roles.</returns>
    public Task<RestApiResponse<User>> AssignRolesToUser(string userAccessToken, RolesUserCreate rolesUserCreate, Guid merchantID, Guid userID)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.MerchantRolesUserUrl(_apiClient.GetBaseUri().ToString(), merchantID, userID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(AssignRolesToUser));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<User>(url, userAccessToken, rolesUserCreate.ToJsonContent()),
            _ => Task.FromResult(new RestApiResponse<User>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Roles Merchant endpoint to remove roles on a user.
    /// </summary>
    /// <param name="userAccessToken">A user scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant.</param>
    /// <param name="rolesUserDelete">The roles to be removed from the user.</param>
    /// <param name="userID">The ID of the user.</param>
    public Task<RestApiResponse> RemoveRolesFromUser(string userAccessToken, RolesUserDelete rolesUserDelete, Guid merchantID, Guid userID)
    {
        var url = MoneyMoovUrlBuilder.MerchantsApi.MerchantRolesUserUrl(_apiClient.GetBaseUri().ToString(), merchantID, userID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(RemoveRolesFromUser));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.DeleteAsync(url, userAccessToken, rolesUserDelete.ToJsonContent()),
            _ => Task.FromResult(new RestApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
