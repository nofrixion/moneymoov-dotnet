// -----------------------------------------------------------------------------
//  Filename: BeneficiaryAuthoriseStatusEnum.cs
// 
//  Description: Status of a beneficiary authorisation event.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  02 11 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum BeneficiaryAuthoriseStatusEnum
{
    
    Success,
    Failure,
}