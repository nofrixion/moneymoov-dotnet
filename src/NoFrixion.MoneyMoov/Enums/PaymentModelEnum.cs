// -----------------------------------------------------------------------------
//  Filename: PaymentModelEnum.cs
// 
//  Description: 
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  22 12 2025  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum PaymentModelEnum
{
    None,
    
    /// <summary>
    /// Entity executes the payment on behalf of the merchant.
    /// </summary>
    PoBo,
    
    /// <summary>
    /// Platform acts as an agent, while the underlying merchant or company remains the legal creditor.
    /// </summary>
    Agency
}