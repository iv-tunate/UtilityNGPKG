using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UtilityNGPKG.PaymentGateway.Paystack.DTOs
{
    /// <summary>
    /// Represents the request payload for creating a transfer recipient on Paystack.
    /// </summary>
    public class CreateRecipientRequest
    {
        /// <summary>
        /// The recipient type. Defaults to 'nuban'.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = "nuban";

        /// <summary>
        /// The recipient's full name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The recipient's bank account number.
        /// </summary>
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// The Paystack bank code corresponding to the recipient's bank.
        /// </summary>
        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        /// <summary>
        /// The currency to be used. Defaults to 'NGN'.
        /// </summary>
        public string Currency { get; set; } = "NGN";
    }
}
