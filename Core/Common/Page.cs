namespace Common;

public class Page<T> where T : class
{
    public Page(IReadOnlyCollection<T> items, int pageNumber, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalCount = totalCount;
    }

    public IReadOnlyCollection<T> Items { get; }
    
    public int PageNumber { get; }
    
    public int TotalCount { get; }

    public bool HasNextPage => Items.Count * PageNumber < TotalCount;

    public bool HasPreviousPage => PageNumber > 0;
}