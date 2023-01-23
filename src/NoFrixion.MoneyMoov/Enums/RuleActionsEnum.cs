//-----------------------------------------------------------------------------
// Filename: RulesActionsEnum.cs
// 
// Description: A list of the supported Rule Actions, or executable steps, for
// a MoneyMoov Rule.
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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov;

[JsonConverter(typeof(StringEnumConverter))]
public enum RuleActionsEnum
{
    IsAlive,

    Greeter,

    Payout,

    PayoutPercentage,

    Split,

    Sweep
}
