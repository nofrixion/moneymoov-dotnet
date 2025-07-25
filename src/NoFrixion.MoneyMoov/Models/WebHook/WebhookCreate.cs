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

using NoFrixion.MoneyMoov.Attributes;
using NoFrixion.MoneyMoov.Enums;
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

    /// <summary>
    /// The destination URL for the webhook.
    /// Required for webhook notifications.
    /// </summary>
    public string? DestinationUrl { get; set; }

    public bool Retry { get; set; } = true;

    /// <summary>
    /// The secret key required to authenticate webhook notifications.
    /// Required for webhook notifications.
    /// </summary>
    public string? Secret { get; set; }

    public bool IsActive { get; set; } = true;

    /// <summary>
    /// The recipient email address(es) for notifications. Multiple addresses can be separated by a comma, semicolon, or space.
    /// Reruired for email notifications.
    /// </summary>
    [EmailAddressMultiple(ErrorMessage = "One or more of the email addresses are invalid. Addresses can be separated by a comma, semi-colon or space.")]
    public string? EmailAddress { get; set; }

    /// <summary>
    /// The email address to which notifications about failed webhook deliveries will be sent.
    /// </summary>
    [EmailAddressMultiple(ErrorMessage = "One or more of the email addresses are invalid. Addresses can be separated by a comma, semi-colon or space.")]
    public string? FailedNotificationEmailAddress { get; set; }

    /// <summary>
    /// The type of notification that will be sent.
    /// </summary>
    [Required]
    public NotificationMethodTypesEnum NotificationMethod { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Secret?.Length > SECRET_MAX_LENGTH)
        {
            yield return new ValidationResult($"The Secret string was too long. The Secret maximum length is {SECRET_MAX_LENGTH} characters.");
        }

        if (ResourceTypes == null || ResourceTypes.Count() == 0)
        {
            yield return new ValidationResult("Cannot create a webhook with an empty resource type list. Please specify at least one resource type.");
        }

        if (ResourceTypes?.Contains(WebhookResourceTypesEnum.None) == true)
        {
            yield return new ValidationResult("Cannot create a webhook with a resource type of none.");
        }

        if (NotificationMethod is NotificationMethodTypesEnum.None)
        {
            yield return new ValidationResult("Cannot create a webhook with a notification method type of none.");
        }

        if (NotificationMethod is NotificationMethodTypesEnum.Email
            && string.IsNullOrWhiteSpace(EmailAddress))
        {
            yield return new ValidationResult("Email address is required for email notification method.");
        }

        if (NotificationMethod is NotificationMethodTypesEnum.Webhook
            && (string.IsNullOrWhiteSpace(DestinationUrl) || string.IsNullOrWhiteSpace(Secret)))
        {
            yield return new ValidationResult("Destination URL and Secret are required for webhook notification method.");
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
            { nameof(EmailAddress), EmailAddress ?? string.Empty },
            { nameof(FailedNotificationEmailAddress), FailedNotificationEmailAddress ?? string.Empty },
            { nameof(NotificationMethod), NotificationMethod.ToString() }
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