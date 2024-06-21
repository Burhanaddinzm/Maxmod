namespace Maxmod.ViewModels.Pagination;

/// <summary>
/// ViewModel for handling pagination.
/// </summary>
public class PagerVM
{
    /// <summary>
    /// Gets the total number of items.
    /// </summary>
    public int TotalItems { get; private set; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int CurrentPage { get; private set; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; private set; }

    /// <summary>
    /// Gets the first page number to display in the pagination control.
    /// </summary>
    public int StartPage { get; private set; }

    /// <summary>
    /// Gets the last page number to display in the pagination control.
    /// </summary>
    public int EndPage { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagerVM"/> class.
    /// </summary>
    public PagerVM()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PagerVM"/> class with the specified parameters.
    /// </summary>
    /// <param name="totalItems">The total number of items.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The number of items per page. Default is 6.</param>
    public PagerVM(int totalItems, int page, int pageSize = 6)
    {
        // Calculate the total number of pages.
        int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
        int currentPage = page;

        // Determine the start and end pages.
        int startPage = currentPage - 4;
        int endPage = currentPage + 3;

        // Adjust startPage if it is less than or equal to 0.
        if (startPage <= 0)
        {
            endPage -= (startPage - 1);
            startPage = 1;
        }

        // Adjust endPage if it exceeds totalPages.
        if (endPage > totalPages)
        {
            endPage = totalPages;
            if (endPage > 8)
            {
                startPage = endPage - 7;
            }
        }

        // Set properties.
        TotalItems = totalItems;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = totalPages;
        StartPage = startPage;
        EndPage = endPage;
    }
}
