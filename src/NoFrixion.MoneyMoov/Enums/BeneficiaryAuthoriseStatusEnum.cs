// -----------------------------------------------------------------------------
//  Filename: BeneficiaryAuthoriseStatusEnum.cs
// 
//  Description: TODO:
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  02 11 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
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