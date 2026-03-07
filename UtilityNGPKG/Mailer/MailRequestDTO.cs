using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UtilityNGPKG.Mailer
{
    /// <summary>
    /// Represents the details required to send an email message.
    /// </summary>
    public class MailRequestDTO
    {
        /// <summary>
        /// Gets or sets the email address of the receiver.
        /// </summary>
        [Required]
        public string Receiver { get; set; }

        /// <summary>
        /// Gets or sets the name address of the receiver.
        /// </summary>
        [Required]
        public string ReceiverName { get; set; } = "Friend";

        /// <summary>
        /// Gets or sets the subject line of the email.
        /// Defaults to "Notification" if not specified.
        /// </summary>
        public string Subject { get; set; } = "Notification";

        /// <summary>
        /// Gets or sets the HTML or plain text body of the email.
        /// </summary>
        [Required]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the sender associated with this instance.
        /// </summary>
        [Required]
        public string Sender { get; set; }
        /// <summary>
        /// Gets or sets the display name of the sender associated with the message.
        /// </summary>
        [Required]
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the optional list of files to be attached to the email.
        /// </summary>
        public List<IFormFile>? Attachments { get; set; }
    }
}