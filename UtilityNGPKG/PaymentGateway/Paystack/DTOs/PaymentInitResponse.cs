namespace UtilityNGPKG.PaymentGateway.Paystack.DTOs
{
    /// <summary>
    /// Represents the response received after initializing a payment on Paystack.
    /// </summary>
    public class PaymentInitResponse
    {
        /// <summary>
        /// Indicates if the initialization request was successfully created.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// A descriptive message from the Paystack gateway.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Contains the authorization URL, reference, and access code. <see cref="PaymentData"/>
        /// </summary>
        public PaymentData Data { get; set; }
    }
}
