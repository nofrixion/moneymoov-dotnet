// -----------------------------------------------------------------------------
//  Filename: WebHookRequest.cs
// 
//  Description: Biz model containing info for creating/updating a ModulR webhook.
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  04 02 2022  Donal O'Connor   Created, Carmichael House,
//  Dublin, Ireland.
//  22 Sep 2022 Aaron Clauson   Added MerchantID property.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class WebHookRequest : IValidatableObject
{
    public Guid ID { get; set; }

    [Required]
    public Guid MerchantID { get; set; }

    [Required]
    public WebhookEvent Type { get; set; }

    [Required]
    public string? DestinationUrl { get; set; }

    public bool Retry { get; set; } = true;

    [Required]
    public string? Secret { get; set; }

    public bool IsActive { get; set; } = true;

    public string? EmailAddress { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Secret?.Length != 32)
        {
            yield return new ValidationResult("Invalid Secret provided. Secret must be 32 characters long.");
        }
    }

    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        dict.Add(nameof(ID), ID.ToString());
        dict.Add(nameof(MerchantID), MerchantID.ToString());
        dict.Add(nameof(Type), Type.ToString());
        dict.Add(nameof(DestinationUrl), DestinationUrl ?? string.Empty);
        dict.Add(nameof(Retry), Retry.ToString());
        dict.Add(nameof(Secret), Secret ?? string.Empty);
        dict.Add(nameof(IsActive), IsActive.ToString());
        dict.Add(nameof(EmailAddress), EmailAddress ?? string.Empty);

        return dict;
    }
}