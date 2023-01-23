﻿//-----------------------------------------------------------------------------
// Filename: SplitPercentageAction.cs
// 
// Description: A Rule Action that sweeps funds from an account to multiple
// different accounts based on a percentage split.
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

public class SplitPercentageAction : RuleAction
{
    public Counterparty FirstDestination { get; set; } = new Counterparty();
    public decimal FirstPercentage { get; set; }
    public Counterparty SecondDestination { get; set; } = new Counterparty();
    public decimal SecondPercentage { get; set; }  
    public decimal AmountToLeave { get; set; }
}