using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.Test
{
    using UtilityNGPKG.Pagination;
    using UtilityNGPKG.Pagination.UtilityNGPKG.Pagination;
    using UtilityNGPKG.Test.config;

    public class PaginationHelper_Test : BaseTestFeature
    {
        [Fact]
        public void PaginationHelper_ValidatesParametersAndPaginatesCorrectly()
        {
            var myData = new List<string> { "One", "Two", "Three", "Four", "Five" };

            var helper = paginationHelperFactory.Create<string>(myData, 2);

            var response1 = helper.GetPagedResponse(1, 2);
            Assert.Equal(1, response1.CurrentPage);
            Assert.Equal(3, response1.TotalPages);
            Assert.Equal(5, response1.TotalItems);
            Assert.Equal(2, response1.Data.Count);
            Assert.True(response1.HasNextPage);
            Assert.False(response1.HasPreviousPage);

            var clampedResponse = helper.GetPagedResponse(0, 2);
            Assert.Equal(1, clampedResponse.CurrentPage); // Clamped to 1

            var overResponse = helper.GetPagedResponse(10, 2);
            Assert.Equal(3, overResponse.CurrentPage); // Clamped to max page index (3)
            Assert.Single(overResponse.Data); // Last page has 1 item ("Five"). since it's two per page and we have a total of 5 items

            // Filtering with Predicate
            var filteredResponse = helper.GetPagedResponse(x => x.StartsWith("T"), 1, 10);
            Assert.Equal(2, filteredResponse.TotalItems); // "Two", "Three"
            Assert.Equal(2, filteredResponse.Data.Count);

            //  Sorting with KeySelector
            var sortedResponse = helper.GetPagedResponse(x => x, 1, 2, descending: true);
            // Expected sorted descending: "Two", "Three", "One", "Four", "Five"
            Assert.Equal("Two", sortedResponse.Data[0]);
            Assert.Equal("Three", sortedResponse.Data[1]);
        }

        [Fact]
        public void PaginationFactory_CreatesValidHelper()
        {
            var numbers = new int[] { 10, 20, 30 };
            var helper = paginationHelperFactory.Create(numbers, 2);

            Assert.NotNull(helper);
            var response = helper.GetPagedResponse(1, 10);
            Assert.Equal(3, response.TotalItems);
        }
    }
}
