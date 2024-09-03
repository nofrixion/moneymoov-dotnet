//-----------------------------------------------------------------------------
// Filename: PaymentRailEnum.cs
// 
// Description: Enum for the dfferent rails that can be used to make a payment.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 03 Sep 2024  Aaron Clauson   Created, Carne, Wexford, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum PaymentRailEnum
{
    /// <summary>
    /// The default option where the system will determine the best payment rail to use.
    /// </summary>
    Default,

    /// <summary>
    /// Single Euro Payments Area (SEPA) Credit Transfer scheme. Payments can take up to 
    /// one business day to clear. Supported by all EU institutions.
    /// </summary>
    SEPA_CT,

    /// <summary>
    /// Single Euro Payments Area (SEPA) Instant scheme. Paymnets are processed 24/7 in
    /// seconds. Not yet supported by all EU institutions.
    /// </summary>
    SEPA_INST
}