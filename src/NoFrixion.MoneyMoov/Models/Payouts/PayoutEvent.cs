//  -----------------------------------------------------------------------------
//   Filename: PayoutEvent.cs
// 
//   Description: Payout event:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   16 11 2023  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PayoutEvent
{
    /// <summary>
    /// The user id of the user who the event is for.
    /// </summary>
    public Guid UserID { get; set; }

    /// <summary>
    /// The user name of the user who the event is for.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Event timestamp.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// Status of the payout.
    /// </summary>
    public PayoutStatus Status { get; set; }
    
    /// <summary>
    /// The event type.
    /// </summary>
    public PayoutEventTypesEnum EventType { get; set; }

    /// <summary>
    /// The name of the rule that triggered the event.
    /// </summary>
    public string? RuleName { get; set; }

    /// <summary>
    /// Reason for the error, if any.
    /// </summary>
    public string? ErrorReason { get; set; }
    
    /// <summary>
    /// If this event is a payee verification event, this contains the result of the verification.
    /// </summary>
    public string? SupplierPayeeVerificationResult { get; set; }
    
    /// <summary>
    /// If this event is a payee verification complete event and the result is a close match,
    /// this contains the actual verified name returned by the payee verification supplier.
    /// </summary>
    public string? PayeeVerifiedAccountName { get; set; }
}