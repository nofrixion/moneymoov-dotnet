//-----------------------------------------------------------------------------
// Filename: SweepAction.cs
// 
// Description: A Rule Action that sweeps all the available funds in an account
// to a different account.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 22 Jan 2023  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class SweepAction : RuleAction
{
    public Counterparty Destination { get; set; } = new Counterparty();

    public decimal AmountToLeave { get; set; }
}