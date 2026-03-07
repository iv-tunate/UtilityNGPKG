using System;
using Newtonsoft.Json;

namespace UtilityNGPKG.PaymentGateway.Paystack.DTOs
{
    /// <summary>
    /// Contains details about the initiated withdrawal (transfer).
    /// </summary>
    public class WithdrawalData
    {
        /// <summary>
        /// The unique code identifying this transfer.
        /// </summary>
        [JsonProperty("transfer_code")]
        public string TransferCode { get; set; }

        /// <summary>
        /// The unique reference associated with the transfer.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// The amount transferred (in Kobo or major currency depending on instantiation).
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The status of the transfer (e.g. 'success', 'pending', 'failed').
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The ID of the recipient on Paystack.
        /// </summary>
        public long Recipient { get; set; }

        /// <summary>
        /// Date and time when the transfer was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Date and time when the transfer was last updated on Paystack.
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// The reason provided when initiating the withdrawal.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// The currency used for the transfer.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The unique Paystack transfer ID.
        /// </summary>
        public string Id { get; set; }
    }
}
