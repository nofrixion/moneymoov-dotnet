//-----------------------------------------------------------------------------
// Filename: PayoutChargeBearerEnum.cs
// 
// Description: Enum for the different types of charge, or fee, mechanisms that
// can be used for a payout. Typically only relevant for international payments.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// Aaron Clauson    09 Apr 2025    Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

/// <summary>
/// Enum for the different types of charge, or fee, mechanisms that can be used 
/// for a payout. Typically only relevant for international payments.
/// </summary>
public enum PayoutChargeBearerEnum
{
    /// <summary>
    /// Was not specified.
    /// </summary>
    Unknown,
    
    /// <summary>
    /// The payout beneficiary is requested to bear the transactions charges.
    /// </summary>
    BEN,
    
    /// <summary>
    /// The payout originator is requested to bear the transaction charges.
    /// </summary>
    OUR,

    /// <summary>
    /// The transaction charges are requested to be shared between the payout
    /// originator and beneficiary. This is the default mechanism used for
    /// international payments.
    /// </summary>
    SHA
}