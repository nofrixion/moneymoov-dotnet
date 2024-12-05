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

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class MerchantToken
{
    public Guid ID { get; set; }

    public Guid MerchantID { get; set; }

    public string Description { get; set; }

    public MerchantTokenPermissionsEnum Permissions { get; set; }

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
}