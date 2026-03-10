> **Version:** v1.0.0

# PaystackWebhookPayload — Full Class Reference

The complete structure of an incoming Paystack webhook POST body. Deserialize the raw request body into `PaystackWebhookPayload` after validating the signature.

---

## PaystackWebhookPayload

The root wrapper of every Paystack webhook event.

| Property | Type                     | Notes                                                                                                         |
| -------- | ------------------------ | ------------------------------------------------------------------------------------------------------------- |
| `Event`  | `string`                 | What happened on Paystack's end. Common values: `"charge.success"`, `"transfer.success"`, `"transfer.failed"` |
| `Data`   | `PaystackPaymentRequest` | The event payload — see below                                                                                 |

---

## PaystackPaymentRequest

The core event data. Whether it's a pay-in or a transfer webhook, the payload lands here.

| Property           | Type               | Notes                                                                           |
| ------------------ | ------------------ | ------------------------------------------------------------------------------- |
| `Id`               | `long`             | Paystack's internal event record ID                                             |
| `Domain`           | `string`           | `"test"` or `"live"`                                                            |
| `Reference`        | `string`           | **Match against your database** to identify which transaction this event is for |
| `Amount`           | `decimal`          | Amount in **Kobo** — divide by 100 for Naira                                    |
| `Currency`         | `string`           | e.g. `"NGN"`                                                                    |
| `Status`           | `string`           | Final outcome: `"success"`, `"failed"`, etc.                                    |
| `Paid`             | `bool`             | `true` if the transaction was fully paid                                        |
| `PaidAt`           | `DateTime?`        | Timestamp of when payment was resolved                                          |
| `CreatedAt`        | `DateTime`         | When the transaction was first created                                          |
| `Metadata`         | `PaystackMetadata` | Your custom data from the original initialization — see below                   |
| `Customer`         | `long`             | Paystack's internal customer ID linked to this event                            |
| `Description`      | `string?`          | Optional description attached to the transaction                                |
| `DueDate`          | `DateTime?`        | Applicable only if the transaction had an invoice                               |
| `HasInvoice`       | `bool`             | Whether an invoice was attached                                                 |
| `InvoiceNumber`    | `string?`          | Invoice number if present                                                       |
| `RequestCode`      | `string?`          | Available on payment request webhooks                                           |
| `OfflineReference` | `string?`          | For cash deposits or offline payment channels                                   |

---

## PaystackMetadata

The custom data you attached when calling `InitializePaymentAsync`. Everything you put in the `Metadata` dictionary during initialization comes back here in the webhook.

| Property        | Type     | Notes                                                                 |
| --------------- | -------- | --------------------------------------------------------------------- |
| `Reference`     | `string` | Your transaction reference, echoed back                               |
| `UserId`        | `string` | Your internal user ID (set automatically by `PaystackService`)        |
| `CustomerName`  | `string` | Customer name from the initialization payload                         |
| `Email`         | `string` | Customer email from the initialization payload                        |
| `TransactionId` | `string` | Your internal transaction ID (set automatically by `PaystackService`) |

> These fields match what `PaystackService.InitializePaymentAsync` places in the Metadata automatically. If you added any extra keys to the `Metadata` dictionary manually, they won't appear as typed properties here — you'll need to read them from the raw JSON body directly.

---

## PaystackNotification

A supplementary model representing notification channel metadata sometimes included in Paystack event payloads. Not required for standard payment flows.

| Property  | Type       | Notes                                                          |
| --------- | ---------- | -------------------------------------------------------------- |
| `SentAt`  | `DateTime` | When Paystack dispatched the notification through this channel |
| `Channel` | `string`   | The delivery channel used (e.g. email, SMS)                    |
