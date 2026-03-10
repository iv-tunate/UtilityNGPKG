> **Version:** v1.0.0

# PaginatedResponse\<T\>

The object returned by `PaginationHelper<T>` after slicing a collection. It carries both the items for the current page and all the metadata a frontend needs to render navigation controls.

## Properties

| Property          | Type      | Notes                                                                            |
| ----------------- | --------- | -------------------------------------------------------------------------------- |
| `Data`            | `List<T>` | The items on this specific page                                                  |
| `CurrentPage`     | `int`     | The page number that was returned                                                |
| `PageSize`        | `int`     | How many items per page were requested                                           |
| `TotalItems`      | `int`     | Total count of items in the dataset (or filtered subset if a predicate was used) |
| `TotalPages`      | `int`     | Total number of pages at the current page size                                   |
| `HasNextPage`     | `bool`    | `true` if there is at least one more page after this one                         |
| `HasPreviousPage` | `bool`    | `true` if there is a page before this one                                        |
| `NextPage`        | `int?`    | The next page number, or `null` if this is the last page                         |
| `PreviousPage`    | `int?`    | The previous page number, or `null` if this is the first page                    |

## Example Response (JSON)

```json
{
  "data": [ ... ],
  "currentPage": 2,
  "pageSize": 10,
  "totalItems": 47,
  "totalPages": 5,
  "hasNextPage": true,
  "hasPreviousPage": true,
  "nextPage": 3,
  "previousPage": 1
}
```
