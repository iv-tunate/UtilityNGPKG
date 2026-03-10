> **Version:** v1.0.0

# CreateRecipientRequest

The payload for `CreateRecipientAsync`. Used to register a bank account as an official Paystack transfer recipient so you can send money to it.

Run `ResolveAccountNumber` first to verify the account exists and confirm the account holder's name before creating a recipient.

## Properties

| Property        | Type     | Required | Default   | Notes                                                               |
| --------------- | -------- | -------- | --------- | ------------------------------------------------------------------- |
| `Name`          | `string` | Yes      | —         | The account holder's full legal name                                |
| `AccountNumber` | `string` | Yes      | —         | The 10-digit NUBAN bank account number                              |
| `BankCode`      | `string` | Yes      | —         | The 3-digit bank code from Paystack. Get this from `GetBanksAsync`. |
| `Type`          | `string` | No       | `"nuban"` | Recipient type — leave as default for Nigerian bank accounts        |
| `Currency`      | `string` | No       | `"NGN"`   | Currency code                                                       |

---

# RecipientData

Returned inside `CreateRecipientResponse.Data`. Contains the recipient's Paystack identity — the `RecipientCode` is what you'll use for all future withdrawals to this account.

## Properties

| Property        | Type     | Notes                                                                                          |
| --------------- | -------- | ---------------------------------------------------------------------------------------------- |
| `RecipientCode` | `string` | **Store this.** Required for every `InitiateWithdrawalAsync` call targeting this bank account. |
| `Type`          | `string` | Typically `"nuban"`                                                                            |
| `Name`          | `string` | Account holder name as confirmed by Paystack                                                   |
| `AccountNumber` | `string` | The 10-digit NUBAN number                                                                      |
| `BankCode`      | `string` | The bank's Paystack code                                                                       |
