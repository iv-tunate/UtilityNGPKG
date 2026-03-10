> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../../../CHANGELOG.md)

# IPaystackService — Method Reference

This page covers all the methods available through `IPaystackService`. For an overview of how payment flows work end-to-end, see [PaymentGateway.md](../PaymentGateway.md).

---

## InitializePaymentAsync

Kicks off a payment transaction and returns a URL to redirect the user to. This is always the first step in any Pay-in flow.

Pass in a `PaymentInitRequest` with the user details, amount, and your callback URL. You'll also pass your Paystack secret key — use the correct one for your environment (test vs. live).

The `Amount` you provide should be in **Naira** (major currency). The service converts it to Kobo automatically before sending it to Paystack.

If you don't provide a `Reference`, one is generated for you using your `TransactionId`, the current timestamp, and a short random suffix. This ensures it's unique. You should store whatever reference ends up in the response — you'll need it to verify the payment.

```csharp
var request = new PaymentInitRequest
{
    UserId = currentUser.Id,
    CustomerName = currentUser.FullName,
    Email = currentUser.Email,
    Amount = 5000,       // NGN 5,000 — converted to 500000 kobo internally
    TransactionId = localTransaction.Id,
    CallbackUrl = "https://yourapp.com/payment/callback"
};

var result = await _paystack.InitializePaymentAsync(request, secretKey);

if (result.Status)
{
    // Redirect user to result.Data.AuthorizationUrl
    // Store result.Data.Reference for later verification
}
```

---

## VerifyPaymentAsync

Confirms whether a payment actually succeeded. Call this after the user returns from Paystack's checkout, or in your webhook handler.

Pass the `Reference` you stored during initialization. The response includes the payment status, confirmed amount, channel used, fees charged, and card/customer details if available.

> Always check `Data.Status` — not just the outer `Status` boolean. The outer one tells you the API call worked. `Data.Status` tells you what happened with the actual payment (`"success"`, `"failed"`, `"abandoned"`).

```csharp
var result = await _paystack.VerifyPaymentAsync(reference, secretKey);

if (result.Status && result.Data?.Status == "success")
{
    // payment confirmed — give value to the user
}
```

---

## GetBanksAsync

Returns a list of banks supported by Paystack. Use this to populate a bank selection dropdown in your UI when users want to set up a payout account. Each bank includes its `Name`, `Code`, and `Currency`.

The `Code` is what you'll use when resolving an account number or creating a recipient.

```csharp
var banks = await _paystack.GetBanksAsync(secretKey);
// banks is a List<BankData>
```

---

## ResolveAccountNumber

Looks up a bank account to confirm it exists and returns the registered account holder's name. Run this before creating a recipient — it's how you verify the user provided the correct bank details and that the account belongs to them.

Pass the 10-digit account number and the bank code (from `GetBanksAsync`).

```csharp
var result = await _paystack.ResolveAccountNumber("0123456789", "058", secretKey);

if (result.Status)
{
    string accountName = result.Data.AccountName;
    // show accountName to user for confirmation before proceeding
}
```

---

## CreateRecipientAsync

Registers a verified bank account as a transfer recipient on Paystack. You'll need to do this once per user before you can send them money. The response contains a `RecipientCode` — store this in your database alongside the user's bank details.

```csharp
var recipientRequest = new CreateRecipientRequest
{
    Name = "John Doe",
    AccountNumber = "0123456789",
    BankCode = "058"
};

var recipientData = await _paystack.CreateRecipientAsync(recipientRequest, secretKey);

// Store recipientData.RecipientCode — you'll use this every time you pay this user
```

---

## InitiateWithdrawalAsync

Sends money from your Paystack balance to a recipient. You'll need the `RecipientCode` from the step above. Provide the amount in Naira.

After a successful call, you'll receive a `WithdrawalResponse` with a `Status` and `Data.TransferCode`. The actual transfer might still be processing — monitor its final outcome via the Paystack webhook (`transfer.success` or `transfer.failed` events).

```csharp
var withdrawal = new WithdrawalRequest
{
    UserId = currentUser.Id,
    Amount = 10000,           // NGN 10,000
    RecipientCode = "RCP_xxxxxxxxxxxx",
    Reason = "Earnings payout"
};

var result = await _paystack.InitiateWithdrawalAsync(withdrawal, secretKey);
```

---

## VerifyTransferAsync

Confirms the final status of a previously initiated transfer using the transfer reference. The response structure is the same as `VerifyPaymentAsync`, but includes transfer-specific fields like `TransferCode`, `Reason`, and `Recipient` details.

```csharp
var result = await _paystack.VerifyTransferAsync(transferReference, secretKey);
```

---

## GenerateReference

Generates a unique transaction reference string. This is called automatically during `InitializePaymentAsync` if you don't provide one, but it's also available directly if you need to pre-generate a reference before creating the request.

The format is: `TXN_{guid}_{yyyyMMddHHmmss}_{randomSuffix}`

```csharp
string reference = _paystack.GenerateReference(yourTransactionGuid);
```
