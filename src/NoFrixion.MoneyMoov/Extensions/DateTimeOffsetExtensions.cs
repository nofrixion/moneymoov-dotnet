// -----------------------------------------------------------------------------
//  Filename: DateTimeOffsetExtensions.cs
// 
//  Description: Extensions for DateTimeOffset
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  18 Jul 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------


namespace NoFrixion.MoneyMoov.Extensions;

public static class DateTimeOffsetExtensions
{
    public static DateTime ToTimeZoneSpecificTime(this DateTimeOffset dateTimeOffset, TimeZoneInfo timeZoneInfo)
    {
        return TimeZoneInfo.ConvertTime(
            dateTimeOffset, timeZoneInfo).DateTime;
    }
}