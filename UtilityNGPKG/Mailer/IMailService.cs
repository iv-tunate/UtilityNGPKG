using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityNGPKG.ResponseDetail;

namespace UtilityNGPKG.Mailer
{
    /// <summary>
    /// Defines the contract for sending email messages within the system. Mailtrap and Sendgrid are the two supported email delivery services, allowing for flexible email sending options based on the environment and requirements.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends an email using the specified mail request details via mailtrap... Works best for dev/staging environment.
        /// </summary>
        /// <param name="mail">The mail request containing recipient, subject, body, and other details.</param>
        /// <param name="apiKey">The client's Mailtrap apikey. It is required to authenticate and authorize your email sending requests via Mailtrap.</param>
        /// <returns>
        /// A <see cref="ResponseDetail{T}"/> containing a boolean value indicating whether the mail was sent successfully.
        /// </returns>
        Task<ResponseDetail<bool>> SendMail_MailTrap(MailRequestDTO mail, string apiKey);
        /// <summary>
        /// Sends an email using the specified mail request details via sendgrid. 
        /// </summary>
        /// <param name="mail">The mail request containing recipient, subject, body, and other details.</param>
        /// <param name="apiKey">The client's sendgrid apikey. It is required to authenticate and authorize your email sending requests via sendgrid.</param>
        /// <returns>
        /// A <see cref="ResponseDetail{T}"/> containing a boolean value indicating whether the mail was sent successfully.
        /// </returns>
        Task<ResponseDetail<bool>> SendMail_Sendgrid(MailRequestDTO mail, string apiKey);
    }
}
