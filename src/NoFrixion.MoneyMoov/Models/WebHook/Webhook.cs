// -----------------------------------------------------------------------------
//  Filename: Webhook.cs
// 
//  Description: The model class for a merchant webhook:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  07 04 2022  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;

namespace NoFrixion.MoneyMoov.Models;

public class Webhook
{
    /// <summary>
    /// The name of the HTTP header that the MoneyMoov API server sets for teh webhook's
    /// payload signature.
    /// </summary>
    public const string MONEYMOOV_SIGNATURE_HEADER = "x-moneymoov-signature";

    public Guid ID { get; set; }

    public WebhookEventTypesEum Type { get; set; }

    public string? DestinationUrl { get; set; }

    public bool Retry { get; set; } = true;

    public string? Secret { get; set; }

    public bool IsActive { get; set; } = true;

    public string? EmailAddress { get; set; }

    public static string GetSignature(string secret, byte[] payloadBytes)
    {
        byte[] keyByte = ASCIIEncoding.UTF8.GetBytes(secret);

        using (var hmac = new HMACSHA256(keyByte))
        {
            byte[] hashMessage = hmac.ComputeHash(payloadBytes);
            var hashMsg = Convert.ToBase64String(hashMessage);
            return Convert.ToBase64String(hashMessage);
        }
    }
}