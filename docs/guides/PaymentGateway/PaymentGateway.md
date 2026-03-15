> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../../CHANGELOG.md)

# Payment Gateway

The Payment Gateway module wraps the full payment lifecycle — initializing transactions, verifying payments, managing bank accounts, creating payout recipients, initiating withdrawals, and validating incoming webhooks. It's accessible through the `IPaystackService` interface.

## Functionalites

### Paystack

`IPaystackService` is registered as a scoped service. It's set up automatically via `AddUtilityNGPKG()`. Inject it where you handle Paystack payment flows:

```csharp
public class PaymentService
{
    private readonly IPaystackService _paystack;
    public PaymentService(IPaystackService paystack)
    {
        _paystack = paystack;
    }
}
```

Your Paystack secret key is passed directly into each method call rather than being stored globally. This means you can easily switch between test and live keys at runtime.

> ⚠️ **Never hardcode your secret key in source code.** Store it in environment variables or your secrets vault.

See also: [IPaystackService](./Paystack/IPaystackService.md) for the full method reference.

---

### Taking Payments (Pay-ins)

The typical flow has two steps:

1. **Initialize** → Get an authorization URL and send the user there.
2. **Verify** → After the user pays (or you receive a webhook), verify the transaction.

### Paying Out (Transfers)

Before you can send money out:

1. **Resolve** the user's account number to confirm it's valid.
2. **Create a recipient** → You get back a `RecipientCode`.
3. **Initiate the withdrawal** using that code.
4. **Verify the transfer** once complete (or via webhook).

---

## Webhook Validation

If you've configured a Paystack webhook URL, every event Paystack sends (charge success, transfer success, etc.) should be validated before you trust the payload. Use `PaystackWebhookVerifier`:

```csharp
bool isValid = PaystackWebhookVerifier.IsValidPaystackSignature(
    body: rawRequestBodyString,
    signatureHeader: request.Headers["x-paystack-signature"],
    secret: _config["Paystack:SecretKey"]
);

if (!isValid)
    return Unauthorized(); // reject the request
```

The verifier computes an HMAC-SHA512 hash of the raw request body and compares it against Paystack's provided signature. Always validate before processing.

---

## Model Reference

### PaymentInitRequest — used when initializing a payment

| Property        | Type                         | Required | Default        | Notes                                                        |
| --------------- | ---------------------------- | -------- | -------------- | ------------------------------------------------------------ |
| `UserId`        | `Guid`                       | Yes      | —              | Your internal user ID                                        |
| `CustomerName`  | `string`                     | Yes      | —              | User's full name                                             |
| `Amount`        | `decimal`                    | Yes      | —              | In major currency (Naira) — converted to Kobo automatically  |
| `Email`         | `string`                     | Yes      | —              | Customer email for Paystack's records                        |
| `Currency`      | `string`                     | No       | `"NGN"`        |                                                              |
| `CallbackUrl`   | `string`                     | No       | —              | Where Paystack redirects the user after payment              |
| `TransactionId` | `Guid`                       | Yes      | —              | Your internal transaction ID                                 |
| `Reference`     | `string`                     | No       | Auto-generated | Leave blank to let the service generate one                  |
| `Channels`      | `List<string>`               | No       | All channels   | e.g. `["card", "bank", "ussd"]` to limit options             |
| `Metadata`      | `Dictionary<string, object>` | No       | Auto-filled    | Returns in webhook payload — useful for linking transactions |

### PaymentInitResponse

Returned after a successful initialization. The critical field is `Data.AuthorizationUrl` — redirect your user to this URL to complete checkout.

| Field                   | Notes                                                   |
| ----------------------- | ------------------------------------------------------- |
| `Status`                | Whether the initialization request was accepted         |
| `Data.AuthorizationUrl` | Redirect the user here to complete payment              |
| `Data.Reference`        | The transaction reference — store this for verification |
| `Data.AccessCode`       | Paystack internal code                                  |

### PaymentVerifyResponse

Returned after calling `VerifyPaymentAsync` or `VerifyTransferAsync`.

> **Important:** `Status` tells you whether the API call succeeded. Always check `Data.Status` (the string) to know the actual payment outcome. A successful API call can still return a `"failed"` or `"abandoned"` payment.

| Field                | Notes                                                         |
| -------------------- | ------------------------------------------------------------- |
| `Status`             | HTTP-level success of the verify call                         |
| `Data.Status`        | Payment result: `"success"`, `"failed"`, `"abandoned"`        |
| `Data.Amount`        | Confirmed amount in major currency (already divided by 100)   |
| `Data.Reference`     | For matching to your database records                         |
| `Data.Channel`       | How the user paid (`"card"`, `"bank"`, etc.)                  |
| `Data.Fees`          | Processing fees in major currency                             |
| `Data.Authorization` | Card details (for card payments) — reusable for subscriptions |
| `Data.Customer`      | Customer's Paystack ID and email                              |

### WithdrawalRequest — used when initiating a payout

| Property        | Type      | Required | Default | Notes                                   |
| --------------- | --------- | -------- | ------- | --------------------------------------- |
| `UserId`        | `Guid`    | Yes      | —       | Your internal tracking reference        |
| `Amount`        | `decimal` | Yes      | —       | In major currency (Naira)               |
| `RecipientCode` | `string`  | Yes      | —       | From `CreateRecipientAsync`             |
| `Reason`        | `string`  | No       | —       | Description shown on Paystack dashboard |
| `Currency`      | `string`  | No       | `"NGN"` |                                         |

### CreateRecipientRequest — used to register a transfer recipient

| Property        | Type     | Required | Default   | Notes                                 |
| --------------- | -------- | -------- | --------- | ------------------------------------- |
| `Name`          | `string` | Yes      | —         | Full legal name of the account holder |
| `AccountNumber` | `string` | Yes      | —         | 10-digit NUBAN number                 |
| `BankCode`      | `string` | Yes      | —         | From `GetBanksAsync`                  |
| `Type`          | `string` | No       | `"nuban"` | Leave as default for Nigerian banks   |
| `Currency`      | `string` | No       | `"NGN"`   |                                       |

### PaystackWebhookPayload

When Paystack fires a webhook, the body matches this structure. Deserialize the raw JSON body into `PaystackWebhookPayload` after validating the signature.

| Field            | Notes                                                                              |
| ---------------- | ---------------------------------------------------------------------------------- |
| `Event`          | What happened: `"charge.success"`, `"transfer.success"`, `"transfer.failed"`, etc. |
| `Data.Reference` | Match this against your database to identify the transaction                       |
| `Data.Status`    | `"success"`, `"failed"`, etc.                                                      |
| `Data.Amount`    | Amount in Kobo — divide by 100 for Naira                                           |
| `Data.Metadata`  | The custom data you attached during `InitializePaymentAsync`                       |
