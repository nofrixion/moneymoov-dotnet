//-----------------------------------------------------------------------------
// Filename: AddressTypesEnum.cs
//
// Description: A list of the different types of payer addresses.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 17 Jan 2022 Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

/// <summary>
/// Lists the supported address types.
/// </summary>
public enum AddressTypesEnum
{
    Unknown = 0,

    /// <summary>
    /// The delivery address for the payer.
    /// </summary>
    Shipping = 1,

    /// <summary>
    /// The billing address for the payer.
    /// </summary>
    Billing = 2
}

