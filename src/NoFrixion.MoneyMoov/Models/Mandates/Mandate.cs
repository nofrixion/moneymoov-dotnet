﻿// -----------------------------------------------------------------------------
//  Filename: Mandate.cs
// 
//  Description: Represents a Direct Debit mandate entity that contains customer,
//  bank account and mandate information.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  22 03 2024  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Mandates;

/// <summary>
/// Represents a Direct Debit mandate entity that contains customer,
///  bank account and mandate information.
/// </summary>
public class Mandate
{
    /// <summary>
    /// Internal ID of the mandate.
    /// </summary>
    public Guid ID { get; set; }
    
    /// <summary>
    /// Internal ID of this mandate's merchant.
    /// </summary>
    public Guid MerchantID { get; set; }
    
    /// <summary>
    /// Name of the supplier used to create this mandate.
    /// </summary>
    public PaymentProcessorsEnum? SupplierName { get; set; }
    
    /// <summary>
    /// ID that the supplier assigned to this mandate.
    /// </summary>
    public string? SupplierMandateID { get; set; }
    
    /// <summary>
    /// ID that the supplier assigned to this mandate's
    /// bank account.
    /// </summary>
    public string? SupplierBankAccountID { get; set; }
    
    /// <summary>
    /// ID that the supplier assigned to this mandate's
    /// customer.
    /// </summary>
    public string? SupplierCustomerID { get; set; }
    
    /// <summary>
    /// Customer's first name.
    /// </summary>
    public string? CustomerFirstName { get; set; }
    
    /// <summary>
    /// Customer's last name.
    /// </summary>
    public string? CustomerLastName { get; set; }
    
    /// <summary>
    /// Customer's country of residence code.
    /// </summary>
    public string? CustomerCountryCode { get; set; }
    
    /// <summary>
    /// Customer's country of residence.
    /// </summary>
    public string? CustomerCountryName { get; set; }
    
    /// <summary>
    /// Customer's city of residence.
    /// </summary>
    public string? CustomerCity { get; set; }
    
    /// <summary>
    /// Customer's email address.
    /// </summary>
    public string? CustomerEmailAddress { get; set; }
    
    /// <summary>
    /// Customer's IBAN in case of EUR account.
    /// </summary>
    public string? CustomerIban { get; set; }
    
    /// <summary>
    /// Customer's account number in case of GBP account.
    /// </summary>
    public string? CustomerAccountNumber { get; set; }
    
    /// <summary>
    /// Customer's sort code in case of GBP account.
    /// </summary>
    public string? CustomerSortCode { get; set; }
    
    /// <summary>
    /// Reference assigned to this mandate.
    /// </summary>
    public string? Reference { get; set; }
    
    /// <summary>
    /// Whether this mandate is single-use or recurring.
    /// </summary>
    public bool IsRecurring { get; set; }
    
    /// <summary>
    /// Currency of this mandate.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// This is an optional field that with mandates created via Account Information Services can be
    /// used to do a balance check on the payer's account. We don't currenlty support the AIS workflow.
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Date at which the supplier approved this mandate.
    /// </summary>
    public DateTimeOffset? ApprovedAt { get; set; }
    
    /// <summary>
    /// Last status that the supplier reported for this mandate.
    /// </summary>
    public string? SupplierStatus { get; set; }
    
    /// <summary>
    /// General status of this mandate.
    /// </summary>
    public MerchantMandateStatusEnum Status { get; set; }
    
    /// <summary>
    /// The timestamp this mandate was created at.
    /// </summary>
    public DateTimeOffset Inserted { get; set; }
    
    /// <summary>
    /// The timestamp this mandate was last updated at.
    /// </summary>
    public DateTimeOffset LastUpdated { get; set; }
}