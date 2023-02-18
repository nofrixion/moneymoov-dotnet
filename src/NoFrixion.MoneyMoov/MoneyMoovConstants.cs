//-----------------------------------------------------------------------------
// Filename: MoneyMoovConstants.cs
//
// Description: ??.
//
// Author(s):
// ??
// 
// History:
// ??   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public class MoneyMoovConstants
{
    /// <summary>
    /// ??
    /// </summary>
    public const decimal LOW_TRANSACTION_AMOUNT_DEFINITION = 0.5M;

    /// <summary>
    /// ??
    /// </summary>
    public const int HIGH_ROUND_TRANSACTION_AMOUNT_DEFINITION = 1000;

    /// <summary>
    /// For cases where the URI supplied doesn't need to be sent.
    /// </summary>
    public const string WEBHOOK_BLACKHOLE_URI = "http://127.0.0.1";
}
