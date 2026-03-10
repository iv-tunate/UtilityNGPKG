> **Version:** v1.0.0

# PaystackWebhookVerifier

The `PaystackWebhookVerifier` static class is fundamentally responsible for validating the integrity of payloads sent from Paystack in webhooks.

Paystack signs every webhook event it sends to your endpoint with your `SecretKey` using `HMAC-SHA512`. It is **essential** to always confirm this signature to guarantee the request authentically came from Paystack and hasn't been intercepted or forged by malicious actors.

## How to Use

`PaystackWebhookVerifier` exposes a single method: `IsValidPaystackSignature`.

```csharp
[HttpPost("paystack/webhook")]
public async Task<IActionResult> HandleWebhook()
{
    // Important: Read the body as a raw string! Do not use model binding.
    using var reader = new StreamReader(Request.Body);
    var rawBody = await reader.ReadToEndAsync();

    var signatureHeader = Request.Headers["x-paystack-signature"].ToString();
    var secretKey = _config["Paystack:SecretKey"];

    bool isValid = PaystackWebhookVerifier.IsValidPaystackSignature(rawBody, signatureHeader, secretKey);

    if (!isValid)
    {
        return Unauthorized("Invalid webhook signature.");
    }

    // Now it's safe to deserialize the rawBody into PaystackWebhookPayload
    var payload = JsonConvert.DeserializeObject<PaystackWebhookPayload>(rawBody);

    // Process the event...

    return Ok(); // Acknowledge receipt to Paystack
}
```

## IsValidPaystackSignature

Validates the payload against the provided signature using your secret key.

**Parameters:**
| Parameter | Type | Notes |
|---|---|---|
| `body` | `string` | The **raw serialized JSON string** exactly as received in the HTTP request body. |
| `signatureHeader` | `string` | The value extracted from the `x-paystack-signature` HTTP header sent by Paystack. |
| `secret` | `string` | Your Paystack Secret Key (preferably live, or test for sandbox testing). |

**Returns:** `bool` — `true` if authentic, `false` otherwise.
