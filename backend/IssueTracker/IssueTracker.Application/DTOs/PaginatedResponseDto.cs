namespace IssueTracker.Application.DTOs;

public class PaginatedResponseDto<T>
{
    public List<T> Items { get; set; } = [];

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;

    public bool HasNextPage => Page < TotalPages;

    public bool HasPreviousPage => Page > 1;
}
