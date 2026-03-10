> **Version:** v1.0.0

# YouVerifyResponse

The full response object returned by `VerifyDocumentAsync`. It wraps the YouVerify API response with both HTTP-level success info and the actual identity data.

## Top-Level Properties

| Property    | Type             | Notes                                                                                                                                      |
| ----------- | ---------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
| `IsSuccess` | `bool`           | Whether the API call itself succeeded (network + HTTP-level). A `true` here does not mean the identity was verified — check `Data.Status`. |
| `Message`   | `string`         | High-level description of what happened                                                                                                    |
| `Data`      | `YouVerifyData?` | The identity verification result from YouVerify. Will be `null` if the API call failed entirely.                                           |

## YouVerifyData Properties

| Property | Type                     | Notes                                                                                                                                  |
| -------- | ------------------------ | -------------------------------------------------------------------------------------------------------------------------------------- |
| `Status` | `string`                 | The verification outcome. `"found"` means the identity was successfully matched. Any other value indicates failure or a partial match. |
| `Data`   | `YouVerifyIdentityData?` | Full identity details returned by YouVerify for the document                                                                           |

## YouVerifyIdentityData (Inside Data.Data)

The fields available depend on the document type, but common ones across most types include:

| Property        | Type      | Notes                                                                |
| --------------- | --------- | -------------------------------------------------------------------- |
| `FullName`      | `string`  | Legal full name as registered with the issuing authority             |
| `DateOfBirth`   | `string`  | Format: `YYYY-MM-DD`                                                 |
| `PhoneNumber`   | `string`  | Mobile number on file, if available                                  |
| `Gender`        | `string`  | As registered                                                        |
| `Photo`         | `string?` | Base64-encoded photo, if returned by YouVerify for the document type |
| `StateOfOrigin` | `string?` | Available for NIN lookups                                            |
| `Address`       | `string?` | Registered residential address, if available                         |

> Not all fields are populated for every document type. Always check for null before using identity data fields.
