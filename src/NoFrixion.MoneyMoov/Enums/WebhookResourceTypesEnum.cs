﻿// -----------------------------------------------------------------------------
// Filename: WebhookResourceTypesEnum.cs
// 
// Description: The different types of web hooks that are generated by the
// MoneyMoov API.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 24 Dec 2022  Aaron Clauson    Renamed from WebHook to WebhookResourceTypesEnum.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

[Flags]
public enum WebhookResourceTypesEnum
{
    None = 0,

    /// <summary>
    /// Represents a new transaction that corresponds to funds being received.
    /// This is a duplicate of the TransactionPayin resource and was used in version 1 
    /// webhooks. It's recommended to use TransactionPayin instead.
    /// </summary>
    Payin = 1,

    /// <summary>
    /// Will trigger notifications for payout related events. 
    /// </summary>
    Payout = 2,

    /// <summary>
    /// Will trigger notifications for payment request related events.
    /// </summary>
    PaymentRequest = 4,

    /// <summary>
    /// Will trigger notifications for rule related events.
    /// </summary>
    Rule = 8,

    /// <summary>
    /// Will trigger notifications for a transaction that is receiving funds. 
    /// </summary>
    TransactionPayin = 16,

    /// <summary>
    /// Will trigger notifications for a transaction that is sending funds. 
    /// </summary>
    TransactionPayout = 32,

    /// <summary>
    /// Will trigger notifications for report, including account statement, related events.
    /// </summary>
    Report = 64,
    
    /// <summary>
    /// Will trigger notifications for payrun events
    /// </summary>
    Payrun = 128,
}

