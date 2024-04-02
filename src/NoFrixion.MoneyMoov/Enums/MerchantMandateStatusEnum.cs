// -----------------------------------------------------------------------------
//  Filename: MerchantMandateStatusEnum.cs
// 
//  Description: Enum for the status of a merchant mandate.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  21 03 2024  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum MerchantMandateStatusEnum
{
    /// <summary>
    /// Unknown state
    /// </summary>
    Unknown,
    
    /// <summary>
    /// Mandate was submitted to the Supplier and awaiting approval/review.
    /// </summary>
    Pending,
    
    /// <summary>
    /// Mandate has been approved by the Supplier.
    /// </summary>
    Active,
    
    /// <summary>
    /// Mandate has been expired or cancelled by the supplier.
    /// </summary>
    Expired,
    
    /// <summary>
    /// Supplier could not approve the mandate.
    /// </summary>
    Failed
}