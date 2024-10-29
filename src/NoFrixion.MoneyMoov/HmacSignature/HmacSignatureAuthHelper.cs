// -----------------------------------------------------------------------------
//  Filename: HmacSignatureAuthHelper.cs
// 
//  Description: Used for generating HMAC signatures and verifying them.
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  08 02 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace NoFrixion.MoneyMoov;

public static class HmacSignatureAuthHelper
{
    public static Dictionary<string, string> GetAppHeaders(string appId,
        string idempotencyKey,
        string secret,
        DateTime date,
        Guid merchantId)
    {
        var signature = GenerateSignature(idempotencyKey, date, secret, true);

        var headers = new Dictionary<string, string>
        {
            {HmacAuthenticationConstants.AUTHORIZATION_HEADER_NAME, GenerateAppAuthHeaderContent(appId, signature)},
            {HmacAuthenticationConstants.DATE_HEADER_NAME, date.ToString("R")},
            {HmacAuthenticationConstants.IDEMPOTENT_HEADER_NAME, idempotencyKey},
            {HmacAuthenticationConstants.MERCHANT_ID_HEADER_NAME, merchantId.ToString()},
        };

        return headers;
    }
    
    public static Dictionary<string, string> GetHeaders(string keyId,
        string nonce,
        string secret,
        DateTime date,
        bool asRetry = false)
    {
        var signature = GenerateSignature(nonce, date, secret);

        var headers = new Dictionary<string, string>
        {
            {HmacAuthenticationConstants.AUTHORIZATION_HEADER_NAME, GenerateAuthHeaderContent(keyId, signature)},
            {HmacAuthenticationConstants.DATE_HEADER_NAME, date.ToString("R")},
            {HmacAuthenticationConstants.NONCE_HEADER_NAME, nonce},
            {HmacAuthenticationConstants.HTTP_RETRY_HEADER_NAME, asRetry.ToString().ToLower()},
            {HmacAuthenticationConstants.NOFRIXION_SIGNATURE_HEADER_NAME, signature},
        };

        return headers;
    }

    public static string GenerateSignature(string nonce, DateTime date, string secret, bool hmac256 = false)
    {
        return hmac256 ? 
            HashAndEncode256($"date: {date:R}\n{HmacAuthenticationConstants.IDEMPOTENT_HEADER_NAME}: {nonce}", secret) : 
            HashAndEncode($"date: {date:R}\n{HmacAuthenticationConstants.NONCE_HEADER_NAME}: {nonce}", secret);
    }
    
    private static string GenerateAppAuthHeaderContent(string apiKey, string signature)
    {
        return $"Signature appId=\"{apiKey}\",headers=\"date {HmacAuthenticationConstants.IDEMPOTENT_HEADER_NAME}\",signature=\"{signature}\"";
    }
    
    private static string GenerateAuthHeaderContent(string apiKey, string signature)
    {
        return $"Signature keyId=\"{apiKey}\",headers=\"date x-mod-nonce\",signature=\"{signature}\"";
    }
    
    private static string HashAndEncode(string message, string secret)
    {
        var ascii = Encoding.ASCII;
        
        HMACSHA1 hmac = new HMACSHA1(ascii.GetBytes(secret));
        hmac.Initialize();

        byte[] messageBuffer = ascii.GetBytes(message);
        byte[] hash = hmac.ComputeHash(messageBuffer);

        return WebUtility.UrlEncode(Convert.ToBase64String(hash));
    }
    
    private static string HashAndEncode256(string message, string secret)
    {
        var ascii = Encoding.ASCII;
        
        var hmac = new HMACSHA256(ascii.GetBytes(secret));
        hmac.Initialize();

        var messageBuffer = ascii.GetBytes(message);
        var hash = hmac.ComputeHash(messageBuffer);

        return WebUtility.UrlEncode(Convert.ToBase64String(hash));
    }
}