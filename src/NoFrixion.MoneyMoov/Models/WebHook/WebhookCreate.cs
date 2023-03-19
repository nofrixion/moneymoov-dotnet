//-----------------------------------------------------------------------------
// Filename: WebhookCreate.cs
// 
// Description: Model for creating/updating a merchant webhook.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 12 Mar 2023 Aaron Clauson   Created, based on WebHookRequest class.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class WebhookCreate : IValidatableObject
{
    public Guid ID { get; set; }

    [Required]
    public Guid MerchantID { get; set; }

    [Required]
    public WebhookEventTypesEum Type { get; set; }

    [Required]
    public string? DestinationUrl { get; set; }

    public bool Retry { get; set; } = true;

    [Required]
    public string? Secret { get; set; }

    public bool IsActive { get; set; } = true;

    public string? EmailAddress { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        yield return ValidationResult.Success!;
    }

    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { nameof(ID), ID.ToString() },
            { nameof(MerchantID), MerchantID.ToString() },
            { nameof(Type), Type.ToString() },
            { nameof(DestinationUrl), DestinationUrl ?? string.Empty },
            { nameof(Retry), Retry.ToString() },
            { nameof(Secret), Secret ?? string.Empty },
            { nameof(IsActive), IsActive.ToString() },
            { nameof(EmailAddress), EmailAddress ?? string.Empty }
        };
    }
}