// -----------------------------------------------------------------------------
//  Filename: PaymentsConstants.cs
// 
//  Description: Contains constants for payments related values:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 01 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public static class PaymentsConstants
{
    /// <summary>
    /// Round fiat (EUR, GBP etc) amounts to this many decimal places.
    /// </summary>
    public const int FIAT_ROUNDING_DECIMAL_PLACES = 2;

    /// <summary>
    /// Bitcoin Lightning uses millisats (thousands of a Bitcoin sat).
    /// </summary>
    public const int BITCOIN_LIGHTNING_ROUNDING_DECIMAL_PLACES = 11;

    /// <summary>
    /// The minimum amount that can be set for a Euro payment request that includes the
    /// Payment Initiation (PISP) method.
    /// </summary>
    public const decimal PISP_MINIMUM_EUR_PAYMENT_AMOUNT = 1M;

    /// <summary>
    /// The minimum amount that can be set for a Sterling payment request that includes the
    /// Payment Initiation (PISP) method.
    /// </summary>
    public const decimal PISP_MINIMUM_GBP_PAYMENT_AMOUNT = 2M;

    /// <summary>
    /// The minimum amount that can be set for a payment request that includes the
    /// card payment method. Note that card authorisations can be done for a zero amount,
    /// as a way to get a reusable customer token for future merchant initiated payments.
    /// The invalid range for a card payment is therefore less than 0 or greater than 0
    /// and less than this constant.
    /// </summary>
    public const decimal CARD_MINIMUM_PAYMENT_AMOUNT = 0.01M;

    public const string CARD_SENSISTIVE_TOKEN_SUFFIX = "nofrixion_";

    /// <summary>
    /// Dummy UK accounts in sandbox have a sort code of 6 zeros.
    /// </summary>
    public const string DUMMY_UK_TEST_SORT_CODE = "000000";

    /// <summary>
    /// Dummy sort code used in dev and sandbox.
    /// </summary>
    public const string DUMMY_SORT_CODE = "040002";

    public const string PISP_REFERENCE_PREFIX = "pisp";

    public const int PISP_REFERENCE_MAX_LENGTH = 17;

    public const string PISP_SETTLED_STATUS = "SETTLED";

    public const string PISP_FAILED_STATUS = "FAILED";

    public const string NOFRIXION_OPEN_BANKING_PROVIDER_ID = "nofrixion";
}