> **Version:** v1.0.0

# NotificationMessages

The `NotificationMessages` class provides static factory methods for generating styled, HTML-formatted email content. Instead of writing raw HTML strings in your services, you pass the required dynamic data to these methods, and they return a fully populated `MailResponseDTO`.

This ensures all emails sent from your application follow a consistent brand style and include necessary standard footers (like support contact info and automated message disclaimers).

## How to Use

Generate the content using one of the factory methods, then map it into a `MailRequestDTO` to send via `IMailService`.

```csharp
var content = NotificationMessages.RegistrationConfirmationMailNotification(
    receiver: "user@example.com",
    token: "ABCDEFGHI",
    senderName: "My Awesome App",
    name: "John Doe" // optional
);

var request = new MailRequestDTO
{
    Sender = "noreply@myapp.com",
    SenderName = content.SenderName,
    Receiver = content.Receiver,
    ReceiverName = content.ReceiverName,
    Subject = content.Subject,
    Body = content.Body
};

await _mailer.SendMail_Sendgrid(request, apiKey);
```

---

## Available Templates

### RegistrationConfirmationMailNotification

Sends a welcome email containing a verification token (e.g. for validating their email address). The template mentions the token expires in 5 minutes.

**Parameters:**

- `receiver` (string): Recipient's email address
- `token` (string): The verification code
- `senderName` (string): Your app/platform name
- `name` (string, optional): User's first name

---

### LoginNotification

Sends a security alert email when a login attempt is detected. Includes device information, IP address, geographical location, and a one-time passcode for verification. Mentions the token expires in 10 minutes.

**Parameters:**

- `receiver` (string)
- `name` (string)
- `token` (string): The 2FA or verification token
- `device` (string): E.g., `"Chrome / Windows"`
- `ip` (string): E.g., `"102.34.67.88"`
- `country` (string)

---

### EmailVerifiedNotification

A simple confirmation email telling the user their email address has been successfully verified.

**Parameters:**

- `receiver` (string)
- `name` (string)

---

### AccountVerificationSuccessNotification

Similar to above, but explicitly states their "account" was verified (useful if your onboarding has a manual admin approval or KYC step).

**Parameters:**

- `receiver` (string)
- `name` (string)

---

### PasswordResetNotification

Standard password reset request email containing a reset token. Mentions the token expires in 10 minutes.

**Parameters:**

- `receiver` (string)
- `name` (string)
- `token` (string)

---

### PasswordResetMailNotification

A slightly more detailed variant of the password reset email that includes standard security disclaimers (e.g., "Our support team will never ask for your password") and requires the `senderName` to personalize the copy. Mentions the token expires in 30 minutes.

**Parameters:**

- `receiver` (string)
- `name` (string)
- `token` (string)
- `senderName` (string)

---

### BlacklistNotification

Notifies a user that their account has been restricted. Automatically includes a styled, red-bordered callout box prominently displaying the ban reason.

**Parameters:**

- `receiver` (string)
- `name` (string)
- `reason` (string): The detailed reason for the restriction
- `senderName` (string)
- `senderEmail` (string): Customer support contact email explicitly provided to the banned user
