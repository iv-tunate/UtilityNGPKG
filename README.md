# UtilityNGPKG

A comprehensive utility NuGet package that simplifies common application integrations. It provides ready-to-use services for Payment Gateways, File Uploads, Mail Sending, KYC Integration, Tokenomics/JWT handling, Pagination, Regex Validations, and Input Sanitization.

## Features

- **Payment Processing**: Integrated payment gateway services (currently supporting Paystack, with more providers planned) for payment initialization, verification, resolving accounts, and handling webhooks.
- **File Management**: Simplified file and image uploads using cloud storage providers (currently Cloudinary).
- **Mailing**: Email sending and notification services (currently supporting SendGrid and MailTrap) featuring templated HTML notification generation.
- **Identity & KYC**: Identity verification services (currently supporting YouVerify integration).
- **Security & Tokenomics**: Argon2 password hashing support, comprehensive JWT builder, HTML Sanitization via HtmlSanitizer.
- **Pagination & Utilities**: Out-of-the-box generic pagination helper, broad Regex extensions, and standardized response formatting.

## Installation

You can install this package via NuGet Package Manager or .NET CLI:

```bash
dotnet add package UtilityNGPKG
```

## Quick Start

Register the required services in your `Program.cs` or `Startup.cs`:

```csharp
using UtilityNGPKG;

builder.Services.AddUtilityNGPKG();
```

For detailed usage on each service, please view our [Documentation Site]((https://iv-tunate.github.io/UtilityNGPKG)) which contains step-by-step guides for all modules.

## Documentation

Full documentation is available in the `docs` folder or [here](https://iv-tunate.github.io/UtilityNGPKG).

- [External API Integration](docs/guides/ExternalApiIntegration.md)
- [File Uploads](docs/guides/FilePost/FilePost.md)
- [KYC Integration](docs/guides/KYC/KYC.md)
- [Mailer Configuration](docs/guides/Mailer/Mailer.md)
- [Payment Gateway](docs/guides/PaymentGateway/PaymentGateway.md)

## Contributing

We welcome contributions! Please refer to the [Contributing Guide](docs/contributing/contributing.md) for details on how to submit Pull Requests, report Issues, and build the project locally.

## Security

Please see our [Security Policy](SECURITY.md) for reporting vulnerabilities and guidelines on secure usage.

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.
