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