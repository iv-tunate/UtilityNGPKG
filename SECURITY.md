# Security Policy

## Supported Versions

Currently, only the latest version of UtilityNGPKG is actively supported with security updates. We strongly recommend users always update to the latest patch or minor version.

| Version | Supported          |
| ------- | ------------------ |
| v1.x.x  | :white_check_mark: |

## Reporting a Vulnerability

Security is a primary priority for UtilityNGPKG. If you discover a security vulnerability within this package, please do **NOT** create a public GitHub issue.

Instead, send an email privately to the project maintainers. We aim to acknowledge your report within 48 hours and will provide you with updates as we investigate the issue.

### Security Best Practices

When using UtilityNGPKG, please adhere to these best practices:

1. **Never commit secrets**: Do not commit your SendGrid APIs, Cloudinary Keys, Paystack Secret Keys, or JWT signing keys to version control.
2. **Environment Variables**: Use `.env` files or secure vault systems (e.g., Azure Key Vault, AWS Secrets Manager) to load production configurations.
3. **Use the Sanitizer**: To prevent XSS, actively utilize the `ISanitizationService` provided by this package for all raw HTML user inputs.
