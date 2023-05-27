//-----------------------------------------------------------------------------
// Filename: WebhookEvent.cs
// 
// Description: Model for sending webhook events to registered endpoints.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 26 May 2023 Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class WebhookEvent<T> where T : notnull, IWebhookPayload
{
    /// <summary>
    /// A unique ID for each webhook event. All retransmits of the same event
    /// will use the same ID.
    /// </summary>
    public Guid ID { get; private set; }

    /// <summary>
    /// Will be true if the webhook was generated in a sandbox environment.
    /// </summary>
    public bool IsSandbox { get; private set; }

    /// <summary>
    /// Timestamp the event occurred at.
    /// </summary>
    public DateTimeOffset EventDate { get; set; }

    /// <summary>
    /// The type of event the webhook is for, for example created, updated,
    /// deleted etc.
    /// </summary>
    public WebhookResourceActionsEnum EventType { get; set; }

    /// <summary>
    /// The type of resource the webhook item represents, for example
    /// payload, transaction etc.
    /// </summary>
    public WebhookResourcesEnum ResourceType { get; set; }

    /// <summary>
    /// The resource details of the item that the webhook is for.
    /// </summary>
    public T Data { get; set; }

    public WebhookEvent(
        DateTimeOffset eventDate,
        WebhookResourceActionsEnum eventType,
        WebhookResourcesEnum resourceType,
        T data,
        bool isSandbox)
    {
        ID = Guid.NewGuid();
        IsSandbox = isSandbox;
        EventDate = eventDate;
        EventType = eventType;
        ResourceType = resourceType;
        Data = data;
    }
}

