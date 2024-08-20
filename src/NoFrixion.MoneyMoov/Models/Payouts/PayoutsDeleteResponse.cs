// -----------------------------------------------------------------------------
//  Filename: PayoutsDeleteResponse.cs
// 
//  Description: Contains the response for deleting multiple payouts in a single request.
//
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  19 Aug 2024  Saurav Maiti   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PayoutsDeleteResponse
{
    public List<PayoutDeleteResult> PayoutDeleteResults { get; set; } = [];
}

public class PayoutDeleteResult
{
    public Guid PayoutID { get; set; }
    
    public bool Success { get; set; }
    
    public NoFrixionProblem Problem { get; set; } = NoFrixionProblem.Empty;
}