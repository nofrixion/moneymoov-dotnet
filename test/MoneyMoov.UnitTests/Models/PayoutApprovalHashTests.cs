using System.Text;
using NoFrixion.MoneyMoov;
using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;
using NoFrixion.MoneyMoov.Models.Payouts;
using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests;

public class PayoutApprovalHashTests
{
    [Fact]
    public void OriginalHashRegressionVector_RemainsStable()
    {
        var payout = BuildPayout(
            fxUseDestinationAmount: true,
            id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            accountId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            amount: 90.12m,
            nonce: "test-nonce");

        var legacyApprovalHash = CreateOriginalApprovalHash(payout);

        Assert.Equal("s-OwFKnDubOL1YwboQDJ_WgCozElZepOIpXSeIexnPA", legacyApprovalHash);
        Assert.True(PayoutApprovalHash.MatchesApprovalHash(payout, legacyApprovalHash));
    }

    [Fact]
    public void Create_FixedDestinationFxPayout_RemainsStable_WhenSourceAmountChanges()
    {
        var payout = BuildPayout(fxUseDestinationAmount: true);

        var approvalHash = payout.ToApprovalHash();
        var legacyApprovalHash = CreateOriginalApprovalHash(payout);

        payout.Amount = 91.23m;

        Assert.Equal(approvalHash, payout.ToApprovalHash());
        Assert.NotEqual(legacyApprovalHash, CreateOriginalApprovalHash(payout));
    }

    [Fact]
    public void MatchesApprovalHash_AcceptsOriginalHash_ForUnchangedFixedDestinationFxPayout()
    {
        var payout = BuildPayout(fxUseDestinationAmount: true);

        var legacyApprovalHash = CreateOriginalApprovalHash(payout);

        Assert.True(PayoutApprovalHash.MatchesApprovalHash(payout, legacyApprovalHash));
    }

    [Fact]
    public void MatchesLegacyFixedDestinationFxApprovalHash_AcceptsOriginalHash_AfterSourceOnlyRequote()
    {
        var payout = BuildPayout(fxUseDestinationAmount: true);
        var legacyApprovalHash = CreateOriginalApprovalHash(payout);

        payout.Amount = 91.23m;
        Assert.True(PayoutApprovalHash.MatchesLegacyFixedDestinationFxApprovalHash(payout, legacyApprovalHash, 90m));
    }

    [Fact]
    public void MatchesLegacyFixedDestinationFxApprovalHash_RejectsOriginalHash_AfterNonceChange()
    {
        var payout = BuildPayout(fxUseDestinationAmount: true);
        var legacyApprovalHash = CreateOriginalApprovalHash(payout);

        payout.Amount = 91.23m;
        payout.Nonce = "updated-nonce";
        Assert.False(PayoutApprovalHash.MatchesLegacyFixedDestinationFxApprovalHash(payout, legacyApprovalHash, 90m));
    }

    [Fact]
    public void Create_SourceAuthoritativeFxPayout_Changes_WhenSourceAmountChanges()
    {
        var payout = BuildPayout(fxUseDestinationAmount: false);

        var approvalHash = payout.ToApprovalHash();

        payout.Amount = 91.23m;

        Assert.NotEqual(approvalHash, payout.ToApprovalHash());
    }

    [Fact]
    public void MatchesLegacyFixedDestinationFxApprovalHash_RejectsOriginalHash_ForSourceAuthoritativeFx_AfterSourceAmountChange()
    {
        var payout = BuildPayout(fxUseDestinationAmount: false);
        var legacyApprovalHash = CreateOriginalApprovalHash(payout);

        payout.Amount = 91.23m;

        Assert.False(PayoutApprovalHash.MatchesLegacyFixedDestinationFxApprovalHash(payout, legacyApprovalHash, 90m));
    }

    [Fact]
    public void MatchesLegacyFixedDestinationFxApprovalHash_RejectsOriginalHash_ForNonFx_AfterAmountChange()
    {
        var payout = BuildPayout(fxUseDestinationAmount: false);
        payout.FxDestinationCurrency = null;
        payout.FxDestinationAmount = null;

        var legacyApprovalHash = CreateOriginalApprovalHash(payout);

        payout.Amount = 91.23m;

        Assert.False(PayoutApprovalHash.MatchesLegacyFixedDestinationFxApprovalHash(payout, legacyApprovalHash, 90m));
    }

    [Fact]
    public void MatchesApprovalHash_BatchPayout_AcceptsOriginalCompositeHash_ForUnchangedCurrentState()
    {
        var batchPayout = new BatchPayout
        {
            ID = Guid.NewGuid(),
            Payouts =
            [
                BuildPayout(fxUseDestinationAmount: true),
                BuildPayout(fxUseDestinationAmount: false, amount: 12.34m, nonce: "batch-two")
            ]
        };

        var legacyApprovalHash = CreateOriginalBatchApprovalHash(batchPayout);

        Assert.True(PayoutApprovalHash.MatchesApprovalHash(batchPayout, legacyApprovalHash));
    }

    [Fact]
    public void MatchesApprovalHash_BatchPayout_RejectsOriginalCompositeHash_AfterNonceChange()
    {
        var batchPayout = new BatchPayout
        {
            ID = Guid.NewGuid(),
            Payouts =
            [
                BuildPayout(fxUseDestinationAmount: true),
                BuildPayout(fxUseDestinationAmount: false, amount: 12.34m, nonce: "batch-two")
            ]
        };

        var legacyApprovalHash = CreateOriginalBatchApprovalHash(batchPayout);
        batchPayout.Payouts[0].Nonce = "updated-nonce";

        Assert.False(PayoutApprovalHash.MatchesApprovalHash(batchPayout, legacyApprovalHash));
    }

    private static Payout BuildPayout(
        bool fxUseDestinationAmount,
        Guid? id = null,
        Guid? accountId = null,
        decimal amount = 90m,
        string nonce = "test-nonce")
    {
        return new Payout
        {
            ID = id ?? Guid.NewGuid(),
            AccountID = accountId ?? Guid.NewGuid(),
            Currency = CurrencyTypeEnum.EUR,
            Amount = amount,
            Destination = new Counterparty
            {
                Name = "Destination Account",
                Identifier = new AccountIdentifier
                {
                    IBAN = "GB23MOCK00000070604447",
                    Currency = CurrencyTypeEnum.EUR
                }
            },
            Nonce = nonce,
            FxUseDestinationAmount = fxUseDestinationAmount,
            FxDestinationCurrency = CurrencyTypeEnum.GBP,
            FxDestinationAmount = 75m
        };
    }

    private static string CreateOriginalApprovalHash(Payout payout, decimal? sourceAmount = null)
    {
        var amountMinorUnits = (sourceAmount ?? payout.Amount).ToAmountMinorUnits(payout.Currency).ToString();
        var input =
            payout.ID.ToString() +
            payout.AccountID.ToString() +
            payout.Currency +
            amountMinorUnits +
            payout.Destination!.GetApprovalHash() +
            payout.Scheduled.GetValueOrDefault().ToString() +
            payout.ScheduleDate?.ToString("o") +
            (string.IsNullOrEmpty(payout.Nonce) ? string.Empty : payout.Nonce);

        return HashHelper.CreateHash(input);
    }

    private static string CreateOriginalBatchApprovalHash(BatchPayout batchPayout)
    {
        var input = new StringBuilder();

        foreach (var payout in batchPayout.Payouts)
        {
            input.Append(CreateOriginalApprovalHash(payout));
        }

        return HashHelper.CreateHash(input.ToString());
    }
}
