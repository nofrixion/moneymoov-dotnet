//-----------------------------------------------------------------------------
// Filename: RuleResultsEnum.cs
// 
// Description: A list of the result types a MoneyMoov rule execution can produce.
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
public enum RuleResultsEnum
{
    None,

    Success,

    Error,
}
