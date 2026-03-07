using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.Mailer
{
    public class MailResponseDTO
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
        [Required]
        public string Subject { get; set; } = "Notification";

        /// <summary>
        /// Gets or sets the HTML or plain text body of the email.
        /// </summary>
        [Required]
        public string Body { get; set; }
    }
}
