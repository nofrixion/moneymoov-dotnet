namespace NoFrixion.MoneyMoov.Models.Payouts;

/// <summary>
/// Convenience extensions for the canonical payout approval-hash API.
/// Hash construction still lives in <see cref="PayoutApprovalHash"/>; these helpers only provide
/// a leaner call-site syntax.
/// </summary>
public static class PayoutApprovalHashExtensions
{
    /// <summary>
    /// Creates the canonical approval hash for the supplied payout.
    /// </summary>
    public static string ToApprovalHash(this Payout? payout)
    {
        return PayoutApprovalHash.Create(payout);
    }

    /// <summary>
    /// Creates the canonical approval hash for the supplied batch payout.
    /// </summary>
    public static string ToApprovalHash(this BatchPayout? batchPayout)
    {
        return PayoutApprovalHash.Create(batchPayout);
    }
}
