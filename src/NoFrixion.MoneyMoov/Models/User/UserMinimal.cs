// -----------------------------------------------------------------------------
//  Filename: UserMinimal.cs
// 
//  Description: Model containing basic user info.
//
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  07 Jun 2024  Saurav Maiti  Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class UserMinimal
{
    public Guid ID { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string EmailAddress { get; set; } = string.Empty;
}