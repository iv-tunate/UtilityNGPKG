> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../CHANGELOG.md)

# Sanitizer

The Sanitizer module provides two focused utilities for cleaning user-supplied input before it touches your database or gets rendered in a browser — HTML sanitization and URL sanitization. This is your first line of defense against XSS injection attacks.

## How to Access

`ISanitizationService` is registered as a singleton. It's included automatically when you call `AddUtilityNGPKG()`. Inject it wherever you handle user input:

```csharp
public class ContentService
{
    private readonly ISanitizationService _sanitizer;
    public ContentService(ISanitizationService sanitizer)
    {
        _sanitizer = sanitizer;
    }
}
```

> **Note:** The underlying `HtmlSanitizer` library has known compatibility quirks with .NET 6. If you experience unexpected behavior, upgrading the target framework to .NET 8 is recommended.

---

## Available Methods

### SanitizeHtml

Strips all HTML tags and attributes from the input string, returning plain text. The sanitizer is intentionally configured in a **zero-allowlist mode** — it removes everything, not just known-bad tags. This is the safest approach for user-generated content.

- Returns an empty string if the input is null or whitespace.
- If the sanitizer itself throws (rare edge cases), it falls back to `WebUtility.HtmlEncode` so dangerous characters are escaped rather than passed through.

```csharp
string userInput = "<script>alert('xss')</script>Hello World";
string safe = _sanitizer.SanitizeHtml(userInput); // "Hello World"
```

---

### SanitizeUrl

Validates and returns a clean absolute URL, or an empty string if the input is invalid. It only allows `http://` and `https://` schemes — `javascript:`, `data:`, and any other scheme will be rejected outright.

- Returns an empty string if the input can't be parsed as an absolute URI.
- Returns an empty string if the scheme is not HTTP or HTTPS.

```csharp
string userUrl = "https://example.com/profile";
string safeUrl = _sanitizer.SanitizeUrl(userUrl); // "https://example.com/profile"

string malicious = "javascript:alert(1)";
string blocked = _sanitizer.SanitizeUrl(malicious); // ""
```
