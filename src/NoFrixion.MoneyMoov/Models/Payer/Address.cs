// -----------------------------------------------------------------------------
//  Filename: Address.cs
// 
//  Description: Payer's address:
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
public class Address
{
    public string Country { get; set; }

    public string PostCode { get; set; }

    public string PostTown { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public override string ToString()
    {
        return $"{nameof(Country)}: {Country}, {nameof(PostCode)}: {PostCode}, {nameof(PostTown)}: {PostTown}, {nameof(AddressLine1)}: {AddressLine1}, {nameof(AddressLine2)}: {AddressLine2}";
    }
}