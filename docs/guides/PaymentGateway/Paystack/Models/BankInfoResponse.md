> **Version:** v1.0.0

# BankInfoResponse

Returned by `GetBanksAsync`. Contains the full list of Paystack-supported banks in Nigeria. Use this to populate a bank selection dropdown in your UI.

## Properties

| Property  | Type             | Notes                                          |
| --------- | ---------------- | ---------------------------------------------- |
| `Status`  | `bool`           | Whether the bank list was fetched successfully |
| `Message` | `string`         | Response message from Paystack                 |
| `Data`    | `List<BankData>` | The list of banks — see below                  |

---

# BankData

Represents a single bank in the `BankInfoResponse.Data` list.

## Properties

| Property   | Type     | Notes                                                                                                                |
| ---------- | -------- | -------------------------------------------------------------------------------------------------------------------- |
| `Id`       | `int`    | Paystack's internal numeric ID for this bank                                                                         |
| `Name`     | `string` | Full bank name displayed to users (e.g., `"Guaranty Trust Bank"`)                                                    |
| `Code`     | `string` | The 3-character bank code — pass this into `ResolveAccountNumber` and `CreateRecipientAsync` (e.g., `"058"` for GTB) |
| `Country`  | `string` | Country where the bank operates (e.g., `"Nigeria"`)                                                                  |
| `Currency` | `string` | Supported currency (e.g., `"NGN"`)                                                                                   |
| `Type`     | `string` | Integration type — typically `"nuban"` for Nigerian banks                                                            |
