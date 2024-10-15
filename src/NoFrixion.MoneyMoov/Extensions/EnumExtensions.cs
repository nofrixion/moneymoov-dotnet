//-----------------------------------------------------------------------------
// Filename: GuidExtensions.cs
// 
// Description: Contains extension methods for Guid types:
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 15 Oct 2024  Aaron Clauson   Created, Carne, Wexford, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public static class EnumExtensions
{
    /// <summary>
    /// This method converts an Enum with the Flags attribute to a list of Enums.
    /// </summary>
    public static List<T> ToList<T>(this T flags) where T : Enum
    {
        if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
        {
            throw new ArgumentException("The type parameter T must have the Flags attribute.", nameof(flags));
        }

        // Check if the enum underlying type is ulong
        var underlyingType = Enum.GetUnderlyingType(typeof(T));

        if (underlyingType == typeof(ulong))
        {
            return Enum.GetValues(typeof(T))
                   .Cast<T>()
                   .Where(value => flags.HasFlag(value) && Convert.ToUInt64(value) != 0) // Exclude None or 0
                   .ToList();
        }
        else
        {
            return Enum.GetValues(typeof(T))
               .Cast<T>()
               .Where(value => flags.HasFlag(value) && Convert.ToInt32(value) != 0) // Exclude None or 0
               .ToList();
        }
    }

    /// <summary>
    /// This method converts  list of flag enum values to a single flag enum.
    /// </summary>
    public static T ToFlagEnum<T>(this IEnumerable<T> enumValues) where T : Enum
    {
        if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
        {
            throw new ArgumentException("The type parameter T must have the Flags attribute.", nameof(enumValues));
        }

        // Check if the enum underlying type is ulong
        var underlyingType = Enum.GetUnderlyingType(typeof(T));

        if (underlyingType == typeof(ulong))
        {
            ulong result = 0UL;

            foreach (var value in enumValues)
            {
                result |= Convert.ToUInt64(value);
            }

            return (T)Enum.ToObject(typeof(T), result);
        }
        else
        {
            int result = 0;

            foreach (var value in enumValues)
            {
                result |= Convert.ToInt32(value);
            }

            return (T)Enum.ToObject(typeof(T), result);
        }
    }
}