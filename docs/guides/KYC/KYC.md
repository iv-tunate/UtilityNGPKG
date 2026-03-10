> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../../CHANGELOG.md)

# KYC (Know Your Customer)

The KYC module helps you verify user identities through supported identity providers. It supports two verification flows — checking a document via the identity service, and extracting text directly from a PDF uploaded by the user using native PDF parsing (no external service required for that part).

## How to Access

`IKycService` is registered as a scoped service and is available after adding `AddUtilityNGPKG()` to your program.cs.

```csharp
public class OnboardingService
{
    private readonly IKycService _kyc;
    public OnboardingService(IKycService kyc)
    {
        _kyc = kyc;
    }
}
```

You'll need your YouVerify credentials to call the API endpoints. Keep them out of source code — store them in environment variables:

```csharp
var credentials = new YouVerifyCredentials
{
    Token = _config["YouVerify:Token"],
    BaseUrl = _config["YouVerify:BaseUrl"]
};
```

---

## Available Methods

### VerifyDocumentAsync

Submits a KYC verification request to YouVerify using document details provided by the user. Under the hood, it posts the data to the YouVerify identity verification endpoint and returns the full structured response.

What you pass in:

- The document details (`YouVerifyKycDto`) — including the document type, number, and optionally the user's date of birth and first/last name.
- The credentials for authenticating with YouVerify.

```csharp
var kycData = new YouVerifyKycDto
{
    DocumentType = DocumentType.NIN,
    DocumentNumber = "12345678901",
    DateOfBirth = "1995-03-14",
    FirstName = "John",
    LastName = "Doe"
};

var result = await _kyc.VerifyDocumentAsync(kycData, credentials);

if (result.IsSuccess && result.Data?.Status == "found")
{
    // identity confirmed
}
```

See the [Model Reference](#model-reference) below for the full breakdown of `YouVerifyKycDto` and what the response looks like.

---

### ExtractTextFromPdfAsync

Extracts raw text content from a PDF file provided as an `IFormFile`. No external API call is made — this runs entirely server-side using the PdfPig library.

This is useful when you need to parse user-uploaded documents (e.g., bank statements, certificates) and extract information for further processing.

```csharp
string extractedText = await _kyc.ExtractTextFromPdfAsync(uploadedPdfFile);
```

Returns the full extracted text as a single string. If the PDF has no extractable text (e.g., it's a scanned image), the result will be an empty string.

---

### ValidateWebhook (WebhookVerifier - YouVerify)

If you've set up a webhook endpoint to receive YouVerify callback notifications, use `WebhookVerifier` to validate that incoming requests genuinely came from YouVerify.

It uses HMAC-SHA512 — YouVerify signs the payload with your secret key and sends the signature in the `x-youverify-signature` header. You recompute the hash on your end and compare.

```csharp
bool isValid = WebhookVerifier.IsValidSignature(
    rawBody: requestBodyString,    // the raw JSON string from the request body
    signature: request.Headers["x-youverify-signature"],
    secret: _config["YouVerify:WebhookSecret"]
);

if (!isValid)
    return Unauthorized();
```

> Always validate webhooks before trusting the payload. Never skip this step.

---

## Model Reference

### YouVerifyKycDto

| Property         | Type                  | Required | Notes                            |
| ---------------- | --------------------- | -------- | -------------------------------- |
| `DocumentType`   | `DocumentType` (enum) | Yes      | See below                        |
| `DocumentNumber` | `string`              | Yes      | The ID number on the document    |
| `DateOfBirth`    | `string`              | No       | Format: `YYYY-MM-DD`             |
| `FirstName`      | `string`              | No       | Used for name-match verification |
| `LastName`       | `string`              | No       | Used for name-match verification |

### DocumentType Enum

| Value           | Description                        |
| --------------- | ---------------------------------- |
| `NIN`           | National Identity Number (Nigeria) |
| `BVN`           | Bank Verification Number           |
| `Passport`      | International Passport             |
| `DriverLicense` | Driver's Licence                   |

### YouVerifyCredentials

| Property  | Type     | Required | Notes                                            |
| --------- | -------- | -------- | ------------------------------------------------ |
| `Token`   | `string` | Yes      | Your YouVerify API token                         |
| `BaseUrl` | `string` | Yes      | The base API URL (from your YouVerify dashboard) |

### YouVerifyResponse

The response from `VerifyDocumentAsync` wraps the YouVerify API's reply. Key things to check:

- **`IsSuccess`** — Whether the API call itself succeeded (HTTP-level).
- **`Data.Status`** — The verification outcome from YouVerify. Will be `"found"` if the identity was successfully matched, or another value if not.
- **`Data.Data`** — The full identity data returned by YouVerify, including full name, date of birth, address, photo URL (if available), and more depending on the document type.

If something went wrong (network error, invalid credentials, etc.), `IsSuccess` will be `false` and you'll see details in the error message rather than getting an unhandled exception.
