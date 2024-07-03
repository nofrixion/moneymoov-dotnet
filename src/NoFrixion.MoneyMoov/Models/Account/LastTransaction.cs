// -----------------------------------------------------------------------------
//  Filename: LastTransaction.cs
// 
//  Description: Represents the last transaction that occurred on an account.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  01 07 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

#nullable disable
namespace NoFrixion.MoneyMoov.Models;

public class LastTransaction
{
    public decimal Amount { get; set; }
    
    public DateTimeOffset Date { get; set; }
}