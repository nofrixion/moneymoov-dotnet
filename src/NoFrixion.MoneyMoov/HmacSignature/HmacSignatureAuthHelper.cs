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
    /// <summary>
    /// Original HMAC signature format.
    /// </summary>
    public const int LEGACY_SIGNATURE_VERSION = 0;

    /// <summary>
    /// The current default signature version for the NoFrixion Trusted Third Party (TPP) API key and
    /// mercant token requests.
    /// </summary>
    public const int DEFAULT_SIGNATURE_VERSION = 1;

    /// <summary>
    /// The HMAC SHA256 signature for the NoFrixion Trusted Third Party (TPP) API key requests. 
    /// </summary>
    /// <param name="appId">The ID of the TPP application the signature is for.</param>
    /// <param name="idempotencyKey">A pseudo-random nonce.</param>
    /// <param name="secret">The shared HMAC secret.</param>
    /// <param name="date">The current date time.</param>
    /// <param name="merchantId">The ID of the merchant the request is for.</param>
    /// <returns>A list of headers for the ougoing request.</returns>
    public static Dictionary<string, string> GetAppHeaders(string appId,
        string idempotencyKey,
        string secret,
        DateTime date,
        Guid merchantId)
    {
        var signature = GenerateSignature(idempotencyKey, date, secret, DEFAULT_SIGNATURE_VERSION, SharedSecretAlgorithmsEnum.HMAC_SHA256);

        var headers = new Dictionary<string, string>
        {
            {HmacAuthenticationConstants.AUTHORIZATION_HEADER_NAME, GenerateAppAuthHeaderContent(appId, signature)},
            {HmacAuthenticationConstants.DATE_HEADER_NAME, date.ToString("R")},
            {HmacAuthenticationConstants.IDEMPOTENT_HEADER_NAME, idempotencyKey},
            {HmacAuthenticationConstants.MERCHANT_ID_HEADER_NAME, merchantId.ToString()},
        };

        return headers;
    }
    
    /// <summary>
    /// The original HMAC SHA1 signature used in version 1 webhooks which.
    /// </summary>
    /// <param name="keyId">The key ID, or webhook entity ID, the signature is being generated for.</param>
    /// <param name="nonce">A pseudo-random nonce.</param>
    /// <param name="secret">The shared HMAC secret.</param>
    /// <param name="date">The current date time.</param>
    /// <param name="asRetry">True if a retry header should be added. Not used in the HMAC.</param>
    /// <returns>A list of headers for the ougoing request.</returns>
    public static Dictionary<string, string> GetHeaders(string keyId,
        string nonce,
        string secret,
        DateTime date,
        bool asRetry = false)
    {
        var signature = GenerateSignature(nonce, date, secret, LEGACY_SIGNATURE_VERSION, SharedSecretAlgorithmsEnum.HMAC_SHA1);

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

    public static string GenerateSignature(string nonce, DateTime date, string secret, int signatureVersion, SharedSecretAlgorithmsEnum algorithm)
     => GenerateSignature(nonce, date, Encoding.UTF8.GetBytes(secret), signatureVersion, algorithm);

    public static string GenerateSignature(string nonce, DateTime date, byte[] secret, int signatureVersion, SharedSecretAlgorithmsEnum algorithm)
        => signatureVersion switch
        {
            0 => HashAndEncode($"date: {date:R}\n{HmacAuthenticationConstants.NONCE_HEADER_NAME}: {nonce}", 
                    secret,
                    algorithm),
            _ => HashAndEncode($"date: {date:R}\n{HmacAuthenticationConstants.IDEMPOTENT_HEADER_NAME}: {nonce}", 
                    secret, 
                    algorithm)
            // If the HMAC headers, or anything else that impacts the input hash, changes add a new signature version.
        };

    private static string GenerateAppAuthHeaderContent(string apiKey, string signature)
    {
        return $"Signature appId=\"{apiKey}\",headers=\"date {HmacAuthenticationConstants.IDEMPOTENT_HEADER_NAME}\",signature=\"{signature}\"";
    }
    
    private static string GenerateAuthHeaderContent(string apiKey, string signature)
    {
        return $"Signature keyId=\"{apiKey}\",headers=\"date x-mod-nonce\",signature=\"{signature}\"";
    }
    
    public static string HashAndEncode(string message, byte[] secret, SharedSecretAlgorithmsEnum algorithm)
    {
        if (algorithm == SharedSecretAlgorithmsEnum.None)
        {
            throw new ArgumentException("Algorithm must be specified.", nameof(algorithm));
        }

        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        byte[] hash;

        using (HMAC hmac = algorithm switch
        {
            SharedSecretAlgorithmsEnum.HMAC_SHA1 => new HMACSHA1(secret),
            SharedSecretAlgorithmsEnum.HMAC_SHA256 => new HMACSHA256(secret),
            SharedSecretAlgorithmsEnum.HMAC_SHA384 => new HMACSHA384(secret),
            SharedSecretAlgorithmsEnum.HMAC_SHA512 => new HMACSHA512(secret),
            _ => throw new NotSupportedException($"The algorithm {algorithm} is not supported.")
        })
        {
            hash = hmac.ComputeHash(messageBytes);
        }

        return WebUtility.UrlEncode(Convert.ToBase64String(hash));
    }
}