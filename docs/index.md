---
title: Home
---

# UtilityNGPKG — v1.0.0

Welcome to the official documentation for **UtilityNGPKG**, a collection of ready-to-use .NET service integrations for common backend tasks — from sending emails and verifying identities, to processing payments and generating JWT tokens.

## Getting Started — AddUtilityNGPKG

The ideal way to register all services available on this package is the `AddUtilityNGPKG` extension method. Call it once in your `Program.cs`:

```csharp
builder.Services.AddUtilityNGPKG();
```

This registers the following services automatically:

| Interface                  | Implementation            | Lifetime  |
| -------------------------- | ------------------------- | --------- |
| `IApiIntegrationService`   | `IntegrationService`      | Scoped    |
| `IPaystackService`         | `PaystackService`         | Scoped    |
| `IFileService`             | `FileService`             | Scoped    |
| `IKycService`              | `KycService`              | Scoped    |
| `IPaginationHelperFactory` | `PaginationHelperFactory` | Singleton |
| `ITokenBuilder`            | `TokenBuilder`            | Singleton |
| `IMailService`             | `MailService`             | Singleton |
| `ISanitizationService`     | `SanitizationService`     | Singleton |

It also calls `services.AddHttpClient()` internally, so you might not need to register that separately.
---

## Available Modules

| Module                                                       | Description                                                                       |
| ------------------------------------------------------------ | --------------------------------------------------------------------------------- |
| [External API Integration](guides/ExternalApiIntegration.md) | Generic HTTP client wrapper for outbound third-party API calls                    |
| [File Upload](guides/FilePost/FilePost.md)                   | Upload, download, sign, and delete files via cloud storage providers              |
| [KYC Integration](guides/KYC/KYC.md)                         | Identity verification and PDF text extraction                                     |
| [Mailer](guides/Mailer/Mailer.md)                            | Transactional emails with built-in HTML templates                                 |
| [Pagination](guides/Pagination/Pagination.md)                | Slice in-memory collections into pages with full nav metadata                     |
| [Payment Gateway](guides/PaymentGateway/PaymentGateway.md)   | Full payment lifecycle — pay-ins, payouts, bank resolution, webhooks              |
| [Sanitizer](guides/Sanitizer.md)                             | XSS-safe HTML and URL sanitization                                                |
| [Tokenomics](guides/Tokenomics.md)                           | JWT generation, password hashing (Argon2id), AES/RSA encryption, token generation |
| [Response Detail](guides/ResponseDetail.md)                  | Standardized API response wrapper used throughout the package                     |
