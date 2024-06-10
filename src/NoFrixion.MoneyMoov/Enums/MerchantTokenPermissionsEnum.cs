// -----------------------------------------------------------------------------
//  Filename: MerchantTokenPermissionsEnum.cs
// 
//  Description: Enum for permissions associated with merchant tokens.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  14 02 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
//  09 01 2024  Aaron Clauson   Added report permissions.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

[Flags]
public enum MerchantTokenPermissionsEnum
{
    /// <summary>
    /// Permission to deny all actions. This won't be assigned in practice but when
    /// checking whether a permission is granted, this can be used to specify that merchant
    /// tokens are not allowed to perform any actions.
    /// </summary>
    Deny = 0,

    /// <summary>
    /// Permission to create a payment request
    /// </summary>
    CreatePaymentRequest = 1,

    /// <summary>
    /// Permission to edit a payment request
    /// </summary>
    EditPaymentRequest = 2,

    /// <summary>
    /// Permission to delete a payment request
    /// </summary>
    DeletePaymentRequest = 4,

    /// <summary>
    /// Permission to create a rule
    /// </summary>
    CreateRule = 8,

    /// <summary>
    /// Permission to edit a rule
    /// </summary>
    EditRule = 16,

    /// <summary>
    /// Permission to delete a rule
    /// </summary>
    DeleteRule = 32,

    /// <summary>
    /// Permission to create a payout
    /// </summary>
    CreatePayout = 64,

    /// <summary>
    /// Permission to edit a payout
    /// </summary>
    EditPayout = 128,

    /// <summary>
    /// Permission to delete a payout
    /// </summary>
    DeletePayout = 256,

    /// <summary>
    /// Permission to create a report.
    /// </summary>
    CreateReport = 512,

    /// <summary>
    /// Permission to edit, retrieve and initiate a report.
    /// </summary>
    EditReport = 1024,

    /// <summary>
    /// Permission to delete a report.
    /// </summary>
    DeleteReport = 2048,

    /// <summary>
    /// Permission to execute and get report results.
    /// </summary>
    ExecuteReport = 4096,

    /// <summary>
    /// Permission to create a new payment account.
    /// </summary>
    CreatePaymentAccount = 8192,

    /// <summary>
    /// Permission to edit details on a payment account.
    /// </summary>
    EditPaymentAccount = 16384,

    /// <summary>
    /// Permission to create and submit a payout from a trusted source.
    /// </summary>
    TrustedSubmitPayout = 32768,
}