> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../../CHANGELOG.md)

# Pagination

The Pagination module gives you a clean, reusable way to slice in-memory collections into pages without writing the maths yourself every time. You get back both the paginated data and all the metadata a frontend needs to render navigation controls.

## How to Access

There are two ways to use it:

**1. Via Dependency Injection (recommended)**

`IPaginationHelperFactory` is registered as a singleton. Inject it into your service, then call `Create()` with your collection and page size. This keeps your code testable and decoupled.

```csharp
public class UserService
{
    private readonly IPaginationHelperFactory _pagination;
    public UserService(IPaginationHelperFactory pagination)
    {
        _pagination = pagination;
    }

    public PaginatedResponse<User> GetUsers(List<User> allUsers, int page, int pageSize)
    {
        var helper = _pagination.Create(allUsers, pageSize);
        return helper.GetPagedResponse(page, pageSize);
    }
}
```

**2. Direct instantiation**

You can instantiate `PaginationHelper<T>` directly:

```csharp
var helper = new PaginationHelper<User>(allUsers, itemsPerPage: 10);
var result = helper.GetPagedResponse(pageNumber: 1, pageSize: 10);
```

> **Performance Deep Dive: `IEnumerable<T>` vs. `IQueryable<T>`**
>
> The `PaginationHelper<T>` in this package operates on `IEnumerable<T>`. This means it works with collections that are **already loaded into your server's memory**.
>
> **How it works here (`IEnumerable<T>`):**
> If you have 10,000 users in your database and you pass them to this helper to get page 1 (10 items), your application first downloads all 10,000 users from the database into RAM, and _then_ the helper slices out the 10 you asked for.
>
> - **Pros:** Great for small datasets, cached lists, or data originating from a 3rd-party API where memory footprint is negligible.
> - **Cons:** Terrible for large database tables. It wastes server memory, increases garbage collection pressure, and slows down database query times.
>
> **How to accurately manage large databases (`IQueryable<T>`):**
> When querying large SQL databases via Entity Framework (EF Core), you do **not** use `PaginationHelper<T>` to slice the data. Instead, slice the data natively on the SQL server using `.Skip().Take()`, and then manually construct a `PaginatedResponse<T>` to maintain a consistent API response format for your frontend:
>
> ```csharp
> var totalItems = await dbContext.Users.CountAsync();
>
> var pagedUsers = await dbContext.Users
>     .OrderBy(u => u.Id)
>     .Skip((pageNumber - 1) * pageSize)
>     .Take(pageSize)
>     .ToListAsync();
>
> // You can then wrap the result in PaginatedResponse<T> 
> // (HasNextPage, PreviousPage, etc. are calculated automatically)
> var response = new PaginatedResponse<User>
> {
>     Data = pagedUsers,
>     CurrentPage = pageNumber,
>     PageSize = pageSize,
>     TotalItems = totalItems,
>     TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
> };
>
> return Ok(response);
> ```
>
> **The Takeaway:** Use `PaginationHelper<T>` when you already have the full list in memory (e.g., parsing a CSV file, grouping an existing dataset, or caching). Use native EF Core `.Skip().Take()` combined with a manual `PaginatedResponse<T>` when querying databases directlycor large datasets to ensure peak efficiency while preserving standard API responses.

---

## Available Methods on PaginationHelper

### GetPagedResponse (basic)

Returns a page of items from the collection with full metadata — total items, total pages, current page, and whether there are previous/next pages.

If an out-of-range page number is requested (e.g., page -1 or page 999 when only 5 pages exist), it's clamped automatically. You won't get an exception — just the nearest valid page.

```csharp
var result = helper.GetPagedResponse(pageNumber: 2, pageSize: 10);
// result.Data — the items on this page
// result.TotalItems — full dataset count
// result.TotalPages — total number of pages
// result.HasNextPage / result.HasPreviousPage — for UI navigation
```

---

### GetPagedResponse (with filter)

Filters the collection using a predicate before paginating. `TotalItems` in the response reflects the count after filtering, not the full original collection size.

```csharp
var result = helper.GetPagedResponse(
    predicate: u => u.IsActive,
    pageNumber: 1,
    pageSize: 10
);
```

---

### GetPagedResponse (with sort)

Sorts the collection by a given key, then paginates. Pass `descending: true` to reverse the order (newest first, Z-A, etc.).

```csharp
var result = helper.GetPagedResponse(
    keySelector: u => u.CreatedAt,
    pageNumber: 1,
    pageSize: 10,
    descending: true // newest users first
);
```

---

## PaginationRequest

If you're accepting pagination parameters from an API request, bind them into a `PaginationRequest` and call `Validate()` before use. It enforces safe boundaries — any `PageNumber` below 1 snaps to 1, any `PageSize` above 100 is capped at 100.

```csharp
public IActionResult GetUsers([FromQuery] PaginationRequest req)
{
    req.Validate(); // enforces minimum/maximum bounds
    var result = helper.GetPagedResponse(req.PageNumber, req.PageSize);
    return Ok(result);
}
```

| Property     | Default | Constraints              |
| ------------ | ------- | ------------------------ |
| `PageNumber` | `1`     | Minimum: 1               |
| `PageSize`   | `10`    | Minimum: 1, Maximum: 100 |

---

## PaginatedResponse Properties

| Property          | Type      | Description                                          |
| ----------------- | --------- | ---------------------------------------------------- |
| `Data`            | `List<T>` | Items on the current page                            |
| `CurrentPage`     | `int`     | The page number returned                             |
| `PageSize`        | `int`     | Items per page                                       |
| `TotalItems`      | `int`     | Total dataset size (post-filter if applicable)       |
| `TotalPages`      | `int`     | Total number of pages                                |
| `HasNextPage`     | `bool`    | `true` if there's a page after this one              |
| `HasPreviousPage` | `bool`    | `true` if there's a page before this one             |
| `NextPage`        | `int?`    | Next page number, or `null` if on the last page      |
| `PreviousPage`    | `int?`    | Previous page number, or `null` if on the first page |
