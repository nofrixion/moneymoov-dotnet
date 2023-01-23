//-----------------------------------------------------------------------------
// Filename: GreeterAction.cs
// 
// Description: A diagnostics Rule Action that returns a greeting message.
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

public class GreeterAction : RuleAction
{
    public const string DEFAULT_GREETING = "Hi, {name}";

    public string Greeting { get; set; } = DEFAULT_GREETING;
}