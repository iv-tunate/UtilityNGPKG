> **Version:** v1.0.0

# YouVerifyKycDto

The request payload you send to `IKycService.VerifyDocumentAsync`. It contains the document details needed to look up an identity on YouVerify.

## Properties

| Property         | Type                  | Required | Notes                                                                                       |
| ---------------- | --------------------- | -------- | ------------------------------------------------------------------------------------------- |
| `DocumentType`   | `DocumentType` (enum) | Yes      | The type of ID being verified. See [DocumentType](./DocumentType.md).                       |
| `DocumentNumber` | `string`              | Yes      | The actual ID number printed on the document (e.g. NIN number, BVN number)                  |
| `DateOfBirth`    | `string`              | No       | Format: `YYYY-MM-DD`. Some document types (e.g. passport) use this for name-match accuracy. |
| `FirstName`      | `string`              | No       | Used for name-matching against what YouVerify has on file                                   |
| `LastName`       | `string`              | No       | Used for name-matching against what YouVerify has on file                                   |
