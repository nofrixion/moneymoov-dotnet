// -----------------------------------------------------------------------------
//  Filename: MerchantAccount.cs
// 
//  Description: MerchantAccount class:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class MerchantAccount
{
    /// <summary>
    /// Status of the account. Accounts must be &#39;ACTIVE&#39; to make and receive payments. Can be one of 
    /// </summary>
    /// <value>Status of the account. Accounts must be &#39;ACTIVE&#39; to make and receive payments. Can be one of </value>
    public AccountStatus Status { get; set; }

    public List<string> AccessGroups { get; set; }

    /// <summary>
    /// The current available balance of the MerchantAccount. Calculated by subtracting any pending payments from the current balance
    /// </summary>
    /// <value>The current available balance of the MerchantAccount. Calculated by subtracting any pending payments from the current balance</value>
    public string AvailableBalance { get; set; }

    /// <summary>
    /// Balance of the account in format &#39;NN.NN&#39;
    /// </summary>
    /// <value>Balance of the account in format &#39;NN.NN&#39;</value>
    public string Balance { get; set; }

    /// <summary>
    /// Datetime when the account was created. Format is &#39;yyyy-MM-dd&#39;T&#39;HH:mm:ssZ&#39; where Z is UTC offset. e.g 2017-01-28T01:01:01+0000
    /// </summary>
    /// <value>Datetime when the account was created. Format is &#39;yyyy-MM-dd&#39;T&#39;HH:mm:ssZ&#39; where Z is UTC offset. e.g 2017-01-28T01:01:01+0000</value>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Currency of the account in ISO 4217 format
    /// </summary>
    /// <value>Currency of the account in ISO 4217 format</value>
    public string Currency { get; set; }

    /// <summary>
    /// Unique id of the Customer
    /// </summary>
    /// <value>Unique id of the Customer</value>
    public string CustomerID { get; set; }

    /// <summary>
    /// Customer Name
    /// </summary>
    /// <value>Customer Name</value>
    public string CustomerName { get; set; }

    /// <summary>
    /// Direct Debit Enabled
    /// </summary>
    /// <value>Direct Debit Enabled</value>
    public bool DirectDebit { get; set; }

    /// <summary>
    /// Your reference for an account
    /// </summary>
    /// <value>Your reference for an account</value>
    public string ExternalReference { get; set; }

    /// <summary>
    /// Unique id for the account
    /// </summary>
    /// <value>Unique id for the account</value>
    public string ID { get; set; }

    /// <summary>
    /// Gets or Sets Identifiers
    /// </summary>
    public IEnumerable<AccountIdentifier> Identifiers { get; set; }

    /// <summary>
    /// Name for the account
    /// </summary>
    /// <value>Name for the account</value>
    public string Name { get; set; }

    /// <summary>
    /// Display name for UI
    /// </summary>
    public string DisplayName => $"{Name} ({ID})";

    public override string ToString()
    {
        return $"{nameof(Status)}: {Status}, {nameof(AccessGroups)}: {AccessGroups}, {nameof(AvailableBalance)}: {AvailableBalance}, {nameof(Balance)}: {Balance}, {nameof(CreatedDate)}: {CreatedDate}, {nameof(Currency)}: {Currency}, {nameof(CustomerID)}: {CustomerID}, {nameof(CustomerName)}: {CustomerName}, {nameof(DirectDebit)}: {DirectDebit}, {nameof(ExternalReference)}: {ExternalReference}, {nameof(ID)}: {ID}, {nameof(Identifiers)}: {Identifiers}, {nameof(Name)}: {Name}";
    }
}