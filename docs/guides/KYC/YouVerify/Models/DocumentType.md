> **Version:** v1.0.0

# DocumentType

An enum used in `YouVerifyKycDto` to specify what kind of identity document is being verified.

## Values

| Value           | Description                                                                                   |
| --------------- | --------------------------------------------------------------------------------------------- |
| `NIN`           | National Identity Number — issued by Nigeria's National Identity Management Commission (NIMC) |
| `BVN`           | Bank Verification Number — issued by the Central Bank of Nigeria                              |
| `Passport`      | International Passport                                                                        |
| `DriverLicense` | Nigeria Driver's Licence issued by FRSC                                                       |

## Usage

```csharp
var kycRequest = new YouVerifyKycDto
{
    DocumentType = DocumentType.BVN,
    DocumentNumber = "12345678901"
};
```
