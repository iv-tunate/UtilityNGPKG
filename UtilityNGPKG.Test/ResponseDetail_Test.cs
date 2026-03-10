using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.Test
{
    using UtilityNGPKG.ResponseDetail;

    public class ResponseDetail_Test
    {
        [Fact]
        public void SuccessfulResponses_MapPropertiesCorrectly()
        {
            var dataPayload = new { Id = 1, Name = "Test" };

            var response = ResponseDetail<object>.Successful(dataPayload, "Operation completed", 200);

            Assert.True(response.IsSuccess);
            Assert.Equal("Operation completed", response.Message);
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(response.Data);
            Assert.Null(response.Error);
        }

        [Fact]
        public void SuccessfulPaginatedResponse_MapsMetadataCorrectly()
        {
            var dataList = new[] { "A", "B", "C" };

            var response = ResponseDetail<string[]>.SuccessfulPaginatedResponse(
                dataList,
                totalCount: 100,
                totalPages: 10,
                pageNumber: 2,
                message: "Fetched page",
                statusCode: 200
            );

            Assert.True(response.IsSuccess);
            Assert.Equal(100, response.TotalCount);
            Assert.Equal(10, response.TotalPages);
            Assert.Equal(2, response.PageNumber);
            Assert.NotNull(response.Data);
            Assert.Equal(3, response.Data.Length);
        }

        [Fact]
        public void FailedResponses_MapPropertiesCorrectly()
        {
            var fail1 = ResponseDetail<string>.Failed("An error occurred", 500, "InternalError");

            Assert.False(fail1.IsSuccess);
            Assert.Equal("An error occurred", fail1.Message);
            Assert.Equal(500, fail1.StatusCode);
            Assert.Equal("InternalError", fail1.Error);
            Assert.Null(fail1.Data); 

            var fail2 = ResponseDetail<string>.Failed("Partial data", "Data processing failed", 400, "ValidationError");

            Assert.False(fail2.IsSuccess);
            Assert.Equal("Data processing failed", fail2.Message);
            Assert.Equal(400, fail2.StatusCode);
            Assert.Equal("ValidationError", fail2.Error);
            Assert.Equal("Partial data", fail2.Data);
        }
    }
}
