//-----------------------------------------------------------------------------
// Filename: PaymentProcessorsEnum.cs
//
// Description: List of the processors that are supported for processing
// payment requests.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 15 May 2022 Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

/// <summary>
/// Lists the supported card and PIS processors.
/// </summary>
public enum PaymentProcessorsEnum
{
    /// <summary>
    /// For payments that do not use a processor or the processor
    /// is unknown.
    /// </summary>
    None,

    /// <summary>
    /// CyberSource, card processor for Barclays.
    /// </summary>
    CyberSource,

    /// <summary>
    /// checkout.com card processor.
    /// </summary>
    Checkout,

    /// <summary>
    /// Stripe card processor.
    /// </summary>
    Stripe,

    /// <summary>
    /// Modulr payment initiation processor. 
    /// </summary>
    Modulr,

    /// <summary>
    /// Plaid payment initiation processor. 
    /// </summary>
    Plaid,

    /// <summary>
    /// Yapily payment initiation processor. 
    /// </summary>
    Yapily,

    /// <summary>
    /// NoFrixion payment initiation processor.
    /// </summary>
    Nofrixion,

    /// <summary>
    /// Processed on Bitcoin mainnet.
    /// </summary>
    Bitcoin,

    /// <summary>
    /// Processed on Bitcoin testnet.
    /// </summary>
    BitcoinTestnet,

    /// <summary>
    /// Banking Circle standard accounts.
    /// </summary>
    BankingCircle,

    /// <summary>
    /// Banking Circle agency banking accounts.
    /// </summary>
    BankingCircleAgency,

    /// <summary>
    /// Processor to simulate payment initiation payments. 
    /// </summary>
    Simulator
}

