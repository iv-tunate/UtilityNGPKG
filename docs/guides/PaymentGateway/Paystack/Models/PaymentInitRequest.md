> **Version:** v1.0.0

# PaymentInitRequest

The payload you send to `InitializePaymentAsync` to kick off a payment session. After a successful call, you redirect the user to the `AuthorizationUrl` returned in the response.

## Properties

| Property        | Type                          | Required | Default        | Notes                                                                                                                     |
| --------------- | ----------------------------- | -------- | -------------- | ------------------------------------------------------------------------------------------------------------------------- |
| `UserId`        | `Guid`                        | Yes      | —              | Your internal user ID — used for tracking and included in the Metadata sent to Paystack                                   |
| `CustomerName`  | `string`                      | Yes      | —              | The paying user's full name                                                                                               |
| `Email`         | `string`                      | Yes      | —              | Customer's email address — Paystack uses this for receipts and customer records                                           |
| `Amount`        | `decimal`                     | Yes      | —              | The amount to charge in **Naira** (major currency). The service converts this to Kobo automatically (`Amount × 100`).     |
| `Currency`      | `string`                      | No       | `"NGN"`        | Currency code                                                                                                             |
| `CallbackUrl`   | `string`                      | No       | —              | Where Paystack sends the user after completing (or cancelling) checkout                                                   |
| `TransactionId` | `Guid`                        | Yes      | —              | Your internal transaction ID — used in reference generation and Metadata                                                  |
| `Reference`     | `string`                      | No       | Auto-generated | A unique reference for this transaction. If left blank, the service generates one in the format `TXN_{guid}_{timestamp}`. |
| `Channels`      | `List<string>?`               | No       | All channels   | Limit which payment methods Paystack shows the user (e.g. `["card"]`, `["bank", "ussd"]`)                                 |
| `Metadata`      | `Dictionary<string, object>?` | No       | Auto-filled    | Custom key-value pairs you want embedded in the transaction — they come back in your webhook payload                      |
