> **Version:** v1.0.0

# AccountResolveResponse

Returned by `ResolveAccountNumber`. Tells you whether the bank account lookup succeeded and provides the account holder's confirmed name.

## Properties

| Property  | Type              | Notes                                                                                                          |
| --------- | ----------------- | -------------------------------------------------------------------------------------------------------------- |
| `Status`  | `bool`            | `true` if the account was found and resolved successfully                                                      |
| `Message` | `string`          | Descriptive message — useful for debugging when `Status` is `false` (e.g., `"Could not resolve account name"`) |
| `Data`    | `AccountDetails?` | The resolved account details — see below                                                                       |

---

# AccountDetails

The actual resolved bank account information nested inside `AccountResolveResponse.Data`.

## Properties

| Property        | Type     | Notes                                                                                                                                                              |
| --------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `AccountNumber` | `string` | The 10-digit NUBAN account number (echoed back from your input)                                                                                                    |
| `AccountName`   | `string` | The verified legal name of the account holder as registered with the bank. Show this to the user to confirm their details before proceeding to create a recipient. |
| `BankId`        | `string` | Paystack's internal identifier for the bank linked to this account                                                                                                 |
