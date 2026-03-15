> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../CHANGELOG.md)

# External API Integration

The External API Integration module provides a lightweight, reusable HTTP client wrapper for making outbound requests to third-party APIs. Instead of managing `HttpClient` lifecycle and JSON serialization repeatedly across your codebase, you inject `IApiIntegrationService` and make typed calls in a few lines.

This service is also used **internally** by the Paystack and KYC modules, so you may not need to call it directly at all — but it's fully available if you need to integrate with any other external API.

## How to Access

`IApiIntegrationService` is registered as a scoped service. It's included automatically via `AddUtilityNGPKG()`. Inject it where needed:

```csharp
public class MyThirdPartyService
{
    private readonly IApiIntegrationService _api;
    public MyThirdPartyService(IApiIntegrationService api)
    {
        _api = api;
    }
}
```

---

## Available Methods

### PostRequest

Makes an HTTP POST request with a JSON body to a given URL. You supply the headers (e.g., Authorization), the body object, and an `HttpClient` name. Returns the raw `HttpResponseMessage` so you can handle status codes and deserialize the response on your end.

- The body is serialized to JSON automatically using `System.Text.Json`.
- If the request fails or throws, the exception is logged and you'll receive a default/error response object rather than an uncaught exception.

```csharp
var headers = new Dictionary<string, string>
{
    { "Authorization", $"Bearer {apiKey}" }
};

var body = new { email = "user@example.com", amount = 5000 };

var response = await _api.PostRequest("https://api.example.com/charge", headers, body, "myClientName");

if (response.IsSuccessStatusCode)
{
    var content = await response.Content.ReadAsStringAsync();
    // deserialize content to your expected type
}
```

---

### GetRequest

Makes an HTTP GET request to a given URL with custom headers. Returns the raw `HttpResponseMessage`.

```csharp
var headers = new Dictionary<string, string>
{
    { "Authorization", $"Bearer {apiKey}" }
};

var response = await _api.GetRequest("https://api.example.com/users", headers, "myClientName");
```

---

### SerializeRequestBody

Serializes any object to a JSON `StringContent` suitable for use as an HTTP request body. This is useful if you need to build requests manually or if you're working with HttpClient directly alongside this service.

```csharp
var content = _api.SerializeRequestBody(new { name = "John" });
// Returns: StringContent with JSON and application/json media type
```

---

## Client Naming

The `clientName` parameter in both `PostRequest` and `GetRequest` maps to a named `HttpClient` registered with `IHttpClientFactory`. If you haven't registered a named client, passing any non-empty string will still work using the default client configuration. Named clients are useful when different APIs have different base addresses or timeouts — you'd configure those in `Program.cs` via `services.AddHttpClient("myClientName", ...)`.
