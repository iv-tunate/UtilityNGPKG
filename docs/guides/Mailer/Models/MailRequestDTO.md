> **Version:** v1.0.0

# MailRequestDTO

The payload you build when sending an email through `IMailService`. All fields are for addressing and content — credentials (your API key) are passed separately into the send method.

## Properties

| Property       | Type               | Required | Default          | Notes                                                                                                                            |
| -------------- | ------------------ | -------- | ---------------- | -------------------------------------------------------------------------------------------------------------------------------- |
| `Sender`       | `string`           | Yes      | —                | Must be a **verified sender address** from your SendGrid or MailTrap account. Unverified senders will cause the request to fail. |
| `SenderName`   | `string`           | Yes      | —                | The display name shown in the recipient's email client (e.g. "YourApp Support")                                                  |
| `Receiver`     | `string`           | Yes      | —                | The recipient's email address                                                                                                    |
| `ReceiverName` | `string`           | Yes      | `"Friend"`       | Recipient's display name. Defaults to `"Friend"` if not provided. Used in the email greeting when using built-in templates.      |
| `Subject`      | `string`           | No       | `"Notification"` | The email subject line                                                                                                           |
| `Body`         | `string`           | Yes      | —                | The email content — HTML or plain text. If you're using the `NotificationMessages` factory, this is generated for you.           |
| `Attachments`  | `List<IFormFile>?` | No       | `null`           | Optional file attachments. Supported by SendGrid.                                                                                |
