using System.Text;

namespace NoFrixion.MoneyMoov.Models.Payouts;

/// <summary>
/// Builds and compares payout approval hashes.
/// New approvals use a canonical hash that stays stable for destination-authoritative FX payouts,
/// while matching still accepts the legacy source-amount hash shape where compatibility is required.
/// </summary>
public static class PayoutApprovalHash
{
    private const string DESTINATION_AUTHORITATIVE_AMOUNT_PREFIX = "FXDEST";

    /// <summary>
    /// Creates the canonical approval hash for the supplied payout.
    /// For destination-authoritative FX payouts the destination amount/currency is used as the stable
    /// identity component so FX re-quotes do not change the approval hash.
    /// </summary>
    public static string Create(Payout? payout)
    {
        return CreateCore(payout, GetCanonicalAmountComponent(payout));
    }

    /// <summary>
    /// Checks whether a candidate hash matches the payout approval hash as the payout exists now.
    /// This accepts both the current canonical hash and the legacy source-amount hash recreated from
    /// the payout's current state so existing approval tokens remain valid during the transition.
    /// </summary>
    public static bool MatchesApprovalHash(Payout? payout, string? candidateHash)
    {
        if (payout == null || string.IsNullOrEmpty(candidateHash))
        {
            return false;
        }

        var canonicalHash = Create(payout);
        var legacyHashFromCurrentState = CreateLegacyFromCurrentState(payout);

        return candidateHash == canonicalHash || candidateHash == legacyHashFromCurrentState;
    }

    /// <summary>
    /// Checks whether a candidate hash matches the legacy approval hash for a fixed-destination FX payout
    /// using the source amount recorded on a historical event. This compatibility path is intentionally
    /// limited to destination-authoritative FX payouts so normal payouts still fail if the source amount
    /// changes after authorisation or signing.
    /// </summary>
    public static bool MatchesLegacyFixedDestinationFxApprovalHash(
        Payout? payout,
        string? candidateHash,
        decimal recordedSourceAmount)
    {
        if (!IsDestinationAuthoritativeFxPayout(payout) || string.IsNullOrEmpty(candidateHash))
        {
            return false;
        }

        var legacyHashFromRecordedState = CreateLegacy(payout!, recordedSourceAmount);
        return candidateHash == legacyHashFromRecordedState;
    }

    /// <summary>
    /// Creates the canonical approval hash for a batch payout by hashing the canonical hash of each child payout.
    /// </summary>
    public static string Create(BatchPayout? batchPayout)
    {
        var batchInput = new StringBuilder();

        foreach (var payout in batchPayout?.Payouts ?? [])
        {
            batchInput.Append(Create(payout));
        }

        return HashHelper.CreateHash(batchInput.ToString());
    }

    /// <summary>
    /// Checks whether a candidate hash matches the batch payout approval hash as the batch exists now, accepting either the
    /// current canonical batch hash or the legacy batch hash rebuilt from the current child payouts.
    /// </summary>
    public static bool MatchesApprovalHash(BatchPayout? batchPayout, string? candidateHash)
    {
        if (batchPayout == null || string.IsNullOrEmpty(candidateHash))
        {
            return false;
        }

        var canonicalHash = Create(batchPayout);
        var legacyHashFromCurrentState = CreateLegacy(batchPayout);

        return candidateHash == canonicalHash || candidateHash == legacyHashFromCurrentState;
    }

    private static string CreateLegacyFromCurrentState(Payout payout)
    {
        return CreateLegacy(payout, payout.Amount);
    }

    private static string CreateLegacy(Payout payout, decimal sourceAmount)
    {
        return CreateCore(payout, sourceAmount.ToAmountMinorUnits(payout.Currency).ToString());
    }

    private static string CreateLegacy(BatchPayout batchPayout)
    {
        var batchInput = new StringBuilder();

        foreach (var payout in batchPayout.Payouts)
        {
            batchInput.Append(CreateLegacyFromCurrentState(payout));
        }

        return HashHelper.CreateHash(batchInput.ToString());
    }

    private static string CreateCore(Payout? payout, string amountComponent)
    {
        if (payout?.Destination == null)
        {
            return string.Empty;
        }

        var input =
            payout.ID.ToString() +
            payout.AccountID.ToString() +
            payout.Currency +
            amountComponent +
            payout.Destination.GetApprovalHash() +
            payout.Scheduled.GetValueOrDefault().ToString() +
            payout.ScheduleDate?.ToString("o") +
            (string.IsNullOrEmpty(payout.Nonce) ? string.Empty : payout.Nonce);

        return HashHelper.CreateHash(input);
    }

    private static string GetCanonicalAmountComponent(Payout? payout)
    {
        if (IsDestinationAuthoritativeFxPayout(payout))
        {
            // Prefix the destination-based component so canonical FX hashes cannot collide with the
            // legacy source-minor-units representation for a non-FX or source-authoritative payout.
            var destinationAmountMinorUnits =
                payout!.FxDestinationAmount!.Value.ToAmountMinorUnits(payout.FxDestinationCurrency!.Value);

            return $"{DESTINATION_AUTHORITATIVE_AMOUNT_PREFIX}{payout.FxDestinationCurrency}{destinationAmountMinorUnits}";
        }

        return payout?.AmountMinorUnits.ToString() ?? string.Empty;
    }

    private static bool IsDestinationAuthoritativeFxPayout(Payout? payout)
    {
        return payout is { FxUseDestinationAmount: true, FxDestinationCurrency: not null, FxDestinationAmount: not null };
    }
}
