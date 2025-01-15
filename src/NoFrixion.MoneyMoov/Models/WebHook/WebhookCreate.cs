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
    public const int SECRET_MAX_LENGTH = 32;

    public Guid ID { get; set; }

    [Required]
    public Guid MerchantID { get; set; }

    [Obsolete("This field has been deprecated. Please use ResourceTypes instead.")]
    public WebhookResourceTypesEnum Type
    {
        get =>
            ResourceTypes.Any() ? ResourceTypes.ToFlagEnum() : WebhookResourceTypesEnum.None;

        init
        {
            if (value == WebhookResourceTypesEnum.None)
            {
                ResourceTypes.Clear();
            }
            else
            {
                ResourceTypes = value.ToList();
            }
        }
    }

    /// <summary>
    /// The resource types that the webhook should be generated for.
    /// </summary>
    public List<WebhookResourceTypesEnum> ResourceTypes { get; set; } = new List<WebhookResourceTypesEnum>();

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

        if (ResourceTypes.Contains(WebhookResourceTypesEnum.None))
        {
            yield return new ValidationResult("Can not create webhook with a resource type of none.");
        }
    }

    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>
        {
            { nameof(ID), ID.ToString() },
            { nameof(MerchantID), MerchantID.ToString() },
            { nameof(DestinationUrl), DestinationUrl ?? string.Empty },
            { nameof(Retry), Retry.ToString() },
            { nameof(Secret), Secret ?? string.Empty },
            { nameof(IsActive), IsActive.ToString() },
            { nameof(EmailAddress), EmailAddress ?? string.Empty }
        };

        if (ResourceTypes?.Count() > 0)
        {
            int resourceTypeNumber = 0;
            foreach (var resourceType in ResourceTypes)
            {
                dict.Add($"{nameof(ResourceTypes)}[{resourceTypeNumber}]", resourceType.ToString());
                resourceTypeNumber++;
            }
        }

        return dict;
    }
}