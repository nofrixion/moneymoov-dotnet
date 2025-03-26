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
    int lastSequenceNumber,
    bool hasMorePages)
    : ApiKeysetPageResponseBase<Transaction>(content, lastSequenceNumber, hasMorePages);

public abstract class ApiKeysetPageResponseBase<T>
{
    public List<T> Content { get; set; }

    public int LastSequenceNumber { get; set; }

    public bool HasMorePages { get; set; }
    
    protected ApiKeysetPageResponseBase(
        List<T> content,
        int lastSequenceNumber,
        bool hasMorePages)
    {
        Guard.Against.Null(content, nameof(content));
        
        Content = content;
        LastSequenceNumber = lastSequenceNumber;
        HasMorePages = hasMorePages;
    }
}