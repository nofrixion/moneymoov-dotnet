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
    /// The payload holding the details of the item that the webhook is for.
    /// </summary>
    public List<WebhookEventItem<T>> Items { get; set; }

    public WebhookEvent(
        bool isSandbox,
        List<WebhookEventItem<T>> items)
    {
        ID = Guid.NewGuid();
        IsSandbox = isSandbox;
        Items = items;
    }
}

