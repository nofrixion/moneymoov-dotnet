//-----------------------------------------------------------------------------
// Filename: AmountMinorUnitExtensions.cs
// 
// Description: Contains extension methods for representing amounts in their
// minor unit values. For example fiat currency amounts from Euro and cents to cents.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 24 Apr 2025  Aaron Clauson   Created, Dublin, Ireland.
// 
// License:
// MIT
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public static class AmountMinorUnitExtensions
{
    public static long ToAmountMinorUnits(this decimal amount, CurrencyTypeEnum currency)
    {
        decimal units = currency switch
        {
            var c when c.IsFiat() => amount * (int)Math.Pow(10, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES_EXTERNAL),
            CurrencyTypeEnum.BTC => amount * (int)Math.Pow(10, PaymentsConstants.BITCOIN_ROUNDING_DECIMAL_PLACES),
            _ => throw new ApplicationException($"Currency {currency} was not recognised in {nameof(ToAmountMinorUnits)}.")
        };

        // For safety always disregard any sub-unit amounts.
        units = decimal.Round(units, 0, MidpointRounding.ToZero);

        if (units > long.MaxValue || units < long.MinValue)
        {
            throw new OverflowException($"Amount {amount} is out of range for {nameof(ToAmountMinorUnits)}.");
        }

        return (long)units;
    }
}