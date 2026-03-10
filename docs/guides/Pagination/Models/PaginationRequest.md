> **Version:** v1.0.0

# PaginationRequest

Represents the pagination parameters passed in by a client (usually from a query string). Use `Validate()` before passing values to the paginator to enforce safe bounds.

## Properties

| Property     | Type  | Required | Default | Constraints                                                        |
| ------------ | ----- | -------- | ------- | ------------------------------------------------------------------ |
| `PageNumber` | `int` | No       | `1`     | Minimum: 1. Values below 1 snap to 1 after `Validate()`.           |
| `PageSize`   | `int` | No       | `10`    | Minimum: 1, Maximum: 100. Capped automatically after `Validate()`. |

## Usage

```csharp
public IActionResult GetItems([FromQuery] PaginationRequest request)
{
    request.Validate(); // safe bounds enforced
    var result = helper.GetPagedResponse(request.PageNumber, request.PageSize);
    return Ok(result);
}
```
