// -----------------------------------------------------------------------------
// Filename: PayoutEventTypesEnum.cs
// 
// Description: Enum for the different types of payout events that can
// occur.
//
// Author(s):
// Arif Matin (arif@nofrixion.com)
// 
// History:
// 29 Aep 2023  Arif Matin  Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum PayoutEventTypesEnum
{
    /// <summary>
    /// Something went wrong and the event type is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// A payout was authorised by an approver.
    /// </summary>
    Authorise = 1,

    /// <summary>
    /// A payout was initiated with the supplier. 
    /// </summary>
    Initiate = 2,

    /// <summary>
    /// A payout was initiated. A webhook is sent out when the status of the payment changes.
    /// </summary>
    Webhook = 3,

    /// <summary>
    /// A payout was successful and verified through supplier transaction. 
    /// </summary>
    Settle = 4,

    /// <summary>
    /// A payout was initiated but the funds failed to settle.
    /// </summary>
    Failure = 5,
    
    /// <summary>
    /// A payout was created.
    /// </summary>
    Created = 6,
    
    /// <summary>
    /// A payout was queued.
    /// </summary>
    Queued = 7,
    
    /// <summary>
    /// A payout was scheduled.
    /// </summary>
    Scheduled = 8,
    
    /// <summary>
    /// A payout's associated beneficiary was updated.
    /// </summary>
    BeneficiaryUpdated = 9,
    
    /// <summary>
    /// A payout's associated beneficiary was disabled.
    /// </summary>
    BeneficiaryDisabled = 10,
    
    /// <summary>
    /// A payout was edited.
    /// </summary>
    Edited = 11,
    
    /// <summary>
    /// A payout was rejected by an approver.
    /// </summary>
    RejectedApproval = 12,

    /// <summary>
    /// The payout has been authorised due to a submission from a trusted source.
    /// </summary>
    TrustedAuthorise = 13,
    
    /// <summary>
    /// The payout has been signed.
    /// </summary>
    Signed = 14,
        
    /// <summary>
    /// A payout's associated beneficiary was enabled.
    /// </summary>
    BeneficiaryEnabled = 15,

    /// <summary>
    /// An error that occurred during an attempt to submit a payout for processing. This is not a terminal error
    /// and the attempt can be tried again. Errors after the payout's IsSubmitted flag is set are terminal.
    /// </summary>
    SubmitAttemptError = 16
}
