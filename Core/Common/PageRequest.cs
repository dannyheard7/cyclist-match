namespace Common;

public class PageRequest
{
    public PageRequest(int pageSize, int? page)
    {
        PageSize = pageSize;
        Page = page ?? 0;
    }

    public int PageSize { get; }
    
    public int Page { get; }

    public int Skip => Page * PageSize;

    public static PageRequest All => new PageRequest(int.MaxValue, null);
}