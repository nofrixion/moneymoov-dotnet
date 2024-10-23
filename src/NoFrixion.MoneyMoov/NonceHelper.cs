//-----------------------------------------------------------------------------
// Filename: NonceHelper.cs
// 
// Description: Helper functions for dealing with nonces.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 23 Oct 2024  Aaron Clauson   Created, Carne, Wexford, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using System.Security.Cryptography;

namespace NoFrixion.MoneyMoov;

public static class NonceHelper
{
    public static string GenerateRandomNonce()
    {
        var nonce = new byte[64];
        var random = RandomNumberGenerator.Create();
        random.GetBytes(nonce);

        return Convert.ToBase64String(nonce);
    }
}