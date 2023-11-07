//-----------------------------------------------------------------------------
// Filename: PaymentSchedule.cs
// 
// Description: Helper and extension methods for dealing with payment schedules.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 07 Nov 2023  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public static class PaymentSchedule
{
    public const string NO_SCHEDULED_DATE_DESCRIPTION = "Immediately";

    /// <summary>
    /// Gets a formatted string for the scheduled payment timestamp.
    /// </summary>
    public static string GetFormattedSchedule(DateTimeOffset? scheduleDate) =>
        scheduleDate == null ? NO_SCHEDULED_DATE_DESCRIPTION : scheduleDate.Value.ToString("dd MMM yyyy HH:mm:ss K");

    /// <summary>
    /// Gets a formatted string for a scheduled payment day only, e.g. Nov 7th, this
    /// is taking advnatage of the fact that payments can only be scheduled a relatively short
    /// period into the future (60 days at the time of writing).
    /// </summary>
    public static string GetFormattedScheduleDayOnly(DateTimeOffset? scheduleDate) =>
        scheduleDate == null ? NO_SCHEDULED_DATE_DESCRIPTION : GetScheduleDayOnlyString(scheduleDate.Value);

    public static string GetScheduleDayOnlyString(DateTimeOffset scheduleDate) =>
        $"{scheduleDate.ToString("MMM")} {GetDateSuffix(scheduleDate.Day)}";

    public static string GetDateSuffix(int num)
    {
        switch (num % 100)
        {
            case 11:
            case 12:
            case 13:
                return num.ToString() + "th";
        }

        switch (num % 10)
        {
            case 1:
                return num.ToString() + "st";
            case 2:
                return num.ToString() + "nd";
            case 3:
                return num.ToString() + "rd";
            default:
                return num.ToString() + "th";
        }
    }
}

