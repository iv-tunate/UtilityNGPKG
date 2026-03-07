using System.Text.Json.Serialization;

namespace UtilityNGPKG.ResponseDetail
{
    /// <summary>
    /// A generic wrapper class used to standardize API responses across the application.
    /// </summary>
    /// <typeparam name="T">The type of the data being returned in the response.</typeparam>
    /// <remarks>
    /// This class provides static factory methods with a consistent structure for success and failure scenarios, 
    /// including metadata for pagination and detailed error reporting.
    /// If you want to manually set properties without using the static helper methods, create an instance via `new ResponseDetail<T>()`.
    /// </remarks>
    public class ResponseDetail<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets a descriptive message about the result of the operation.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the actual data returned by the operation. 
        /// Returns <see langword="null"/> if no data is present.
        /// </summary>
        public T? Data { get; set; } = default;

        /// <summary>
        /// Gets or sets the HTTP status code associated with the response.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets a detailed error message if the operation failed.
        /// This property is omitted from the JSON output if it is null.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Error { get; set; }

        /// <summary>
        /// Gets or sets the total number of items available in the dataset (for paginated responses).
        /// This property is omitted from the JSON output if it is null.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages available (for paginated responses).
        /// This property is omitted from the JSON output if it is null.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the current page number retrieved (for paginated responses).
        /// This property is omitted from the JSON output if it is null.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? PageNumber { get; set; }

        /// <summary>
        /// Creates a successful <see cref="ResponseDetail{T}"/> with the specified data and message.
        /// </summary>
        /// <param name="data">The data to return.</param>
        /// <param name="message">A descriptive message (defaults to "Operation completed successfully").</param>
        /// <param name="statusCode">The HTTP status code (defaults to 200).</param>
        /// <returns>A new <see cref="ResponseDetail{T}"/> instance representing a successful operation.</returns>
        public static ResponseDetail<T> Successful(T data, string message = "Operation completed successfully", int statusCode = 200)
        {
            return new ResponseDetail<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Creates a successful <see cref="ResponseDetail{T}"/> tailored for paginated data.
        /// </summary>
        /// <param name="data">The list of data items for the current page.</param>
        /// <param name="totalCount">The total number of items available across all pages.</param>
        /// <param name="totalPages">The total number of pages calculated.</param>
        /// <param name="pageNumber">The current page number retrieved.</param>
        /// <param name="message">A descriptive message (defaults to "Operation Successful").</param>
        /// <param name="statusCode">The HTTP status code (defaults to 200).</param>
        /// <returns>A new <see cref="ResponseDetail{T}"/> instance with pagination metadata.</returns>
        public static ResponseDetail<T> SuccessfulPaginatedResponse(T data, int totalCount, int totalPages, int pageNumber, string message = "Operation Successful", int statusCode = 200)
        {
            return new ResponseDetail<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                StatusCode = statusCode,
                TotalCount = totalCount,
                TotalPages = totalPages,
                PageNumber = pageNumber
            };
        }

        /// <summary>
        /// Creates a failed <see cref="ResponseDetail{T}"/> with the specified message and error details.
        /// </summary>
        /// <param name="message">A descriptive message (defaults to "Operation Failed").</param>
        /// <param name="statusCode">The HTTP status code (defaults to 400).</param>
        /// <param name="error">A detailed error message or stack trace.</param>
        /// <returns>A new <see cref="ResponseDetail{T}"/> instance representing a failed operation.</returns>
        public static ResponseDetail<T> Failed(string message = "Operation Failed", int statusCode = 400, string? error = null)
        {
            return new ResponseDetail<T>
            {
                IsSuccess = false,
                Message = message,
                StatusCode = statusCode,
                Error = error
            };
        }

        /// <summary>
        /// Creates a failed <see cref="ResponseDetail{T}"/> while still returning partial or associated data.
        /// </summary>
        /// <param name="data">The data to return alongside the failure message.</param>
        /// <param name="message">A descriptive message (defaults to "Operation Failed").</param>
        /// <param name="statusCode">The HTTP status code (defaults to 400).</param>
        /// <param name="error">A detailed error message or stack trace.</param>
        /// <returns>A new <see cref="ResponseDetail{T}"/> instance representing a failed operation with data.</returns>
        public static ResponseDetail<T> Failed(T data, string message = "Operation Failed", int statusCode = 400, string? error = null)
        {
            return new ResponseDetail<T>
            {
                IsSuccess = false,
                Message = message,
                StatusCode = statusCode,
                Error = error,
                Data = data
            };
        }
    }
}