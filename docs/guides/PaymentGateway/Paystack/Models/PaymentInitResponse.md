> **Version:** v1.0.0

# PaymentInitResponse

Returned by `InitializePaymentAsync`. Contains everything you need to redirect the user to Paystack checkout and track the transaction on your end.

## Properties

| Property  | Type           | Notes                                                |
| --------- | -------------- | ---------------------------------------------------- |
| `Status`  | `bool`         | Whether Paystack accepted the initialization request |
| `Message` | `string`       | Descriptive message from Paystack                    |
| `Data`    | `PaymentData?` | Contains the URL and reference — see below           |

## PaymentData (Inside Data)

| Property           | Type     | Notes                                                                                          |
| ------------------ | -------- | ---------------------------------------------------------------------------------------------- |
| `AuthorizationUrl` | `string` | **The most important field.** Redirect your user's browser to this URL to complete checkout.   |
| `Reference`        | `string` | The transaction reference — store this in your database. You'll need it to verify the payment. |
| `AccessCode`       | `string` | Paystack's internal code for this session. Not typically needed in your application logic.     |
