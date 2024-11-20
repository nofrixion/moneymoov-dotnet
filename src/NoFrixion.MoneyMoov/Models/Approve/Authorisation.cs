//-----------------------------------------------------------------------------
// Filename: Authorisation.cs
// 
// Description: Represents the authorisation of an entity by a user.
// 
// Author(s):
// Pablo Maldonado (pablo@nofrixion.com)
// 
// History:
// 20 Nov 2024  Pablo Maldonado   Created, Montevideo,  Uruguay.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

#nullable disable

namespace NoFrixion.MoneyMoov.Models.Approve;

public class Authorisation
{
    public User User { get; set; }

    public DateTimeOffset Timestamp { get; set; }

}