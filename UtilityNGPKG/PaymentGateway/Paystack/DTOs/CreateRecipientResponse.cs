namespace UtilityNGPKG.PaymentGateway.Paystack.DTOs
{
    /// <summary>
    /// Represents the response returned when creating a transfer recipient.
    /// </summary>
    public class CreateRecipientResponse
    {
        /// <summary>
        /// Indicates whether the recipient creation was successful.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Contains the response message from Paystack.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The created recipient details.
        /// </summary>
        public RecipientData Data { get; set; }
    }
}
