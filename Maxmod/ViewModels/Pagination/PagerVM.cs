namespace Maxmod.ViewModels.Pagination;

public class PagerVM
{
    public int TotalItems { get; private set; }
    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public int StartPage { get; private set; }
    public int EndPage { get; private set; }

    public PagerVM()
    {
    }

    public PagerVM(int totalItems, int page, int pageSize = 6)
    {
        int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
        int currentPage = page;

        int startPage = currentPage - 4;
        int endPage = currentPage + 3;

        if (startPage <= 0)
        {
            endPage -= (startPage - 1);
            startPage = 1;
        }

        if (endPage > totalPages)
        {
            endPage = totalPages;
            if(endPage > 8)
            {
                startPage = endPage - 7;
            }
        }

        TotalItems = totalItems;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = totalPages;
        StartPage = startPage;
        EndPage = endPage;
    }
}
