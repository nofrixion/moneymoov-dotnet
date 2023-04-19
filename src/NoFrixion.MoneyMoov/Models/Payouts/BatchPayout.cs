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
}
