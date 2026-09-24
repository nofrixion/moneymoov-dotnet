using System.Text;

namespace NoFrixion.MoneyMoov.Models.Payouts;

public static class PayoutApprovalHash
{
    private const string DESTINATION_AUTHORITATIVE_AMOUNT_PREFIX = "FXDEST";

    public static string Create(Payout? payout)
    {
        return CreateCore(payout, GetCanonicalAmountComponent(payout));
    }

    public static bool MatchesCurrent(Payout? payout, string? candidateHash)
    {
        if (payout == null || string.IsNullOrEmpty(candidateHash))
        {
            return false;
        }

        return candidateHash == Create(payout) || candidateHash == CreateOriginalFromCurrentState(payout);
    }

    public static bool MatchesRecorded(Payout? payout, string? candidateHash, decimal recordedSourceAmount)
    {
        if (payout == null || string.IsNullOrEmpty(candidateHash))
        {
            return false;
        }

        return candidateHash == Create(payout) || candidateHash == CreateOriginal(payout, recordedSourceAmount);
    }

    public static string Create(BatchPayout? batchPayout)
    {
        var batchInput = new StringBuilder();

        foreach (var payout in batchPayout?.Payouts ?? [])
        {
            batchInput.Append(Create(payout));
        }

        return HashHelper.CreateHash(batchInput.ToString());
    }

    public static bool MatchesCurrent(BatchPayout? batchPayout, string? candidateHash)
    {
        if (batchPayout == null || string.IsNullOrEmpty(candidateHash))
        {
            return false;
        }

        return candidateHash == Create(batchPayout) || candidateHash == CreateOriginal(batchPayout);
    }

    private static string CreateOriginalFromCurrentState(Payout payout)
    {
        return CreateOriginal(payout, payout.Amount);
    }

    private static string CreateOriginal(Payout payout, decimal sourceAmount)
    {
        return CreateCore(payout, sourceAmount.ToAmountMinorUnits(payout.Currency).ToString());
    }

    private static string CreateOriginal(BatchPayout batchPayout)
    {
        var batchInput = new StringBuilder();

        foreach (var payout in batchPayout.Payouts)
        {
            batchInput.Append(CreateOriginalFromCurrentState(payout));
        }

        return HashHelper.CreateHash(batchInput.ToString());
    }

    private static string CreateCore(Payout? payout, string amountComponent)
    {
        if (payout?.Destination == null)
        {
            return string.Empty;
        }

        string input =
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
        if (payout is { FxUseDestinationAmount: true, FxDestinationCurrency: not null, FxDestinationAmount: not null })
        {
            var destinationAmountMinorUnits =
                payout.FxDestinationAmount.Value.ToAmountMinorUnits(payout.FxDestinationCurrency.Value);

            return $"{DESTINATION_AUTHORITATIVE_AMOUNT_PREFIX}{payout.FxDestinationCurrency}{destinationAmountMinorUnits}";
        }

        return payout?.AmountMinorUnits.ToString() ?? string.Empty;
    }
}
