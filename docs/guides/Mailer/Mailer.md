> **Version:** v1.0.0 &nbsp;|&nbsp; [Changelog](../../CHANGELOG.md)

# Mailer

The Mailer module provides everything needed to send transactional emails from your application. It also ships a collection of pre-built HTML email templates for the most common notification scenarios — registration confirmation, login alerts, password resets, and account status updates.

## How to Access

`IMailService` is registered as a singleton and is available after adding `AddUtilityNGPKG()` to your program.cs.

```csharp
builder.Services.AddUtilityNGPKG();


Inject it into your service or controller:

```csharp
public class NotificationService
{
    private readonly IMailService _mailer;
    public NotificationService(IMailService mailer)
    {
        _mailer = mailer;
    }
}
```

---

## Sending Emails

There are two provider methods. Both accept a `MailRequestDTO` object and an API key for the selected provider. Both return a `ResponseDetail<bool>` so you can check success cleanly.

### SendMail_Sendgrid

Sends an email via Twilio SendGrid's REST API. Best suited for production environments.

- Under the hood, it converts the HTML body to plain text automatically (for email clients that don't render HTML).
- File attachments are supported — pass any `IFormFile` instances in the `Attachments` list on the request.
- The API key should be a valid SendGrid v3 key stored in your application's environment variables or secrets vault — never hardcoded.

```csharp
var mail = new MailRequestDTO
{
    Sender = "noreply@yourdomain.com",
    SenderName = "YourApp",
    Receiver = user.Email,
    ReceiverName = user.Name,
    Subject = "Welcome!",
    Body = "<p>Thanks for signing up.</p>"
};

var result = await _mailer.SendMail_Sendgrid(mail, _config["SendGrid:ApiKey"]);
```

---

### SendMail_MailTrap

Sends via MailTrap's sandbox API. Designed for development and testing — emails are intercepted and never delivered to real inboxes.

Works identically to `SendMail_Sendgrid` in terms of input. Just swap the API key for your MailTrap token.

```csharp
var result = await _mailer.SendMail_MailTrap(mail, _config["MailTrap:ApiKey"]);
```

---

## MailRequestDTO Reference

| Property       | Type               | Required | Default          | Notes                                              |
| -------------- | ------------------ | -------- | ---------------- | -------------------------------------------------- |
| `Sender`       | `string`           | Yes      | —                | Must be a verified sender address on your provider |
| `SenderName`   | `string`           | Yes      | —                | Display name shown in the email client             |
| `Receiver`     | `string`           | Yes      | —                | Recipient's email address                          |
| `ReceiverName` | `string`           | Yes      | `"Friend"`       | Recipient's display name                           |
| `Subject`      | `string`           | No       | `"Notification"` | Subject line                                       |
| `Body`         | `string`           | Yes      | —                | HTML or plain text email content                   |
| `Attachments`  | `List<IFormFile>?` | No       | `null`           | File attachments to include                        |

---

## Pre-Built Email Templates

The `NotificationMessages` class generates ready-to-use `MailResponseDTO` objects with styled HTML bodies. This means you don't have to write any HTML — just fill in the dynamic values and pass the result to `SendMail_*`.

> These templates use a dark-themed design with your sender name as the branding element. The template header color can be customized in a future version.

### RegistrationConfirmationMailNotification

Sends a welcome email containing a verification token. The token expires in 5 minutes — make sure you're generating and storing short-lived tokens on your end.

```csharp
var emailContent = NotificationMessages.RegistrationConfirmationMailNotification(
    receiver: user.Email,
    token: verificationToken,
    senderName: "YourApp",
    name: user.FirstName
);

var mailRequest = new MailRequestDTO
{
    Sender = "noreply@yourdomain.com",
    SenderName = "YourApp",
    Receiver = emailContent.Receiver,
    ReceiverName = emailContent.ReceiverName,
    Subject = emailContent.Subject,
    Body = emailContent.Body
};

await _mailer.SendMail_Sendgrid(mailRequest, apiKey);
```

---

### LoginNotification

Sends a security alert email when a login attempt is detected. Includes device info, IP address, country, and a verification token. The token expires in 10 minutes.

```csharp
var emailContent = NotificationMessages.LoginNotification(
    receiver: user.Email,
    name: user.FirstName,
    token: loginToken,
    device: "Chrome / Windows",
    ip: "102.34.67.88",
    country: "Nigeria"
);
```

---

### PasswordResetMailNotification

Sends a password reset email with a token. The token expires in 30 minutes and security tips are included inline in the email.

```csharp
var emailContent = NotificationMessages.PasswordResetMailNotification(
    receiver: user.Email,
    name: user.FirstName,
    token: resetToken,
    senderName: "YourApp"
);
```

---

### AccountVerificationSuccessNotification

A simple confirmation email telling the user their account has been verified.

---

### BlacklistNotification

Notifies a user that their account has been restricted. Accepts a `reason` parameter that's displayed prominently in the email body.

```csharp
var emailContent = NotificationMessages.BlacklistNotification(
    receiver: user.Email,
    name: user.FirstName,
    reason: "Suspicious activity detected.",
    senderName: "YourApp",
    senderEmail: "support@yourdomain.com"
);
```
