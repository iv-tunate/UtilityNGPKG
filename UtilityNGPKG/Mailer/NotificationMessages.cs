using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace UtilityNGPKG.Mailer
{
    /// <summary>
    /// Provides methods and properties for constructing and managing email notification messages, including templates
    /// for registration, login, password reset, account status notifications, etc.
    /// </summary>
    /// <remarks>This class is intended for internal use to generate standardized email content for various
    /// user account events. It centralizes the creation of email subjects and HTML-formatted bodies to ensure
    /// consistency across notifications. The class supports personalization of messages and includes methods for
    /// generating emails with verification tokens, device information, and account status updates. All generated emails
    /// are intended to be sent through the application's email delivery system.</remarks>
    public class NotificationMessages
    {
        /// <summary>
        /// The subject of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The HTML body content of the email.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The recipient's email address.
        /// </summary>
        public string RecipientEmail { get; set; }

        /// <summary>
        /// The sender's email address.
        /// </summary>
        public string SenderEmail { get; set; }

        /// <summary>
        /// The sender's name... It's best if this is the name of the platform or service sending the email to avoid any confusion for the recipient. For example, if the email is being sent from a platform called "TechHub", then the sender name should be "TechHub" to clearly indicate to the recipient where the email is coming from.
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// The date and time the email was sent.
        /// </summary>
        public DateTime SentDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationMessages"/> class with the specified parameters.
        /// </summary>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <param name="recipientEmail">The recipient's email address.</param>
        /// <param name="senderEmail">The sender's email address.</param>
        /// <param name="senderName">The sender's name.</param>
        public NotificationMessages(string subject, string body, string recipientEmail, string senderEmail, string senderName)
        {
            Subject = subject;
            Body = body;
            RecipientEmail = recipientEmail;
            SenderEmail = senderEmail;
            SentDate = DateTime.Now;
            SenderName = senderName;
        }

        /// <summary>
        /// Generates the base email template layout.
        /// </summary>
        /// <param name="name">The name of the recipient to personalize the email.</param>
        /// <param name="bodyContent">The actual content to insert into the email body.</param>
        /// <param name="senderMail">The email address of the sender to include in the contact information section.</param>
        /// <param name="senderName">The display name of the sender to include in the email header and contact information.</param>
        /// <param name="extraDetail">Any additional details to include in the email header, such as a motto, subtitle or context </param>
        /// <returns>An HTML-formatted string representing the full email template.</returns>
        private static string BaseEmailTemplate(string name = "", string bodyContent = "", string senderMail = "", string senderName = "", string extraDetail="")
        {
            var mail = $@"
                <html>
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>{senderName} Platform</title>
                    <style>
                        body {{
                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                            background-color: #F4F4F4;
                            padding: 20px;
                            margin: 0;
                            color: #DADBDD;
                        }}
                        .sender{{
                            color: #DADBDD;    
                            font-weight: bold;
                            font-size:1rem;
                        }}
                        .email-container {{
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #111215;
                            padding: 30px;
                            border-radius: 8px;
                            box-shadow: 0 0 15px rgba(0, 0, 0, 0.2);
                            border: 1px solid #333740;
                        }}
                        .header {{
                            text-align: center;
                            margin-bottom: 30px;
                            padding-bottom: 20px;
                            border-bottom: 2px solid #BF0000;
                        }}
                        .logo {{
                            font-size: 24px;
                            font-weight: bold;
                            color: #BF0000;
                            margin-bottom: 5px;
                        }}
                        .subtitle {{
                            color: #858990;
                            font-size: 14px;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 12px 24px;
                            margin: 20px 0;
                            color: #FFFFFF !important;
                            background-color: #BF0000;
                            border: none;
                            border-radius: 5px;
                            text-decoration: none;
                            font-weight: 500;
                            font-size: 16px;
                        }}
                        .button:hover {{
                            background-color: #800000;
                        }}
                        p {{
                            line-height: 1.6;
                            margin-bottom: 15px;
                            color: #DADBDD;
                        }}
                        .footer {{
                            margin-top: 30px;
                            padding-top: 20px;
                            border-top: 1px solid #333740;
                            font-size: 14px;
                            color: #858990;
                        }}
                        .contact-info {{
                            margin-top: 20px;
                            font-size: 12px;
                            color: #696D77;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""email-container"">
                        <div class=""header"">
                            <div class=""logo"">{senderName}</div>
                            <div class=""subtitle"">{extraDetail}</div>
                        </div>

                        <h3>Hi{(!string.IsNullOrWhiteSpace(name) ? " " + name : "")}, </h3>
                        {bodyContent}

                        <div class=""footer"">
                            <p>Warm regards,<br><strong>The {senderName} Team</strong></p>
                            <div class=""contact-info"">
                                <p>This is an automated message from {senderName}.<br>
                                Need help? Reach out to our support team @ {senderMail}.</p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>";
            return mail;
        }

        /// <summary>
        /// Constructs a registration confirmation email with token for account verification.
        /// </summary>
        /// <param name="receiver">The email address of the recipient.</param>
        /// <param name="token">The account verification token.</param>
        /// <param name="senderName">The name of the sender or platform to personalize the email.</param>
        /// <param name="senderEmail">The sender's email address.</param>
        /// <param name="name">The recipient's name. It defaults to an empty string if no value is passed</param>
        /// <param name="attachments">Optional list of file attachments.</param>
        /// <returns>A populated <see cref="MailRequestDTO"/> ready to be sent.</returns>
        public static MailRequestDTO RegistrationConfirmationMailNotification(string receiver, string token, string senderName, string senderEmail, string name = " ", List<IFormFile>? attachments = null)
        {
            var subject = string.IsNullOrWhiteSpace(senderName) ? "WELCOME" : $"WELCOME TO {senderName.ToUpper()}";
            var body = $@"
            <br/>
            <p>Thank you for joining <b className=""sender"">{senderName}!!!</b></p>
            <p>Please verify your account with this token <b classNamw=""sender"">{token.ToUpper()}</b></p>
            <p>It expires in 5 mins";
            return new MailRequestDTO
            {
                ReceiverName = name,
                Receiver = receiver,
                Sender = senderEmail,
                SenderName = senderName,
                Subject = subject,
                Body = BaseEmailTemplate(name, body, senderEmail, senderName),
                Attachments = attachments
            };
        }

        /// <summary>
        /// Constructs a login notification email with device and location details.
        /// </summary>
        /// <param name="receiver">The email address of the recipient.</param>
        /// <param name="name">The name of the recipient.</param>
        /// <param name="token">The login verification token.</param>
        /// <param name="device">The device used to attempt login.</param>
        /// <param name="ip">The IP address of the login attempt.</param>
        /// <param name="country">The country location of the IP address.</param>
        /// <param name="senderName">The name of the sender or platform to personalize the email.</param>
        /// <param name="senderEmail">The sender's email address.</param>
        /// <param name="attachments">Optional list of file attachments.</param>
        /// <returns>A populated <see cref="MailRequestDTO"/> ready to be sent.</returns>
        public static MailRequestDTO LoginNotification(string receiver, string name, string token, string device, string ip, string country, string senderName, string senderEmail, List<IFormFile>? attachments = null)
        {
            var subject = "🔐 Login Notification";

            string body = $@"
                <p>Hello there {name},</p>

                <p>We detected a login attempt to your account.</p>

                <p>
                    <strong>Device:</strong> {device} <br/>
                    <strong>IP Address:</strong> {ip} <br/>
                    <strong>Location:</strong> {country} <br/>
                    <strong>Time:</strong> {DateTime.UtcNow:dddd, MMMM dd, yyyy 'at' hh:mm tt} (UTC)
                </p>

                <p>Please verify this login using the token below:</p>

                <h2 style='color:#800000'>{token}</h2>
                <p style='margin-top:-10px;'>This token will expire in <strong>10 minutes</strong>.</p>

                <br/>
                <p>If this wasn’t you, please ignore this email or contact our support team immediately.</p>

                <br/>
                <p style='color:gray; font-size:0.9em;'>This is an automated message. Please do not reply directly to this email.</p>
            ";

            return new MailRequestDTO
            {
                ReceiverName = name,
                Receiver = receiver,
                Sender = senderEmail,
                SenderName = senderName,
                Subject = subject,
                Body = BaseEmailTemplate(name, body, senderEmail, senderName),
                Attachments = attachments
            };
        }

        /// <summary>
        /// Constructs an email verification success notification.
        /// </summary>
        /// <param name="receiver">The email address of the recipient.</param>
        /// <param name="name">The name of the recipient.</param>
        /// <param name="senderName">The name of the sender or platform to personalize the email.</param>
        /// <param name="senderEmail">The sender's email address.</param>
        /// <param name="attachments">Optional list of file attachments.</param>
        /// <returns>A populated <see cref="MailRequestDTO"/> ready to be sent.</returns>
        public static MailRequestDTO EmailVerifiedNotification(string receiver, string name, string senderName, string senderEmail, List<IFormFile>? attachments = null)
        {
            var subject = "EMAIL VERIFIED";
            var body = $@"
            <br/>
            <p>Congratulations {name}, your email has been successfully verified.</p>";
            return new MailRequestDTO
            {
                ReceiverName = name,
                Receiver = receiver,
                Sender = senderEmail,
                SenderName = senderName,
                Subject = subject,
                Body = BaseEmailTemplate(name, body, senderEmail, senderName),
                Attachments = attachments
            };
        }

        /// <summary>
        /// Constructs a successful account verification notification email.
        /// </summary>
        /// <param name="receiver">The email address of the recipient.</param>
        /// <param name="name">The name of the recipient.</param>
        /// <param name="senderName">The name of the sender or platform to personalize the email.</param>
        /// <param name="senderEmail">The sender's email address.</param>
        /// <param name="attachments">Optional list of file attachments.</param>
        /// <returns>A populated <see cref="MailRequestDTO"/> ready to be sent.</returns>
        public static MailRequestDTO AccountVerificationSuccessNotification(string receiver, string name, string senderName, string senderEmail, List<IFormFile>? attachments = null)
        {
            var subject = "ACCOUNT VERIFICATION SUCCESSFUL";
            var body = $@"
            <br/>
            <p>Congratulations {name}, your account has been successfully verified.</p>";
            return new MailRequestDTO
            {
                ReceiverName = name,
                Receiver = receiver,
                Sender = senderEmail,
                SenderName = senderName,
                Subject = subject,
                Body = BaseEmailTemplate(name, body, senderEmail, senderName),
                Attachments = attachments
            };
        }

        /// <summary>
        /// Constructs a password reset notification email with a reset token.
        /// </summary>
        /// <param name="receiver">The email address of the recipient.</param>
        /// <param name="name">The name of the recipient.</param>
        /// <param name="token">The reset token.</param>
        /// <param name="senderName">The name of the sender or platform to personalize the email.</param>
        /// <param name="senderEmail">The sender's email address.</param>
        /// <param name="attachments">Optional list of file attachments.</param>
        /// <returns>A populated <see cref="MailRequestDTO"/> ready to be sent.</returns>
        public static MailRequestDTO PasswordResetNotification(string receiver, string name, string token, string senderName, string senderEmail, List<IFormFile>? attachments = null)
        {
            var subject = "PASSWORD RESET REQUEST";
            var body = $@"
            <br/>
            <p>Hi {name},</p>
            <p>We received a request to reset your password. Use the token below to reset it:</p>
            <h2 style='color:#800000'>{token}</h2>
            <p style='margin-top:-10px;'>This token will expire in <strong>10 minutes</strong>.</p>
            <br/>
            <p>If you did not request a password reset, please ignore this email or contact our support team.</p>";
            return new MailRequestDTO
            {
                ReceiverName = name,
                Receiver = receiver,
                Sender = senderEmail,
                SenderName = senderName,
                Subject = subject,
                Body = BaseEmailTemplate(name, body, senderEmail, senderName),
                Attachments = attachments
            };
        }

        /// <summary>
        /// Constructs a password reset email with token for password recovery (detailed variant).
        /// </summary>
        /// <param name="receiver">The email address of the recipient.</param>
        /// <param name="name">The name of the recipient.</param>
        /// <param name="token">The password reset token.</param>
        /// <param name="senderName">The name of the sender or platform to personalize the email.</param>
        /// <param name="senderEmail">The sender's email address.</param>
        /// <param name="attachments">Optional list of file attachments.</param>
        /// <returns>A populated <see cref="MailRequestDTO"/> ready to be sent.</returns>
        public static MailRequestDTO PasswordResetMailNotification(string receiver, string name, string token, string senderName, string senderEmail, List<IFormFile>? attachments = null)
        {
            var subject = "🔑 Password Reset Request";

            string body = $@"
                <p>Hi {name},</p>

                <p>We received a request to reset your password for your {senderName} account.</p>

                <p>Please use the token below to reset your password:</p>

                <h2 style='color:#800000; background-color:#f5f5f5; padding:15px; text-align:center; border-radius:5px;'>{token}</h2>
                <p style='margin-top:-10px; text-align:center;'>This token will expire in <strong>30 minutes</strong>.</p>

                <br/>
                <p><strong>Important Security Information:</strong></p>
                <ul>
                    <li>If you didn't request this password reset, please ignore this email</li>
                    <li>Never share this token with anyone</li>
                    <li>Our support team will never ask for your password or reset token</li>
                </ul>

                <br/>
                <p>If you continue to have issues accessing your account, please contact our support team.</p>

                <br/>
                <p style='color:gray; font-size:0.9em;'>This is an automated message. Please do not reply directly to this email.</p>
            ";

            return new MailRequestDTO   
            {
                Receiver = receiver,
                ReceiverName = name,
                Sender = senderEmail,
                SenderName = senderName,
                Subject = subject,
                Body = BaseEmailTemplate(name, body, senderEmail, senderName),
                Attachments = attachments
            };
        }

        /// <summary>
        /// Constructs a notification email for a blacklisted user.
        /// </summary>
        /// <param name="receiver">The email address of the recipient.</param>
        /// <param name="name">The name of the recipient.</param>
        /// <param name="reason">The reason for blacklisting.</param>
        /// <param name="senderName">The name of the sender or platform to personalize the email.</param>
        /// <param name="senderEmail">The email address of the sender to include in the contact information section.</param>
        /// <param name="attachments">Optional list of file attachments.</param>
        /// <returns>A populated <see cref="MailRequestDTO"/> ready to be sent.</returns>
        public static MailRequestDTO BlacklistNotification(string receiver, string name, string reason, string senderName, string senderEmail, List<IFormFile>? attachments = null)
        {
            var subject = $"⚠️ Account Restricted - {senderName} Platform";
            var body = $@"
            <br/>
            <p>Hi {name},</p>
            <p>Your account on {senderName} has been restricted (blacklisted). This means you will no longer be able to log in or access our services.</p>
            <div style='background-color: #fcecea; border-left: 4px solid #f44336; padding: 15px; margin: 20px 0;'>
                <p style='color: #d32f2f; font-weight: bold; margin-bottom: 5px;'>Reason for restriction:</p>
                <p style='color: #000000; margin: 0;'>{reason}</p>
            </div>
            <p>If you believe this is a mistake, please contact our support team at {senderEmail} for further clarification.</p>";

            return new MailRequestDTO
            {
                Receiver = receiver,
                ReceiverName = name,
                Sender = senderEmail,
                SenderName = senderName,
                Subject = subject,
                Body = BaseEmailTemplate(name, body, senderEmail, senderName),
                Attachments = attachments
            };
        }
    }
}
