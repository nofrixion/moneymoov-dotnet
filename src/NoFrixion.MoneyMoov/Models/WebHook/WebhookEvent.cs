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

public class WebhookEvent
{
    /// <summary>
    /// A unique ID for each webhook event. All retransmits of the same event
    /// will use the same ID.
    /// </summary>
    public Guid ID { get; private set; }
    
    /// <summary>
    /// ID of the merchant that the webhook is for.
    /// </summary>
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Will be true if the webhook was generated in a sandbox environment.
    /// </summary>
    public bool IsSandbox { get; private set; }

    /// <summary>
    /// The payload holding the details of the item that the webhook is for.
    /// </summary>
    public List<WebhookEventItem> Items { get; set; }

    public WebhookEvent(
        bool isSandbox,
        List<WebhookEventItem> items,
        Guid merchantID)
    {
        ID = Guid.NewGuid();
        IsSandbox = isSandbox;
        Items = items;
        MerchantID = merchantID;
    }
}

