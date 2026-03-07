using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.Pagination
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    namespace UtilityNGPKG.Pagination
    {
        /// <summary>
        /// Request model used by clients to pass pagination parameters.
        /// </summary>
        /// <remarks>
        /// Instantiate or bind this class in your API endpoints or services to capture the desired page number and page size.
        /// </remarks>
        public class PaginationRequest
        {
            /// <summary>
            /// The target page number (1-based index). Default is 1.
            /// </summary>
            public int PageNumber { get; set; } = 1;

            /// <summary>
            /// The requested number of items to retrieve per page. Default is 10.
            /// </summary>
            public int PageSize { get; set; } = 10;

            /// <summary>
            /// Validates and corrects the pagination parameters to ensure they fall within safe boundaries.
            /// </summary>
            /// <remarks>
            /// Forces <see cref="PageNumber"/> to be at least 1. 
            /// Forces <see cref="PageSize"/> to be at least 10, and caps it at a maximum of 100 to prevent excessively large data requests.
            /// </remarks>
            public void Validate()
            {
                if (PageNumber < 1)
                    PageNumber = 1;

                if (PageSize < 1)
                    PageSize = 10;

                if (PageSize > 100)
                    PageSize = 100;
            }
        }

        /// <summary>
        /// A standardized generic response envelope for returning paginated datasets.
        /// </summary>
        /// <typeparam name="T">The type of the data elements contained within the page.</typeparam>
        /// <remarks>
        /// Return an instance of this class from your services or API controllers. It bundles the actual requested <see cref="Data"/> slice
        /// together with necessary metadata like <see cref="TotalPages"/> and <see cref="TotalItems"/>, allowing clients to easily build navigational UI controls.
        /// </remarks>
        public class PaginatedResponse<T>
        {
            /// <summary>
            /// The subset list of data items belonging to the current requested page.
            /// </summary>
            public List<T> Data { get; set; } = new();

            /// <summary>
            /// The current page number being returned (1-based index).
            /// </summary>
            public int CurrentPage { get; set; }

            /// <summary>
            /// The maximum number of items allowed per page, as requested or evaluated.
            /// </summary>
            public int PageSize { get; set; }

            /// <summary>
            /// The total absolute count of items available across the entire dataset (before pagination slicing).
            /// </summary>
            public int TotalItems { get; set; }

            /// <summary>
            /// The total calculated number of pages required to display the entire dataset using the current <see cref="PageSize"/>.
            /// </summary>
            public int TotalPages { get; set; }

            /// <summary>
            /// Indicates whether a subsequent page exists sequentially after the current one.
            /// </summary>
            public bool HasNextPage => CurrentPage < TotalPages;

            /// <summary>
            /// Indicates whether a preceding page exists sequentially before the current one.
            /// </summary>
            public bool HasPreviousPage => CurrentPage > 1;

            /// <summary>
            /// The sequential number of the next page, or <c>null</c> if the current page is the final page.
            /// </summary>
            public int? NextPage => HasNextPage ? CurrentPage + 1 : null;

            /// <summary>
            /// The sequential number of the previous page, or <c>null</c> if the current page is the first page.
            /// </summary>
            public int? PreviousPage => HasPreviousPage ? CurrentPage - 1 : null;
        }

        /// <summary>
        /// A helper utility designed to paginate in-memory collections easily.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <remarks>
        /// There are two primary ways to utilize this helper:
        /// <list type="number">
        /// <item>
        /// <b>Direct Instantiation</b>: Create an instance manually by calling <c>new PaginationHelper&lt;T&gt;(myData, pageSize)</c>.
        /// </item>
        /// <item>
        /// <b>Via Dependency Injection (Recommended)</b>: Inject <see cref="IPaginationHelperFactory"/> into your service/controller and call <see cref="IPaginationHelperFactory.Create{T}(IEnumerable{T}, int)"/>.
        /// </item>
        /// </list>
        /// <para>
        /// Example usage via Factory:
        /// <code>
        /// var myData = new List&lt;string&gt; { "A", "B", "C", "D" };
        /// var helper = _paginationFactory.Create(myData, 2);
        /// var response = helper.GetPagedResponse(1, 2); // Yields Page 1 containing "A" and "B" with all metadata included.
        /// </code>
        /// </para>
        /// </remarks>
        public class PaginationHelper<T>
        {
            private readonly List<T> _collection;
            private readonly int _itemsPerPage;

            /// <summary>
            /// Initializes a new instance of the <see cref="PaginationHelper{T}"/> using the provided collection.
            /// </summary>
            /// <param name="collection">The entire in-memory collection of items to be evaluated and paginated.</param>
            /// <param name="itemsPerPage">The predefined limit dictating how many items appear on a single page.</param>
            /// <exception cref="ArgumentNullException">Thrown if the provided <paramref name="collection"/> is null.</exception>
            /// <exception cref="ArgumentException">Thrown if <paramref name="itemsPerPage"/> is set to a value less than 1.</exception>
            /// <remarks>
            /// Note that instantiating this class copies the source <paramref name="collection"/> to a local list. It is suited for moderate 
            /// in-memory collections, but may not be optimal for huge database tracking sets where executing <c>Skip()</c> and <c>Take()</c> 
            /// natively via an IQueryable would be vastly more efficient.
            /// Alternatively, use <see cref="IPaginationHelperFactory.Create{T}(IEnumerable{T}, int)"/> injected via DI.
            /// </remarks>
            public PaginationHelper(IEnumerable<T> collection, int itemsPerPage)
            {
                if (collection == null)
                    throw new ArgumentNullException(nameof(collection), "Collection cannot be null");

                if (itemsPerPage < 1)
                    throw new ArgumentException("Items per page must be greater than 0", nameof(itemsPerPage));

                _collection = collection.ToList();
                _itemsPerPage = itemsPerPage;
            }

            /// <summary>
            /// The total number of items in the collection.
            /// </summary>
            public int TotalItemCount => _collection.Count;

            /// <summary>
            /// The total number of pages required to display all items using the configured page size.
            /// </summary>
            public int TotalPageCount => (int)Math.Ceiling((double)_collection.Count / _itemsPerPage);

            /// <summary>
            /// Returns the number of items on a specific page.
            /// </summary>
            /// <param name="pageIndex">The zero-based page index (e.g. 0 denotes the first page).</param>
            /// <returns>The item count for the given page, or <c>-1</c> if the page index is out of range.</returns>
            public int GetPageItemCount(int pageIndex)
            {
                if (pageIndex < 0 || pageIndex >= TotalPageCount)
                    return -1;

                // Last page may have fewer items than a full page
                if (pageIndex == TotalPageCount - 1)
                {
                    int remainder = _collection.Count % _itemsPerPage;
                    return remainder == 0 ? _itemsPerPage : remainder;
                }

                return _itemsPerPage;
            }

            /// <summary>
            /// Returns the zero-based page index that contains the item at the given item index.
            /// </summary>
            /// <param name="itemIndex">The zero-based absolute index of the target item.</param>
            /// <returns>The zero-based page index, or <c>-1</c> if the item index is out of range.</returns>
            public int GetPageIndex(int itemIndex)
            {
                if (itemIndex < 0 || itemIndex >= _collection.Count)
                    return -1;

                return itemIndex / _itemsPerPage;
            }

            /// <summary>
            /// Returns the list of items belonging to the specified page.
            /// </summary>
            /// <param name="pageIndex">The zero-based page index.</param>
            /// <returns>The items on the requested page, or an empty list if the page index is out of range.</returns>
            public List<T> GetPage(int pageIndex)
            {
                if (pageIndex < 0 || pageIndex >= TotalPageCount)
                    return new List<T>();

                int startIndex = pageIndex * _itemsPerPage;
                return _collection.Skip(startIndex).Take(_itemsPerPage).ToList();
            }

            /// <summary>
            /// Returns a paginated slice of the collection together with metadata such as total pages and navigation flags.
            /// </summary>
            /// <param name="pageNumber">The desired page number (1-based index). Values below 1 are clamped to 1.</param>
            /// <param name="pageSize">The number of items per page. Falls back to the constructor value if less than 1.</param>
            /// <returns>A <see cref="PaginatedResponse{T}"/> containing the page data and full pagination metadata.</returns>
            /// <remarks>
            /// Use this overload when you want a simple page slice with no filtering or sorting.
            /// <para>Example: <c>helper.GetPagedResponse(2, 10)</c> returns the second page of 10 items each.</para>
            /// </remarks>
            public PaginatedResponse<T> GetPagedResponse(int pageNumber, int pageSize)
            {
                (pageNumber, pageSize, int pageIndex) = NormalizePage(pageNumber, pageSize, _collection.Count);

                var data = GetPage(pageIndex);

                return new PaginatedResponse<T>
                {
                    Data = data,
                    CurrentPage = pageIndex + 1,
                    PageSize = pageSize,
                    TotalItems = _collection.Count,
                    TotalPages = TotalPageCount
                };
            }

            /// <summary>
            /// Filters the collection using a predicate, then returns a paginated response of the matching items.
            /// </summary>
            /// <param name="predicate">A function to test each item; only items that return <c>true</c> are included.</param>
            /// <param name="pageNumber">The desired page number (1-based index). Values below 1 are clamped to 1.</param>
            /// <param name="pageSize">The number of items per page. Falls back to the constructor value if less than 1.</param>
            /// <returns>A <see cref="PaginatedResponse{T}"/> containing the filtered page data and updated pagination metadata.</returns>
            /// <remarks>
            /// The returned <see cref="PaginatedResponse{T}.TotalItems"/> reflects the count of matched items only, not the full collection size.
            /// </remarks>
            public PaginatedResponse<T> GetPagedResponse(Func<T, bool> predicate, int pageNumber, int pageSize)
            {
                var filteredCollection = _collection.Where(predicate).ToList();

                if (filteredCollection.Count == 0)
                {
                    return new PaginatedResponse<T>
                    {
                        Data = new List<T>(),
                        CurrentPage = pageNumber,
                        PageSize = pageSize,
                        TotalItems = 0,
                        TotalPages = 0
                    };
                }

                (pageNumber, pageSize, int pageIndex) = NormalizePage(pageNumber, pageSize, filteredCollection.Count);

                int totalPages = (int)Math.Ceiling((double)filteredCollection.Count / pageSize);
                var startIndex = pageIndex * pageSize;
                var data = filteredCollection.Skip(startIndex).Take(pageSize).ToList();

                return new PaginatedResponse<T>
                {
                    Data = data,
                    CurrentPage = pageIndex + 1,
                    PageSize = pageSize,
                    TotalItems = filteredCollection.Count,
                    TotalPages = totalPages
                };
            }

            /// <summary>
            /// Sorts the collection by the specified key, then returns a paginated response of the ordered items.
            /// </summary>
            /// <typeparam name="TKey">The type of the value used for sorting (e.g. <c>string</c>, <c>DateTime</c>).</typeparam>
            /// <param name="keySelector">A function that extracts the sort key from each item (e.g. <c>x => x.CreatedAt</c>).</param>
            /// <param name="pageNumber">The desired page number (1-based index). Values below 1 are clamped to 1.</param>
            /// <param name="pageSize">The number of items per page. Falls back to the constructor value if less than 1.</param>
            /// <param name="descending">Pass <c>true</c> to sort in descending order (newest first, Z–A, etc.).</param>
            /// <returns>A <see cref="PaginatedResponse{T}"/> containing the sorted and paginated data.</returns>
            /// <remarks>
            /// Ideal when you need dynamic ordering before presenting results (e.g. sort products by price, or posts by date).
            /// </remarks>
            public PaginatedResponse<T> GetPagedResponse<TKey>(
                Func<T, TKey> keySelector,
                int pageNumber,
                int pageSize,
                bool descending = false)
            {
                var sortedCollection = descending
                    ? _collection.OrderByDescending(keySelector).ToList()
                    : _collection.OrderBy(keySelector).ToList();

                (pageNumber, pageSize, int pageIndex) = NormalizePage(pageNumber, pageSize, sortedCollection.Count);

                int totalPages = (int)Math.Ceiling((double)sortedCollection.Count / pageSize);
                var startIndex = pageIndex * pageSize;
                var data = sortedCollection.Skip(startIndex).Take(pageSize).ToList();

                return new PaginatedResponse<T>
                {
                    Data = data,
                    CurrentPage = pageIndex + 1,
                    PageSize = pageSize,
                    TotalItems = sortedCollection.Count,
                    TotalPages = totalPages
                };
            }

            /// <summary>
            /// Clamps <paramref name="pageNumber"/> and <paramref name="pageSize"/> to valid values and converts to a safe zero-based page index.
            /// </summary>
            /// <param name="pageNumber">The incoming 1-based page number.</param>
            /// <param name="pageSize">The incoming page size.</param>
            /// <param name="totalItems">The total count of items in scope (used to clamp the page index to the last available page).</param>
            /// <returns>A tuple of the clamped page number, page size, and zero-based page index.</returns>
            private (int pageNumber, int pageSize, int pageIndex) NormalizePage(int pageNumber, int pageSize, int totalItems)
            {
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = _itemsPerPage;

                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
                int pageIndex = pageNumber - 1;

                if (totalPages > 0 && pageIndex >= totalPages)
                    pageIndex = totalPages - 1;

                return (pageNumber, pageSize, pageIndex);
            }
        }
    }
}