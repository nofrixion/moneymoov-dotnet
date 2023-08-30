//-----------------------------------------------------------------------------
// Filename: StringExtensions.cs
//
// Description: Extension methods for the type string
//
// Author(s):
// Aaron Clauson
// 
// History:
// 08 Apr 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NoFrixion.MoneyMoov.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Builds a HTTP URL from a base URL and query string parameters.
    /// </summary>
    /// <param name="baseUrl">The base URL without any query parameters.</param>
    /// <param name=" queryParams">The query parameters to add to the base URL.</param>
    /// <returns>A HTTP URL.</returns>
    public static string ToUrl(this string baseUrl, NameValueCollection queryParams)
    {
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add(queryParams);

        return baseUrl.Contains("?") ?
            $"{baseUrl}&{queryString}" :
            $"{baseUrl}?{queryString}";
    }

    /// <summary>
    /// Checks if string value is normal without special chars.
    /// </summary>
    /// <param name="str">string to be checked</param>
    /// <returns>true or false on the check</returns>
    public static bool IsNormalString(this String str)
    {
        if (str == null)
            // If string is null return true. This is because 
            // we are only looking for special characters in this method.
            return true;

        Regex stringRegex = new Regex(@"^[a-zA-Z0-9_\s-]*$");

        return stringRegex.IsMatch(str);
    }

    /// <summary>
    /// Copied from https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format.
    /// </summary>
    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    /// <summary>
    /// Attempts to parse a GUID from a string.
    /// </summary>
    /// <param name="s"></param>
    /// <returns>A GUID if successful or Guid.Empty if not.</returns>
    public static Guid ToGuid(this string? s)
    {
        if (!string.IsNullOrEmpty(s) &&
            Guid.TryParse(s, out var guid))
        {
            return guid;
        }

        return Guid.Empty;
    }

    /// <summary>
    /// Returns the name of any method that calls it.
    /// </summary>
    public static string? GetCaller([CallerMemberName] string? caller = null)
    {
        return caller;
    }

    public static bool TryFromBase64String(this string base64String, out string result)
    {
        if(string.IsNullOrEmpty(base64String))
        {
            result = string.Empty;
            return false;
        }

        // Convert URL safe base64 strings to standard base64 strings.
        string stdBase64String = base64String.Replace('-', '+').Replace('_', '/');
        while (stdBase64String.Length % 4 != 0)
        {
            stdBase64String += '=';
        }

        Span<byte> buffer = new Span<byte>(new byte[stdBase64String.Length * 4 / 3]);
        if (Convert.TryFromBase64String(stdBase64String, buffer, out int bytesWritten))
        {
            result = Encoding.UTF8.GetString(buffer.Slice(0, bytesWritten));
            return true;
        }
        else
        {
            result = string.Empty;
            return false;
        }
    }
}
