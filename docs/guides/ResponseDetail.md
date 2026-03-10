> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../../CHANGELOG.md)

# ResponseDetail

`ResponseDetail<T>` is a generic response wrapper used throughout the package — most service methods return it so you always have a consistent structure to handle, regardless of whether an operation succeeded or failed.

## What It Looks Like

```json
{
  "isSuccess": true,
  "message": "Operation completed successfully",
  "statusCode": 200,
  "data": { ... }
}
```

Or on failure:

```json
{
  "isSuccess": false,
  "message": "Token is invalid or expired",
  "statusCode": 400,
  "error": "Signature mismatch: ..."
}
```

The `error` and pagination fields (`totalCount`, `totalPages`, `pageNumber`) are omitted from JSON output entirely when they're null — so responses stay clean and minimal.

---

## Using It in Your Controllers

Rather than building response objects manually, use the built-in static factory methods:

### Successful

Returns a 200 success response with data.

```csharp
return Ok(ResponseDetail<UserDto>.Successful(userDto));

// With custom message and status code:
return Ok(ResponseDetail<UserDto>.Successful(userDto, "User created", 201));
```

### SuccessfulPaginatedResponse

Returns a 200 response for paginated results, including total counts and page info.

```csharp
return Ok(ResponseDetail<List<UserDto>>.SuccessfulPaginatedResponse(
    data: pagedUsers,
    totalCount: 150,
    totalPages: 15,
    pageNumber: 1
));
```

### Failed

Returns a failure response with no data. Default status code is 400.

```csharp
return BadRequest(ResponseDetail<object>.Failed("Invalid email format"));

// With a specific status code and error detail:
return StatusCode(500, ResponseDetail<object>.Failed("Server error", 500, ex.Message));
```

### Failed (with data)

Sometimes a failed operation still needs to return partial data — for example, returning a partially completed object alongside an error message.

```csharp
return BadRequest(ResponseDetail<UserDto>.Failed(partialResult, "Verification pending", 400));
```

---

## Property Reference

| Property     | Type      | Notes                                             |
| ------------ | --------- | ------------------------------------------------- |
| `IsSuccess`  | `bool`    | Whether the operation succeeded                   |
| `Message`    | `string`  | Human-readable result description                 |
| `Data`       | `T?`      | The returned data (null if failed with no data)   |
| `StatusCode` | `int`     | HTTP status code                                  |
| `Error`      | `string?` | Detailed error info — omitted from JSON when null |
| `TotalCount` | `int?`    | Total items available (pagination only)           |
| `TotalPages` | `int?`    | Total pages (pagination only)                     |
| `PageNumber` | `int?`    | Current page (pagination only)                    |
