> **Version:** v1.0.0

# CreateRecipientResponse

The response returned directly by `CreateRecipientAsync`. It confirms whether Paystack successfully registered the bank account as a transfer recipient and provides the `RecipientData` you'll need for future payouts.

## Properties

| Property  | Type            | Notes                                                                                                                           |
| --------- | --------------- | ------------------------------------------------------------------------------------------------------------------------------- |
| `Status`  | `bool`          | `true` if the recipient was created successfully                                                                                |
| `Message` | `string`        | Paystack's response message                                                                                                     |
| `Data`    | `RecipientData` | The created recipient's details, including the `RecipientCode` — see [TransferRecipientModels.md](./TransferRecipientModels.md) |
