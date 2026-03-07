using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityNGPKG.Pagination.UtilityNGPKG.Pagination;

namespace UtilityNGPKG.Pagination
{
    /// <summary>
    /// The default internal implementation of <see cref="IPaginationHelperFactory"/>.
    /// Registered as a singleton in the dependency injection container.
    /// </summary>
    internal class PaginationHelperFactory : IPaginationHelperFactory
    {
        /// <inheritdoc/>
        public PaginationHelper<T> Create<T>(IEnumerable<T> collection, int itemsPerPage)
            => new PaginationHelper<T>(collection, itemsPerPage);
    }
}
