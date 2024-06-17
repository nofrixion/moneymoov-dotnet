//-----------------------------------------------------------------------------
// Filename: PayrunSubmit.cs
// 
// Description: Represent the model to submit a Payrun 
// 
// Author(s):
// Pablo Maldonado (pablo@nofrixion.com)
// 
// History:
// 10 Jun 2024  Pablo Maldonado   Created, Montevideo, Montevideo, Uruguay.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PayrunSubmit
{
    public DateTimeOffset? ScheduledDate { get; set; }
}