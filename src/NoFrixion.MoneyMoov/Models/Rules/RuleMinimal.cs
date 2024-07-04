// -----------------------------------------------------------------------------
//  Filename: RuleMinimal.cs
// 
//  Description: Minimal representation of a rule.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  04 07 2024  Saurav Maiti   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class RuleMinimal
{
    public Guid ID { get; set; }

    public string Name { get; set; } = string.Empty;
}