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
    /// This is a special case of Transaction. If both in and out transactions are
    /// desired the Transaction option can be used.
    /// </summary>
    Payin = 1,

    /// <summary>
    /// Will trigger notifications for payout related events. 
    /// </summary>
    /// <remarks>
    /// Note due to unfortunate naming if a merchant is using version 1 webhooks 
    /// this webhook type corresponds to a transaction and NOT a payout where funds 
    /// are being sent out i.e. the opposite of payin.
    /// </remarks>
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
    /// <remarks>
    /// This is the equivalent of the webhooks version 1 Payin.
    /// </remarks>
    TransactionPayin = 16,

    /// <summary>
    /// Will trigger notifications for a transaction that is sending funds. 
    /// </summary>
    /// <remarks>
    /// This is the equivalent of the webhooks version 1 Payout.
    /// </remarks>
    TransactionPayout = 32,

    /// <summary>
    /// Will trigger notifications for report, including account statement, related events.
    /// </summary>
    Report = 64,
    
    /// <summary>
    /// Will trigger notifications for tribe load event
    /// </summary>
    TribeLoad = 128,
}

