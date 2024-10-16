// -----------------------------------------------------------------------------
//  Filename: MandateCreate.cs
// 
//  Description: Model for creating a Direct Debit mandate.
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

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.Mandates;

public class MandateCreate
{
    /// <summary>
    /// Merchant ID that this mandate is associated with.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, specify a merchant.")]
    public Guid MerchantID { get; set; }
    
    /// <summary>
    /// Customer's first name.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, provide a valid first name.")]
    [MaxLength(256)]
    [RegularExpression("^[a-zA-Z0-9\\-,'. ]+$", ErrorMessage = "Please, provide a valid first name.")]
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Customer's last name.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, provide a valid last name.")]
    [MaxLength(256)]
    [RegularExpression("^[a-zA-Z0-9\\-,'. ]+$", ErrorMessage = "Please, provide a valid last name.")]
    public string? LastName { get; set; }
    
    /// <summary>
    /// First line of the customer's address.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, provide a valid address.")]
    [MaxLength(100)]
    [RegularExpression("^[a-zA-Z0-9\\-,'./ ]+$", ErrorMessage = "Please, provide a valid first address line.")]
    public string? AddressLine1 { get; set; }
    
    /// <summary>
    /// Second line of the customer's address. Optional.
    /// </summary>
    [MaxLength(100)]
    [RegularExpression("^[a-zA-Z0-9\\-,'./ ]+$", ErrorMessage = "Please, provide a valid second address line.")]
    public string? AddressLine2 { get; set; }
    
    /// <summary>
    /// Customer's postal code.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, provide a postal code.")]
    [MaxLength(10)]
    [RegularExpression("^[a-zA-Z0-9\\-,. /]+$", ErrorMessage = "Please, provide a valid postal code.")]
    public string? PostalCode { get; set; }
    
    /// <summary>
    /// Customer's city.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, provide a city name.")]
    [MaxLength(128)]
    [RegularExpression("^[a-zA-Z0-9\\-,'. ]+$", ErrorMessage = "Please, provide a valid city name.")]
    public string? City { get; set; }

    /// <summary>
    /// 2-character country code of the customer's bank account.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, specify your bank account's country.")]
    [MaxLength(2)]
    public string? CountryCode { get; set; }
    
    /// <summary>
    /// IBAN of the customer's bank account in case of EUR account.
    /// </summary>
    [MaxLength(34)]
    public string? Iban { get; set; }

    /// <summary>
    /// Account number of the customer's bank account in case of GBP account.
    /// </summary>
    [MaxLength(8)]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Please, provide a valid account number.")]
    public string? AccountNumber { get; set; }

    /// <summary>
    /// Sort code of the customer's bank account in case of GBP account.
    /// </summary>
    [MaxLength(6)]
    [RegularExpression("^[0-9\\-]*$", ErrorMessage = "Please, provide a valid sort code.")]
    public string? SortCode { get; set; }
    
    /// <summary>
    /// Customer's email address.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, provide a valid email address.")]
    [EmailAddress(ErrorMessage = "Please, provide a valid email address.")]
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Field that you can use as reference.
    /// </summary>
    [MaxLength(128)]
    public string? Reference { get; set; }

    /// <summary>
    /// Indicates whether this mandate is single-use or recurring.
    /// </summary>
    public bool? IsRecurring { get; set; }

    /// <summary>
    /// Currency of the mandate.
    /// </summary>
    [Required(ErrorMessage = "Please, specify a valid currency.")]
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// This is an optional field that with mandates created via Account Information Services can be
    /// used to do a balance check on the payer's account. We don't currenlty support the AIS workflow.
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonProperty]
    public decimal Amount { get; set; }

    /// <summary>
    /// Places all the mandate's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the mandate's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();
        
        dict.Add(nameof(MerchantID), MerchantID.ToString());
        dict.Add(nameof(FirstName), FirstName ?? string.Empty);
        dict.Add(nameof(LastName), LastName ?? string.Empty);
        dict.Add(nameof(AddressLine1), AddressLine1 ?? string.Empty);
        dict.Add(nameof(AddressLine2), AddressLine2 ?? string.Empty);
        dict.Add(nameof(PostalCode), PostalCode ?? string.Empty);
        dict.Add(nameof(City), City ?? string.Empty);
        dict.Add(nameof(CountryCode), CountryCode ?? string.Empty);
        dict.Add(nameof(Iban), Iban ?? string.Empty);
        dict.Add(nameof(AccountNumber), AccountNumber?.ToString() ?? string.Empty);
        dict.Add(nameof(SortCode), SortCode?.ToString() ?? string.Empty);
        dict.Add(nameof(EmailAddress), EmailAddress ?? string.Empty);
        dict.Add(nameof(Reference), Reference ?? string.Empty);
        dict.Add(nameof(IsRecurring), IsRecurring?.ToString() ?? string.Empty);
        dict.Add(nameof(Currency), Currency.ToString());
        dict.Add(nameof(Amount), Amount.ToString());
        
        return dict;
    }
}