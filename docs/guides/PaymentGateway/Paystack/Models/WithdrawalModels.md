> **Version:** v1.0.0

# WithdrawalRequest

The payload for `InitiateWithdrawalAsync`. Initiates a transfer from your Paystack balance to a registered recipient's bank account.

## Properties

| Property        | Type      | Required | Default | Notes                                                                                                                         |
| --------------- | --------- | -------- | ------- | ----------------------------------------------------------------------------------------------------------------------------- |
| `UserId`        | `Guid`    | Yes      | —       | Your internal user ID — for tracking purposes in your own database                                                            |
| `Amount`        | `decimal` | Yes      | —       | Amount in **Naira** (major currency). Converted to Kobo automatically before the API call.                                    |
| `RecipientCode` | `string`  | Yes      | —       | Obtained from a previous `CreateRecipientAsync` call. You should store this against the user's bank account in your database. |
| `Reason`        | `string`  | No       | —       | A note describing the transfer, visible on the Paystack dashboard (e.g. `"Weekly earnings payout"`)                           |
| `Currency`      | `string`  | No       | `"NGN"` | Currency code                                                                                                                 |

---

# WithdrawalResponse

Returned immediately after initiating a withdrawal. The transfer is usually still processing at this point — monitor its final outcome via the `transfer.success` or `transfer.failed` Paystack webhook events.

## Properties

| Property  | Type              | Notes                                                              |
| --------- | ----------------- | ------------------------------------------------------------------ |
| `Status`  | `bool`            | Whether the withdrawal request was accepted and queued by Paystack |
| `Message` | `string`          | Paystack's response message                                        |
| `Data`    | `WithdrawalData?` | Details of the created transfer                                    |

## WithdrawalData (Inside Data)

| Property       | Type             | Notes                                                                                                                                 |
| -------------- | ---------------- | ------------------------------------------------------------------------------------------------------------------------------------- |
| `TransferCode` | `string`         | Paystack's unique identifier for this transfer. Store this if you want to verify the transfer status later via `VerifyTransferAsync`. |
| `Reference`    | `string`         | The reference associated with this transfer                                                                                           |
| `Amount`       | `decimal`        | The transfer amount                                                                                                                   |
| `Status`       | `string`         | Initial status — usually `"pending"` or `"processing"` immediately after initiation                                                   |
| `Reason`       | `string`         | The reason you provided in the request                                                                                                |
| `Currency`     | `string`         | Currency of the transfer                                                                                                              |
| `CreatedAt`    | `DateTimeOffset` | When the transfer was created on Paystack                                                                                             |
| `UpdatedAt`    | `DateTimeOffset` | Last update timestamp                                                                                                                 |
| `Recipient`    | `long`           | Paystack's internal recipient ID                                                                                                      |
