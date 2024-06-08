//-----------------------------------------------------------------------------
// Filename: RulesStatusEnum.cs
// 
// Description: A list of the statuses a MoneyMoov rule can be in.
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

using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleStatusEnum
{
    PendingApproval,

    Active,

    Disabled,

    Failed
}
