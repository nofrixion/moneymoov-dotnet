//-----------------------------------------------------------------------------
// Filename: TribeLoad.cs
// 
// Description: Represents a load into a tribe account. 
// 
// Author(s):
// Saurav Maiti (saurav@nofrixion.com)
// 
// History:
// 15 Oct 2024  Saurav Maiti   Created, Harcourt street, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class TribeLoad: IWebhookPayload
{
    /// <summary>
    /// Represents the transaction that was attempted to
    /// be loaded into the tribe account.
    /// </summary>
    public required Transaction Transaction { get; set; }
    
    /// <summary>
    /// Represents the problem that occurred when attempting to
    /// load the transaction into the tribe account.
    /// </summary>
    public string? FailureReason { get; set; }

    public Guid ID { get; set; }
    
    /// <summary>
    /// The ID of the merchant that the tribe account belongs to.
    /// </summary>
    public Guid MerchantID { get; set; }
    
    public DateTimeOffset Inserted { get; set; }
}