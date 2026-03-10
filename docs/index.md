---
title: Home
---

# UtilityNGPKG — v1.0.0

Welcome to the official documentation for **UtilityNGPKG**, a collection of ready-to-use .NET service integrations for common backend tasks — from sending emails and verifying identities, to processing payments and generating JWT tokens.

## Getting Started

Add the package to your project and register all services in one line:

```csharp
builder.Services.AddUtilityNGPKG();
```

See the [Getting Started guide](guides/General/GeneralUtilities.md) for the full service registration table and notes on configuration.

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
