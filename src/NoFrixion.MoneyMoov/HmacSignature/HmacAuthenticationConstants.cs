// -----------------------------------------------------------------------------
//  Filename: HmacAuthenticationConstants.cs
// 
//  Description: Constants for HMAC authentication:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  22 04 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public static class HmacAuthenticationConstants
{
    public const string SIGNATURE_SCHEME_NAME = "Signature";
    public const string APP_ID_HEADER_NAME = "appId";
    public const string AUTHORIZATION_HEADER_NAME = "Authorization";
    public const string DATE_HEADER_NAME = "Date";
    public const string NONCE_HEADER_NAME = "x-mod-nonce";
    public const string MERCHANT_ID_HEADER_NAME = "x-nfx-merchantid";
    public const string NOFRIXION_SIGNATURE_HEADER_NAME = "x-nfx-signature";
    public const string HTTP_RETRY_HEADER_NAME = "x-mod-retry";
    public const string IDEMPOTENT_HEADER_NAME = "idempotency-key";
    public const string TOKEN_ID_PARAMETER_NAME = "tokenId";
}