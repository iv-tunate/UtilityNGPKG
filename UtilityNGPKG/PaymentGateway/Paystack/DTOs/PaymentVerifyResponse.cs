using System;
using Newtonsoft.Json;

namespace UtilityNGPKG.PaymentGateway.Paystack.DTOs
{
    /// <summary>
    /// Represents the response payload received when verifying a Paystack transaction.
    /// </summary>
    public class PaymentVerifyResponse
    {
        /// <summary>
        /// Indicates whether the verification request itself was successful. Note: this doesn't mean the payment was successful. Check Data.Status for the payment status.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// A message describing the verification request response.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Contains the actual transaction verification details. <see cref="PaymentVerificationData"/>
        /// </summary>
        public PaymentVerificationData? Data { get; set; }
    }

    /// <summary>
    /// Contains the verification data attributes of the transaction.
    /// </summary>
    public class PaymentVerificationData
    {
        /// <summary>
        /// Type of domain: 'test' or 'live'.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// The status of the payment. Will ideally be 'success', 'failed', 'abandoned'.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The unique reference associated with the transaction.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// The amount of the transaction in major currency.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Paystack's gateway response indicating the result of the transaction processing.
        /// </summary>
        public string GatewayResponse { get; set; }

        /// <summary>
        /// Timestamp when the payment was completed.
        /// </summary>
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// Timestamp when the transaction was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The channel used by the user to make payment (e.g. card, bank).
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// The currency the payment was made in.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The fees deducted, in major currency.
        /// </summary>
        public decimal Fees { get; set; }

        /// <summary>
        /// Provides authorization information mainly if card payment was made. <see cref="AuthorizationData"/>
        /// </summary>
        public AuthorizationData? Authorization { get; set; }

        /// <summary>
        /// Basic customer information. <see cref="CustomerData"/>
        /// </summary>
        public CustomerData? Customer { get; set; }

        /// <summary>
        /// Provides recipient transfer data for transfer verification. <see cref="RecipientTransferData"/>
        /// </summary>
        public RecipientTransferData? Recipient { get; set; }

        /// <summary>
        /// The code identifying the transfer, if this was a transfer verification.
        /// </summary>
        [JsonProperty("transfer_code")]
        public string? TransferCode { get; set; }

        /// <summary>
        /// Transfer reason attached to the payload.
        /// </summary>
        public string? Reason { get; set; }
    }

    /// <summary>
    /// Details about the card or channel authorization used in the payment.
    /// </summary>
    public class AuthorizationData
    {
        /// <summary>
        /// The authorization code, which can be stored and used for future charges.
        /// </summary>
        public string AuthorizationCode { get; set; }
        /// <summary>
        /// First 6 digits of the card.
        /// </summary>
        public string Bin { get; set; }
        /// <summary>
        /// Last 4 digits of the card.
        /// </summary>
        public string Last4 { get; set; }
        /// <summary>
        /// Expiration month of the card.
        /// </summary>
        public string ExpMonth { get; set; }
        /// <summary>
        /// Expiration year of the card.
        /// </summary>
        public string ExpYear { get; set; }
        /// <summary>
        /// The type of the card (e.g. visa, mastercard).
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// Issuing bank of the card.
        /// </summary>
        public string Bank { get; set; }
        /// <summary>
        /// Country code of the card's origin.
        /// </summary>
        public string CountryCode { get; set; }
        /// <summary>
        /// Indicates if the authorization is reusable for future recurring payments.
        /// </summary>
        public bool Reusable { get; set; }
    }

    /// <summary>
    /// Customer details attached to the payment request.
    /// </summary>
    public class CustomerData
    {
        /// <summary>
        /// The Paystack internal ID for the customer.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// The email address of the customer.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The customer code used for referring to the customer on Paystack.
        /// </summary>
        public string CustomerCode { get; set; }
    }

    /// <summary>
    /// Basic receiver details attached to transfer verification.
    /// </summary>
    public class RecipientTransferData
    {
        /// <summary>
        /// The recipient ID.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Name of the recipient.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The recipient code generated by Paystack upon creation.
        /// </summary>
        public string RecipientCode { get; set; }
        /// <summary>
        /// Core bank connection details of the recipient. <see cref="RecipientDetails"/>
        /// </summary>
        public RecipientDetails? Details { get; set; }
    }

    /// <summary>
    /// Details containing the banking information of the recipient.
    /// </summary>
    public class RecipientDetails
    {
        /// <summary>
        /// The bank name attached to the recipient.
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// The account number of the recipient.
        /// </summary>
        public string AccountNumber { get; set; }
    }
}
