//-----------------------------------------------------------------------------
// Filename: RuleAction.cs
// 
// Description: A Rule Action is an executable steps in a MoneyMoov Rule.
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

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class RuleAction
{
    public int Priority { get; set; }

    public RuleActionsEnum ActionType { get; set; }

    [Required]
    public string ActionContentJson { get; set; } = string.Empty;
}