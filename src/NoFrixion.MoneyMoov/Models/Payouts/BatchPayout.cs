//-----------------------------------------------------------------------------
// Filename: BatchPayout.cs
//
// Description: Represents a group of payouts that can be approved and
// submitted together.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 19 Apr 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT
//-----------------------------------------------------------------------------


namespace NoFrixion.MoneyMoov.Models;

public class BatchPayout
{
    public Guid ID { get; set; }

    /// <summary>
    /// This field is used when returning a batch payout record to a client. If set it holds the URL
    /// the user needs to visit in order to complete a strong authentication check in order to approve 
    /// the batch payouts.
    /// </summary>
    public string? ApproveUrl { get; set; }

    public List<Payout> Payouts { get; set; } = new List<Payout>();
}
