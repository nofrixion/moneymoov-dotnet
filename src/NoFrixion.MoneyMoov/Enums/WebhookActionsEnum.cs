//-----------------------------------------------------------------------------
// Filename: WebhookActionsEnum.cs
// 
// Description: Enum for the different types of actions that can be performed
// on webhook resources.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 26 May 2023 Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov;

[JsonConverter(typeof(StringEnumConverter))]
public enum WebhookActionsEnum
{
    Created,

    Updated,

    Deteted
}
