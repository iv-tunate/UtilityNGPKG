> **Version:** v1.0.0

# YouVerifyErrorResponse

When an API request to YouVerify fails at the HTTP level (e.g. Bad Request, Unprocessable Entity), this model structurally matches the JSON payload commonly returned in the response body.

## Properties

| Property     | Type     | Notes                                                   |
| ------------ | -------- | ------------------------------------------------------- |
| `Success`    | `bool`   | Will typically be `false` here                          |
| `StatusCode` | `int`    | The corresponding HTTP Response code (e.g., 400)        |
| `Message`    | `string` | Specific error detail indicating what failed validation |
| `Name`       | `string` | Internal error exception mapping or identifier type     |
