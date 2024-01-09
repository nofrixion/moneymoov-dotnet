// -----------------------------------------------------------------------------
//  Filename: BeneficiaryEvent.cs
// 
//  Description: Represents an event for a beneficiary.
// 
//  Author(s):
//  Pablo Maldonado (pablo@nofrixion.com)
// 
//  History:
//  16 Nov 2023  Pablo Maldonado   Created, Punta Colorada, Maldonado, Uruguay.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

#nullable disable
public class BeneficiaryEvent
{
    public Guid ID { get; set; }

    public Guid BeneficiaryID { get; set; }

    public string EventStatus { get; set; }

    public BeneficiaryEventTypeEnum EventType { get; set; }

    public Guid? UserID { get; set; }

    public string AuthoriserHash { get; set; }

    public string ErrorReason { get; set; }

    public string ErrorMessage { get; set; }
    
    public string BeneficiaryName { get; set; }
    
    public CurrencyTypeEnum? Currency { get; set; }
    
    public Guid? AccountID { get; set; }
    
    public string AccountName { get; set; }
    
    public string IBAN { get; set; }
    
    public string AccountNumber { get; set; }
    
    public string SortCode { get; set; }
    
    public string BitcoinAddress { get; set; }

    public DateTimeOffset Inserted { get; set; }

    /// <summary>
    /// A hash of the source account ID's that are authorised to use the beneficiary.
    /// An empty value means the beneficairy can be used by all the merchant's source
    /// accounts.
    /// </summary>
    public string SourceAccountsHash { get; set; }

    public User User { get; set; }
}
