// -----------------------------------------------------------------------------
//  Filename: MerchantToken.cs
// 
//  Description: Biz friendly representation of the MerchantToken database model.
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  17 01 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using JetBrains.Annotations;
using NoFrixion.MoneyMoov.Enums;
using NoFrixion.MoneyMoov.Models.Approve;

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class MerchantToken
{
    public Guid ID { get; set; }

    public Guid MerchantID { get; set; }

    public string Description { get; set; }

    [Obsolete("This field has been deprecated. Please use PermissionTypes instead.")]
    public MerchantTokenPermissionsEnum Permissions
    {
        get =>
           PermissionTypes.Any() ? PermissionTypes.ToFlagEnum() : MerchantTokenPermissionsEnum.Deny;
    }

    /// <summary>
    /// The permissions that the merchant token supports.
    /// </summary>
    public List<MerchantTokenPermissionsEnum> PermissionTypes { get; set; } = new List<MerchantTokenPermissionsEnum>();

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// The JWT merchant token. It will only be available when the merchant token is
    /// initially created. The token is not stored by NoFrixion.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// If set to false the merchant token will not be accepted to authorise a request.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Optional shared secret algorithm to use for HMAC authentication.
    /// </summary>
    public SharedSecretAlgorithmsEnum SharedSecretAlgorithm { get; set; }

    /// <summary>
    /// The base 64 encoded shared secret that is used for request authentication with an HMAC.
    /// Note this property will ONLY be set when the token is initially created. It is not possible
    /// to retrieve the secret afterwards. If it is lost a new token should be created.
    /// </summary>
    public string SharedSecretBase64 { get; set; }

    /// <summary>
    /// Represent the version of the overall merchant token. This field is to allow the secret and public key mechanisms to
    /// vary over time. For example if the HTTP header fields to include in the algorithms change this version will faciliatate
    /// keeping track of which signature versions a particular merchant token is using.
    /// </summary>
    public int RequestSignatureVersion { get; set; }

    /// <summary>
    /// Optional. If set indicates the merchant token is not valid after the specified expiry date.
    /// </summary>
    public DateTimeOffset? ExpiresAt { get; set; }

    /// <summary>
    /// A list of users who have successfully authorised the latest version of the beneficiary.
    /// </summary>
    [CanBeNull] public List<Authorisation> Authorisations { get; set; }

    /// <summary>
    /// True if the merchant token can be authorised by the user who loaded it.
    /// </summary>
    public bool CanAuthorise { get; set; }

    /// <summary>
    /// True if the beneficiary was loaded for a user and that user has already authorised the latest version of the beneficiary.
    /// </summary>
    public bool HasCurrentUserAuthorised {  get; set; }
    
    /// <summary>
    /// The number of authorisers required for this merchant token. Is determined by business settings
    /// on the source account and/or merchant.
    /// </summary>
    public int AuthorisersRequiredCount { get; set; }

    /// <summary>
    /// The number of distinct authorisers that have authorised the merchant token.
    /// </summary>
    public int AuthorisersCompletedCount { get; set; }

    /// <summary>
    /// A list of authentication types allowed to authorise the merchant token.
    /// </summary>
    public List<AuthenticationTypesEnum> AuthenticationMethods { get; set; }
    
    public DateTimeOffset? LastAuthorised { get; set; }

    /// <summary>
    /// Optional. If set represents a comma separated list of IP addresses that this token is authorised to be used from.
    /// Attempts to use the token from an IP address not in the list will be rejected.
    /// </summary>
    public string IPAddressWhitelist { get; set; }

    /// <summary>
    /// Optional. If set to greater than 0 indicates the token is enabled for shared secret (HMAC) authentication. This
    ///  field represents the version of the master key, contained in the app settings, that was used to derive the shared secret key.
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public int SharedSecretMasterKeyVersion { get; set; }

    /// <summary>
    /// Indicates whether the merchant token is archived.
    /// </summary>
    public bool IsArchived { get; set; }

    /// <summary>
    /// Gets a hash of the critical fields for the beneficiary. This hash is
    /// used to ensure a beneficiary's details are not modified between the time the
    /// authorisation is given and the time the beneficiary is enabled.
    /// </summary>
    /// <returns>A hash of the beneficiary's critical fields.</returns>
    public string GetApprovalHash()
    {
        var input =
            Description +
            MerchantID +
            GetPermissionListHash();

        return HashHelper.CreateHash(input);
    }
    
    private string GetPermissionListHash() => string.Join(',', PermissionTypes);
}