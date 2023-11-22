// -----------------------------------------------------------------------------
//  Filename: BeneficiaryEventTypeEnum.cs
// 
//  Description: Enum for the different types of beneficiary events that can
//  occur.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  02 11 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum BeneficiaryEventTypeEnum
{
    /// <summary>
    /// Something went wrong and the event type is unknown.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// A beneficiary was created.
    /// </summary>
    Create = 1,
    
    /// <summary>
    /// A beneficiary was authorised by an approver.
    /// </summary>
    Authorise = 2,
    
    /// <summary>
    /// A beneficiary was updated.
    /// </summary>
    Update = 3,
    
    /// <summary>
    /// A beneficiary was disabled.
    /// </summary>
    Disable = 4,
}