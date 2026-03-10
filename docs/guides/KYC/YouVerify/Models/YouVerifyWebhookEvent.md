> **Version:** v1.0.0

# YouVerify Webhook Models

In addition to returning identity responses directly through the API, YouVerify can post background status updates to your webhook. These classes map the structure of those webhooks.

Deserialize the raw webhook payload into `YouVerifyWebhookEvent` after validating the signature.

---

## YouVerifyWebhookEvent

The root wrapper for YouVerify webhook payloads.

| Property | Type                       | Notes                 |
| -------- | -------------------------- | --------------------- |
| `Event`  | `string`                   | The event code        |
| `Data`   | `IdentityVerificationData` | The core webhook data |

---

## IdentityVerificationData

The payload section outlining exactly what was validated in the background.

| Property         | Type          | Notes                                                                          |
| ---------------- | ------------- | ------------------------------------------------------------------------------ |
| `FirstName`      | `string`      | The extracted or matched first name                                            |
| `LastName`       | `string`      | The extracted or matched last name                                             |
| `Gender`         | `string`      | The extracted gender                                                           |
| `IdNumber`       | `string`      | The document number verified                                                   |
| `Type`           | `string`      | Document type                                                                  |
| `DateOfBirth`    | `string`      | Format `YYYY-MM-DD`                                                            |
| `DataValidation` | `bool`        | High-level success flag indicating if the specific data submitted was verified |
| `Validations`    | `Validations` | Specific granular validation checks for the submitted fields                   |

---

## Validations & ValidationField

Details granular validations performed against individual properties. Primarily contains `DateOfBirth` validation presently.

| Property    | Type     | Notes                                                                               |
| ----------- | -------- | ----------------------------------------------------------------------------------- |
| `Validated` | `bool`   | Whether the particular property was a precise match to the physical identity record |
| `Value`     | `string` | The actual property value compared against the registry                             |
