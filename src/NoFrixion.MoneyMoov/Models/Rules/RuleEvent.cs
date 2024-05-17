//-----------------------------------------------------------------------------
// Filename: RuleEvent.cs
// 
// Description: A model that represents an event produced by a MoneyMoov
// Rule execution.
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

public class RuleEvent
{
    public Guid ID { get; set; }

    public Guid RuleID { get; set; }

    public RuleEventTypesEnum RuleEventType { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public string? Message { get; set; }

    public string? ErrorMessage { get; set; }

    public string? RawResponse { get; set; }
    
    public bool IsAuthoriseToEnable { get; set; }
    
    public User? User { get; set; }
}