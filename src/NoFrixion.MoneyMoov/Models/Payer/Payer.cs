// -----------------------------------------------------------------------------
//  Filename: Payer.cs
// 
//  Description: Contains details about the payer of a transaction:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class Payer
{
    public string Name { get; set; }

    public Address Address { get; set; }

    public Identifier Identifier { get; set; }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Address)}: {Address}, {nameof(Identifier)}: {Identifier}";
    }
}