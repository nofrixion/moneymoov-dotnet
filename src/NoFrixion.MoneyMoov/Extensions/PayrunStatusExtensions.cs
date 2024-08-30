// -----------------------------------------------------------------------------
//  Filename: PayrunStatusExtensions.cs
// 
//  Description: Extensions for PayrunStatus enum
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  25 08 2024  Saurav Maiti   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Extensions;

public static class PayrunStatusExtensions
{
    public static bool CanEdit(this PayrunStatus status) =>
        status is PayrunStatus.Draft or PayrunStatus.AuthorisationPending or PayrunStatus.Rejected;
    
    public static bool CanDelete(this PayrunStatus status) =>
        status is PayrunStatus.Draft or PayrunStatus.Rejected or PayrunStatus.AuthorisationPending;
}