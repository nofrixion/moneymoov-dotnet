﻿//-----------------------------------------------------------------------------
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
    public const int SECRET_MAX_LENGTH = 32;

    public Guid ID { get; set; }

    [Required]
    public Guid MerchantID { get; set; }

    [Required]
    public WebhookResourceTypesEnum Type { get; set; }

    [Required]
    public string? DestinationUrl { get; set; }

    public bool Retry { get; set; } = true;

    [Required]
    public string? Secret { get; set; }

    public bool IsActive { get; set; } = true;

    public string? EmailAddress { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Secret?.Length > SECRET_MAX_LENGTH)
        {
            yield return new ValidationResult($"The Secret string was too long. The Secret maximum length is {SECRET_MAX_LENGTH} characters.");
        }

        if (Type == WebhookResourceTypesEnum.None)
        {
            yield return new ValidationResult("Can not create webhook with type none.");
        }
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