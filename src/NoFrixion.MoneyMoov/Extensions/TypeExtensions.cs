//-----------------------------------------------------------------------------
// Filename: TypeExtensions.cs
// 
// Description: Extension methods.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 03 Nov 2021 Aaron Clauson   Created, Carmichael House, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace MoneyMoov;

public static class TypeExtensions
{
    /// <summary>
    /// Returns the name of any method that calls it.
    /// </summary>
    public static string? GetCaller([CallerMemberName] string? caller = null)
    {
        return caller;
    }
}
