// -----------------------------------------------------------------------------
//  Filename: VirtualAccountCreate.cs
// 
//  Description: VirtualAccountCreate model:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  02 05 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class VirtualAccountCreate
{
    /// <summary>
    /// The name of the virtual account.
    /// </summary>
    [Required(ErrorMessage = "Virtual account name is required.", AllowEmptyStrings=false)]
    public required string Name { get; set; }
}