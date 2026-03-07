namespace UtilityNGPKG.PaymentGateway.Paystack.DTOs
{
    /// <summary>
    /// Represents the response returned after initiating a withdrawal (transfer) request.
    /// </summary>
    public class WithdrawalResponse
    {
        /// <summary>
        /// Indicates if the withdrawal request was queued or processed successfully.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Contains the response message from Paystack.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The detailed data of the executed transfer.
        /// </summary>
        public WithdrawalData Data { get; set; }
    }
}
