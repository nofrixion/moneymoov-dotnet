// -----------------------------------------------------------------------------
//  Filename: ApiKeysetPageResponseBase.cs
// 
//  Description: Keyset page response model:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  26 03 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using Ardalis.GuardClauses;

namespace NoFrixion.MoneyMoov.Models;

public class TransactionKeysetPageResponse(
    List<Transaction> content,
    int lastKey,
    bool hasMorePages)
    : ApiKeysetPageResponseBase<Transaction, int>(content, lastKey, hasMorePages);

public class PayoutKeysetPageResponse(
    List<Payout> content,
    string lastKey,
    bool hasMorePages)
    : ApiKeysetPageResponseBase<Payout, string>(content, lastKey, hasMorePages);

public abstract class ApiKeysetPageResponseBase<TContentType, TKeyType>
{
    public List<TContentType> Content { get; set; }

    public TKeyType LastKey { get; set; }

    public bool HasMorePages { get; set; }
    
    protected ApiKeysetPageResponseBase(
        List<TContentType> content,
        TKeyType lastKey,
        bool hasMorePages)
    {
        Guard.Against.Null(content, nameof(content));
        
        Content = content;
        LastKey = lastKey;
        HasMorePages = hasMorePages;
    }
}