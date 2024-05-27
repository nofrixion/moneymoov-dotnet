//-----------------------------------------------------------------------------
// Filename: RuleEventTypesEnum.cs
// 
// Description: A list of the event types a MoneyMoov rule execution can produce.
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
public enum RuleEventTypesEnum
{
    None,
    
    /// <summary>
    /// The rule was created.
    /// </summary>
    Created,
    
    /// <summary>
    /// The rule status changed to approved.
    /// </summary>
    Approved,
    
    /// <summary>
    /// The rule was edited.
    /// </summary>
    Edited,
    
    /// <summary>
    /// The rule was disabled.
    /// </summary>
    Disabled,

    /// <summary>
    /// The rule was executed successfully.
    /// </summary>
    ExecutionSuccess,

    /// <summary>
    /// The rule was executed and returned an error.
    /// </summary>
    ExecutionError,

    
}
