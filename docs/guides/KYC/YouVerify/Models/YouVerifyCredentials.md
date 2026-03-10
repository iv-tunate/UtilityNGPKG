> **Version:** v1.0.0

# YouVerifyCredentials

Authentication configuration for the YouVerify API. Pass this into `VerifyDocumentAsync` when making KYC calls.

> **Keep these out of source code.** Store in environment variables or your secrets vault.

## Properties

| Property  | Type     | Required | Notes                                                                                                         |
| --------- | -------- | -------- | ------------------------------------------------------------------------------------------------------------- |
| `Token`   | `string` | Yes      | Your YouVerify API token obtained from the YouVerify dashboard                                                |
| `BaseUrl` | `string` | Yes      | The base API URL — e.g. `https://api.youverify.co/v2` for production. Use the sandbox URL during development. |
