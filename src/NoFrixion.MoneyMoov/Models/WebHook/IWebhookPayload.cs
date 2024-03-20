//-----------------------------------------------------------------------------
// Filename: IWebhook.cs
// 
// Description: Common interface for models that are can be used in a webhook.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 27 May 2023 Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public interface IWebhookPayload
{
    public Guid ID { get; set; }

    public Guid MerchantID { get; set; }

    public DateTimeOffset Inserted { get; set; }
}

