//-----------------------------------------------------------------------------
// Filename: WebhookEventItem.cs
// 
// Description: Model for the individual items that can be sent in a webhook event.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 28 May 2023 Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class WebhookEventItem<T> where T : notnull, IWebhookPayload
{
    /// <summary>
    /// Timestamp the event occurred at.
    /// </summary>
    public DateTimeOffset EventDate { get; set; }

    /// <summary>
    /// The type of event the webhook is for, for example created, updated,
    /// deleted etc.
    /// </summary>
    [EnumDataType(typeof(WebhookActionsEnum))]
    [JsonConverter(typeof(StringEnumConverter))]
    public WebhookActionsEnum ActionType { get; set; }

    /// <summary>
    /// The type of resource the webhook item represents, for example
    /// payload, transaction etc.
    /// </summary>
    [EnumDataType(typeof(WebhookResourceTypesEnum))]
    [JsonConverter(typeof(StringEnumConverter))]
    public WebhookResourceTypesEnum ResourceType { get; set; }

    /// <summary>
    /// The payload holding the details of the item that the webhook is for.
    /// </summary>
    public T Payload { get; set; }

    public WebhookEventItem(
        DateTimeOffset eventDate,
        WebhookActionsEnum actionType,
        WebhookResourceTypesEnum resourceType,
        T payload)
    {
        EventDate = eventDate;
        ActionType = actionType;
        ResourceType = resourceType;
        Payload = payload;
    }
}

