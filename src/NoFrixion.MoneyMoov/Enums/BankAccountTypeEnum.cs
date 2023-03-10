// -----------------------------------------------------------------------------
//  Filename: BankAccountTypeEnum.cs
// 
//  Description: List of bank account types.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  08 03 2023  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov;

public enum BankAccountTypeEnum
{
    [EnumMember(Value = "None")]
    None = 0,

    /// <summary>
    /// For personal bank accounts.
    /// </summary>
    [EnumMember(Value = "Personal")]
    Personal = 1,

    /// <summary>
    /// For business bank accounts.
    /// </summary>
    [EnumMember(Value = "Business")]
    Business = 2
}