> **Version:** v1.0.0

# YouVerify Webhook Validation

The `WebhookVerifier` static class validates that incoming YouVerify webhook POST requests are genuine — i.e., they actually originated from YouVerify and haven't been tampered with in transit. It uses HMAC-SHA256 with your webhook signing key.

## How to Use

There are two overloads depending on whether you still have access to the `HttpRequest` object, or you've already read the body.

### With HttpRequest (in-request validation)

Call this inside your webhook controller action. The method reads the raw body from the request stream and re-positions it so your normal model binding still works after the call.

```csharp
[HttpPost("kyc/webhook")]
public async Task<IActionResult> HandleKycWebhook()
{
    bool isValid = WebhookVerifier.IsValidYouVerifySignature(
        Request,
        _config["YouVerify:WebhookSecret"]
    );

    if (!isValid)
        return Unauthorized();

    var payload = await Request.ReadFromJsonAsync<YouVerifyWebhookEvent>();
    // process the event...
    return Ok();
}
```

> **Important:** `EnableBuffering()` must be called before accessing `Request.Body` for re-reading. Add `app.Use(async (ctx, next) => { ctx.Request.EnableBuffering(); await next(); });` to your middleware pipeline.

---

### With raw body string (pre-read body)

Use this when you've already read the body as a string elsewhere in your pipeline:

```csharp
bool isValid = WebhookVerifier.IsValidYouVerifySignature(
    rawBody: bodyString,
    signatureHeader: request.Headers["x-youverify-signature"],
    signingKey: _config["YouVerify:WebhookSecret"]
);
```

The method strips the `sha256=` prefix from the signature header automatically when using the `HttpRequest` overload. With the string overload, pass just the signature value without the prefix.

## How It Works

1. Reads the raw request body as UTF-8 text.
2. Computes an HMAC-SHA256 hash of the body using your signing key.
3. Compares the computed hash against the `x-youverify-signature` header value using `CryptographicOperations.FixedTimeEquals` — this is safe against timing attacks.
4. Returns `true` only if everything matches exactly.
