// -----------------------------------------------------------------------------
//  Filename: ProofOfPaymentResourceTypeEnum.cs
// 
//  Description: Possible entity types supported for proof of payment generation.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  26 11 2024  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum ProofOfPaymentResourceTypeEnum
{
    None = 0,
    Payout = 1,
    Transaction = 2
}