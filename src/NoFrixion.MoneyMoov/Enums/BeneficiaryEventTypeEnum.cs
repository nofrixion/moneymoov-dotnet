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
    
    /// <summary>
    /// A beneficiary was enabled.
    /// </summary>
    Enable = 5,

    /// <summary>
    /// An archive is a soft delete. It removes the beneficiary from the active list.
    /// </summary>
    Archive = 6,
    
    /// <summary>
    /// Indicates that payee verification has been initiated for the beneficiary.
    /// </summary>
    PayeeVerificationInitiated = 7,
    
    /// <summary>
    /// Indicates that payee verification has been completed successfully for the beneficiary.
    /// </summary>
    PayeeVerificationComplete = 8,
    
    /// <summary>
    /// Indicates that payee verification has failed for the beneficiary.
    /// </summary>
    PayeeVerificationFailed = 9
}