// -----------------------------------------------------------------------------
//  Filename: BeneficiaryIdentifier.cs
// 
//  Description: BeneficiaryIdentifier model
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  06 11 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

#nullable disable
namespace NoFrixion.MoneyMoov.Models;

public class BeneficiaryIdentifier
{
    public Guid ID { get; set; }

    public string AccountName { get; set; }
    
    public string AccountNumber { get; set; }

    public string SortCode { get; set; }

    public string BitcoinAddress { get; set; }

    public Guid BeneficiaryID { get; set; }

    public Guid MerchantID { get; set; }

    public string IBAN { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public bool IsEnabled { get; set; }
}