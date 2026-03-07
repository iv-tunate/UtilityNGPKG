using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityNGPKG.Pagination.UtilityNGPKG.Pagination;

namespace UtilityNGPKG.Pagination
{
    /// <summary>
    /// A factory interface for creating instances of <see cref="PaginationHelper{T}"/> via Dependency Injection.
    /// </summary>
    /// <remarks>
    /// Inject this interface into your services or controllers to generate configured pagination helpers 
    /// without manually instantiating them, promoting better testing and loose coupling.
    /// </remarks>
    public interface IPaginationHelperFactory
    {
        /// <summary>
        /// Creates a new configured instance of <see cref="PaginationHelper{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The entire in-memory collection of items to be paginated.</param>
        /// <param name="itemsPerPage">The limit dictating how many items appear on a single page.</param>
        /// <returns>A new instance of <see cref="PaginationHelper{T}"/> ready to serve paginated responses.</returns>
        PaginationHelper<T> Create<T>(IEnumerable<T> collection, int itemsPerPage);
    }
}
