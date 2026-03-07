using System.Collections.Generic;

namespace UtilityNGPKG.PaymentGateway.Paystack.DTOs
{
    /// <summary>
    /// Represents the response returned when fetching supported banks from Paystack.
    /// </summary>
    public class BankInfoResponse
    {
        /// <summary>
        /// Indicates whether the bank fetch request was successful.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Contains the response message from Paystack.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The list of banks retrieved.
        /// </summary>
        public List<BankData> Data { get; set; }
    }

    /// <summary>
    /// Contains details of a single bank supported by Paystack.
    /// </summary>
    public class BankData
    {
        /// <summary>
        /// The name of the bank.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The bank code, required when creating transfer recipients or verifying account numbers.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The country where the bank operates (e.g., Nigeria).
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The currency supported by the bank (e.g., NGN).
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The type of bank integration (e.g., nuban).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The unique internal Paystack ID for the bank.
        /// </summary>
        public int Id { get; set; }
    }
}
