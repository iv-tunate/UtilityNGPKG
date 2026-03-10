# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - Initial Release

### Added

- **Payment Processing**: Full Paystack integration (Payment Initialization, Verification, Transfers, and Webhooks).
- **File Management**: Cloudinary integration for secure file uploads and retrieval.
- **Mailer**: Email notifications supporting SendGrid and MailTrap, with pre-built HTML templates.
- **Identity & KYC**: YouVerify integration for Nigerian identity document validation and PDF text extraction.
- **Tokenomics**: JWT generator and Argon2id password hashing utilities.
- **Sanitizer**: XSS-safe HTML and URL sanitization.
- **Pagination**: Generic pagination helpers for collections.
- **Validations**: Compiled Regex utilities for email, phone, name, and password validation.
- Standardized `ResponseDetail<T>` wrapper for consistent API responses.
- Fluent DI registration via `AddUtilityNGPKG()`.
