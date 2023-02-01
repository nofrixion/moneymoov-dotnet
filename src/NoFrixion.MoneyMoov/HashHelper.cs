// -----------------------------------------------------------------------------
//  Filename: HashHelper.cs
// 
//  Description: Includes methods to create hashes and verify hashes:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  16 11 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;

namespace NoFrixion.MoneyMoov;

public static class HashHelper
{
    /// <summary>
    /// Creates a 64 byte SHA256 hash of a given string
    /// </summary>
    /// <param name="input">The string data to hash</param>
    /// <returns>The hash</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string CreateHash(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        using var sha256Hash = SHA256.Create();
        byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        var sBuilder = new StringBuilder();

        foreach (var b in data)
        {
            sBuilder.Append(b.ToString("x2"));
        }

        return sBuilder.ToString();
    }

    /// <summary>
    /// Verifies a hash against an input
    /// </summary>
    /// <param name="input">The string to verify</param>
    /// <param name="hash">The hash</param>
    /// <returns>True if the hashes match</returns>
    public static bool VerifyHash(string input, string hash)
    {
        var hashOfInput = CreateHash(input);

        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        return comparer.Compare(hashOfInput, hash) == 0;
    }
}