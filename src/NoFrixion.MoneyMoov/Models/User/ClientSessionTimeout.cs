//-----------------------------------------------------------------------------
// Filename: ClientSessionTimeout.cs
//
// Description: Model to represent a client session timeout for a merchant.
//
// Author(s):
// Saurav Maiti (saurav@nofrixion.com)
// 
// History:
// 18 Mar 2025  Saurav Maiti   Created, Hamilton Gardens, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class ClientSessionTimeout
{
    /// <summary>
    /// The merchant ID for which the session timeout applies.
    /// </summary>
    public Guid MerchantID { get; set; }
    
    /// <summary>
    /// The number of seconds a session for this user should last before expiring.
    /// </summary>
    public int TimeoutSeconds { get; set; }
}