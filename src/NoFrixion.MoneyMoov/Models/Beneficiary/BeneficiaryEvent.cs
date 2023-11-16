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

    public BeneficiaryAuthoriseStatusEnum EventStatus { get; set; }

    public BeneficiaryEventTypeEnum EventType { get; set; }

    public Guid? AuthoriserID { get; set; }

    public string AuthoriserHash { get; set; }

    public string ErrorReason { get; set; }

    public string ErrorMessage { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public User Authoriser { get; set; }
}
