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

    public List<Payout> Payouts { get; set; } = new List<Payout>();

    /// <summary>
    /// Gets a hash of the critical fields for a batch payout. This hash is
    /// used to ensure the batch payout's details are not modified between the time the
    /// approval is given and the time the payout is actioned.
    /// </summary>
    public string GetApprovalHash()
    {
        string input = string.Empty;

        foreach (var payout in Payouts)
        {
            input += payout.GetApprovalHash();

        }

        return HashHelper.CreateHash(input);
    }
}
