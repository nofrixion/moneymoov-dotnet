// -----------------------------------------------------------------------------
//  Filename: Payout.cs
// 
//  Description: Contains details of a payout request with additional fields for the portal:
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  22 Nov 2021  Arif Matin     Created, Carmichael House, Dublin, Ireland.
//  12 Dec 2022  Aaron Clauson  Renamed from PortalPayout to Payout.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class Payout : IPayout
{
    /// <summary>
    /// The ID for the payout.
    /// </summary>
    public Guid ID { get; set; }

    /// <summary>
    /// Gets or Sets Account Id of sending account
    /// </summary>
    public Guid AccountID { get; set; }

    /// <summary>
    /// Gets or Sets User ID of who created the payout request
    /// </summary>
    public Guid UserID { get; set; }

    /// <summary>
    /// Gets or Sets payout type
    /// </summary>
    public AccountIdentifierType? Type { get; set; }

    /// <summary>
    /// Gets or Sets description of payout request
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or Sets Currency of payout request
    /// </summary>
    public string? Currency { get; set; }

    /// <summary>
    /// Gets or Sets payout amount
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// Gets or Sets your reference ID
    /// </summary>
    public string? YourReference { get; set; }

    /// <summary>
    /// See <see cref="IPayout.DestinationAccount"/>
    /// </summary>
    public Counterparty? DestinationAccount { get; set; }

    /// <summary>
    /// Gets or Sets destination reference ID
    /// </summary>
    public string? TheirReference { get; set; }

    /// <summary>
    /// Gets or Sets the status of payout request
    /// </summary>
    public PayoutStatus Status { get; set; }

    /// <summary>
    /// The ID of the user that requested access to the PayOut record. Note
    /// this is NOT necessarily the user that created it. 
    /// </summary>
    public Guid? CurrentUserID { get; set; }

    /// <summary>
    /// The role of the user that requested access to the PayOut record. Note
    /// this is NOT necessarily the user that created it. For example one user
    /// may create the payout and then a different user will load the record to
    /// approve it.
    /// </summary>
    public UserRolesEnum? CurrentUserRole { get; set; }

    /// <summary>
    /// This field is used when returning an payout record to a client. If set it holds the URL
    /// the user needs to visit in order to complete a strong authentication check in order to approve 
    /// the payout.
    /// </summary>
    public string? ApprovePayoutUrl { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset Inserted { get; set; }

    /// <summary>
    /// The name of the account the payout is being made from.
    /// </summary>
    public string SourceAccountName { get; set; } = string.Empty;
}
