using System;

namespace UtilityNGPKG.PaymentGateway.Paystack.DTOs
{
    /// <summary>
    /// Represents the request payload for initiating a withdrawal (transfer) to a recipient.
    /// </summary>
    public class WithdrawalRequest
    {
        /// <summary>
        /// Used by the system to track the user making the withdrawal request.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The amount to be withdrawn. Should be provided in major currency (e.g., Naira).
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The Paystack generated recipient code to transfer the money to.
        /// </summary>
        public string RecipientCode { get; set; }

        /// <summary>
        /// A description or note for the transfer.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// The currency for the transfer. Defaults to 'NGN'.
        /// </summary>
        public string Currency { get; set; } = "NGN";
    }
}
