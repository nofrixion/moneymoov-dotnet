// -----------------------------------------------------------------------------
//  Filename: PayoutStatus.cs
// 
//  Description: A (batch) payout's status:
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  21 Feb 2022  Arif Matin       Created, Carmichael House, Dublin, Ireland.
//  05 Apr 2022  Arif Matin       Added new status.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov;

/// <summary>
/// The status of payout.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PayoutStatus
{
    UNKNOWN = 0,

    /// <summary>
    /// Enum REJECTED for value: REJECTED
    /// </summary>
    [EnumMember(Value = "REJECTED")]
    REJECTED = 1,

    /// <summary>
    /// Enum PENDING for value: PENDING
    /// </summary>
    [EnumMember(Value = "PENDING")]
    PENDING = 2,

    /// <summary>
    /// Enum PROCESSED for value: PROCESSED
    /// </summary>
    [EnumMember(Value = "PROCESSED")]
    PROCESSED = 3,

    /// <summary>
    /// Enum PENDING_APPROVAL for value: PENDING_APPROVAL
    /// </summary>
    [EnumMember(Value = "PENDING_APPROVAL")]
    PENDING_APPROVAL = 4,

    /// <summary>
    /// Means the payout has been sent to an upstream supplier but no
    /// immediate response was received. The assumption is that the 
    /// supplier has queued it for processing and an update will
    /// be subsequently received, or pulled.
    /// </summary>
    [EnumMember(Value = "QUEUED_UPSTREAM")]
    QUEUED_UPSTREAM = 5,

    /// <summary>
    /// Means the payout has been queued locally prior to being sent to
    /// an upstream supplier.
    /// </summary>
    [EnumMember(Value = "QUEUED")]
    QUEUED = 6,

    /// <summary>
    /// Means when the payout is approved and the IsSubmitted flag is set to true,
    /// but the send payment fails then the payout status is set to FAILURE.
    /// This will prevent any further user actions. If the user does want to 
    /// resubmit the payment they should clone it and try again.
    /// </summary>
    [EnumMember(Value = "FAILED")]
    FAILED = 7,

    /// <summary>
    /// The payout is missing some crucial input fields and cannot be submitted
    /// until they are provided.
    /// </summary>
    [EnumMember(Value = "PENDING_INPUT")]
    PENDING_INPUT = 8,
    
    /// <summary>
    /// Means the payout has been scheduled for processing at a future date.
    /// </summary>
    [EnumMember(Value = "SCHEDULED")]
    SCHEDULED = 9,

    /// <summary>
    /// Enum REJECTED_APPROVAL for value: REJECTED_APPROVAL
    /// </summary>
    [EnumMember(Value = "REJECTED_APPROVAL")]
    REJECTED_APPROVAL = 10,
}
