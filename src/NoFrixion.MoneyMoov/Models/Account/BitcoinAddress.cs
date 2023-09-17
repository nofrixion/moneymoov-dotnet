//-----------------------------------------------------------------------------
// Filename: BitcoinAddress.cs
// 
// Description: Represents an electronic money payment account. Typically
//  denominated in EUR or GBP.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 17 Sep 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class BitcoinAddress
{
    /// <summary>
    /// Unique id for the address.
    /// </summary>
    public Guid ID { get; set; }

    /// <summary>
    /// The ID of the account that address belongs to.
    /// </summary>
    public Guid AccountID { get; set; }

    public string Address { get; set; }

    /// <summary>
    /// Timestamp when the address was created.
    /// </summary>
    public DateTimeOffset Inserted { get; set; }
}