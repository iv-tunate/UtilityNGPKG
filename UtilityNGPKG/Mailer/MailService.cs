using UtilityNGPKG.ResponseDetail;
using SendGrid;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
namespace UtilityNGPKG.Mailer
{
    /// <summary>
    /// Provides functionality for sending email messages using external providers such as SendGrid and MailTrap.
    /// </summary>
    /// <remarks>This service abstracts the process of composing and sending emails, including support for
    /// attachments and both HTML and plain text content. It is intended for internal use and requires valid API
    /// credentials for the respective email provider. Logging is performed for both successful and failed send
    /// attempts.</remarks>
    internal class MailService : IMailService
    {
        private readonly ILogger<MailService> logger;
        public MailService(ILogger<MailService> logger)
        {
            this.logger = logger;
        }
        public async Task<ResponseDetail<bool>> SendMail_Sendgrid(MailRequestDTO mail, string apiKey)
        {
            try
            {
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress(mail.Sender, mail.SenderName);
                var to = new EmailAddress(mail.Receiver, mail.ReceiverName);

                var msg = new SendGridMessage
                {
                    From = from,
                    Subject = mail.Subject,
                };
                msg.AddTo(to);

                var htmlBody = mail.Body;
                var plainTextBody = Regex.Replace(htmlBody, "<.*?>", string.Empty);

                msg.AddContent(MimeType.Text, plainTextBody);
                msg.AddContent(MimeType.Html, htmlBody);

                if (mail.Attachments != null && mail.Attachments.Count > 0)
                {
                    foreach (var file in mail.Attachments)
                    {
                        using var ms = new MemoryStream();
                        await file.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        var base64File = Convert.ToBase64String(fileBytes);

                        msg.AddAttachment(file.FileName, base64File);
                    }
                }

                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation($"Mail to {mail.Receiver} was successfully sent");
                    return ResponseDetail<bool>.Successful(true);
                }
                else
                {
                    var errorBody = await response.Body.ReadAsStringAsync();
                    logger.LogError($"Failed to send mail to {mail.Receiver}: {errorBody}");
                    return ResponseDetail<bool>.Failed(errorBody, (int)response.StatusCode, "SendGrid Error");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An exception occurred while sending mail to {mail?.Receiver}");
                return ResponseDetail<bool>.Failed(ex.Message, ex.HResult, "Caught Exception");
            }
        }

        public async Task<ResponseDetail<bool>> SendMail_MailTrap(MailRequestDTO mail, string apiKey)
        {
            try
            {
                var http = new HttpClient();
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                var payload = new
                {
                    from = new { email = mail.Sender, name = mail.SenderName },
                    to = new[] { new { email = mail.Receiver, name = mail.ReceiverName } },
                    subject = mail.Subject,
                    text = Regex.Replace(mail.Body, "<.*?>", string.Empty),
                    html = mail.Body,
                    attachments = mail.Attachments?.Select(a =>
                    {
                        using var ms = new MemoryStream();
                        a.CopyTo(ms);
                        return new
                        {
                            content = Convert.ToBase64String(ms.ToArray()),
                            filename = a.FileName,
                            type = a.ContentType,
                            disposition = "attachment"
                        };
                    }).ToList()
                };

                var response = await http.PostAsJsonAsync(
                    "https://send.api.mailtrap.io/api/send",
                    payload
                );

                if (response.IsSuccessStatusCode)
                {
                    logger.LogInformation($"Mail to {mail.Receiver} was successfully sent");
                    return ResponseDetail<bool>.Successful(true);
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    logger.LogError($"Failed to send mail to {mail.Receiver}: {errorBody}");
                    return ResponseDetail<bool>.Failed(errorBody, (int)response.StatusCode, "SendGrid Error");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An exception occurred while sending mail to {mail?.Receiver}");
                return ResponseDetail<bool>.Failed(ex.Message, ex.HResult, "Caught Exception");
            }
        }
    }
}
