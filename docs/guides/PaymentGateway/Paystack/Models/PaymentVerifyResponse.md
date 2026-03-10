> **Version:** v1.0.0

# PaymentVerifyResponse — Full Class Reference

`PaymentVerifyResponse` and its nested classes represent the complete structure returned by `VerifyPaymentAsync` and `VerifyTransferAsync`. This page documents every class in the hierarchy.

---

## PaymentVerifyResponse

The top-level response wrapper.

| Property  | Type                       | Notes                                                                                                                                  |
| --------- | -------------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| `Status`  | `bool`                     | Whether the verify API call succeeded at the HTTP level. A `true` here does not mean the payment was successful — check `Data.Status`. |
| `Message` | `string`                   | Paystack's response message                                                                                                            |
| `Data`    | `PaymentVerificationData?` | Full transaction details — `null` if the call failed                                                                                   |

---

## PaymentVerificationData

The core transaction data. This is where you'll find everything about what happened with the money.

| Property          | Type                     | Notes                                                             |
| ----------------- | ------------------------ | ----------------------------------------------------------------- |
| `Status`          | `string`                 | The payment outcome: `"success"`, `"failed"`, or `"abandoned"`    |
| `Reference`       | `string`                 | Use this to match the transaction to your database record         |
| `Amount`          | `decimal`                | Confirmed charge in major currency (already converted from Kobo)  |
| `GatewayResponse` | `string`                 | Human-readable result from the gateway, e.g. `"Approved"`         |
| `Domain`          | `string`                 | `"test"` or `"live"`                                              |
| `Channel`         | `string`                 | How the user paid: `"card"`, `"bank"`, `"ussd"`                   |
| `Currency`        | `string`                 | Currency code, e.g. `"NGN"`                                       |
| `Fees`            | `decimal`                | Processing fees in major currency                                 |
| `PaidAt`          | `DateTime?`              | When the payment was completed                                    |
| `CreatedAt`       | `DateTime?`              | When the transaction was initiated                                |
| `TransferCode`    | `string?`                | Only present on transfer verifications — the unique transfer code |
| `Reason`          | `string?`                | Transfer reason — only present on transfer verifications          |
| `Authorization`   | `AuthorizationData?`     | Card token details — only present if `Channel` is `"card"`        |
| `Customer`        | `CustomerData?`          | The Paystack customer record linked to this transaction           |
| `Recipient`       | `RecipientTransferData?` | Recipient details — only present on transfer verifications        |

---

## AuthorizationData

Card authorization details returned for card-channel payments. Store `AuthorizationCode` if you plan to support recurring charges.

| Property            | Type     | Notes                                                                                      |
| ------------------- | -------- | ------------------------------------------------------------------------------------------ |
| `AuthorizationCode` | `string` | Save this to initiate future charges on the same card without the user re-entering details |
| `Bin`               | `string` | First 6 digits of the card                                                                 |
| `Last4`             | `string` | Last 4 digits of the card                                                                  |
| `ExpMonth`          | `string` | Card expiry month                                                                          |
| `ExpYear`           | `string` | Card expiry year                                                                           |
| `CardType`          | `string` | e.g. `"visa"`, `"mastercard"`                                                              |
| `Bank`              | `string` | Issuing bank of the card                                                                   |
| `CountryCode`       | `string` | Country code of the card's origin                                                          |
| `Reusable`          | `bool`   | Whether this authorization can be re-used for future recurring payments                    |

---

## CustomerData

Basic Paystack customer record associated with the transaction.

| Property       | Type     | Notes                                                       |
| -------------- | -------- | ----------------------------------------------------------- |
| `Id`           | `long`   | Paystack's internal numeric ID for this customer            |
| `Email`        | `string` | Customer's email address                                    |
| `CustomerCode` | `string` | Paystack's unique customer code (e.g. `"CUS_xxxxxxxxxxxx"`) |

---

## RecipientTransferData

Populated only when verifying a transfer (payout). Contains details about who received the funds.

| Property        | Type                | Notes                            |
| --------------- | ------------------- | -------------------------------- |
| `Id`            | `long`              | Paystack's internal recipient ID |
| `Name`          | `string`            | Recipient's full name            |
| `RecipientCode` | `string`            | The Paystack recipient code      |
| `Details`       | `RecipientDetails?` | Bank account details — see below |

---

## RecipientDetails

Bank account information nested inside `RecipientTransferData.Details`.

| Property        | Type     | Notes                            |
| --------------- | -------- | -------------------------------- |
| `BankName`      | `string` | Name of the recipient's bank     |
| `AccountNumber` | `string` | Recipient's NUBAN account number |
