using System;
using Newtonsoft.Json;

namespace UtilityNGPKG.PaymentGateway.Paystack.DTOs
{
    /// <summary>
    /// Represents the root object returned by Paystack when sending a webhook notification for an event.
    /// </summary>
    public class PaystackWebhookPayload
    {
        /// <summary>
        /// The event type triggered (e.g., 'charge.success', 'transfer.success', 'transfer.failed').
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; set; }

        /// <summary>
        /// The payload containing details of the event generated constraint. <see cref="PaystackPaymentRequest"/>
        /// </summary>
        [JsonProperty("data")]
        public PaystackPaymentRequest Data { get; set; }
    }

    /// <summary>
    /// Contains details specific to the payment or transfer event received through a webhook.
    /// </summary>
    public class PaystackPaymentRequest
    {
        /// <summary>
        /// The unique ID of the event record on Paystack.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Indication of the domain of the application: 'test' or 'live'.
        /// </summary>
        [JsonProperty("domain")]
        public string Domain { get; set; }

        /// <summary>
        /// The unique reference associated with the transaction.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// The amount of the transaction in major and minor currency combinations (e.g Kobo natively but typically expected uncoverted depending on context).
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// The currency used to make the payment.
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Applicable if this was an invoice.
        /// </summary>
        [JsonProperty("due_date")]
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Checks if an invoice was attached to the payment payload.
        /// </summary>
        [JsonProperty("has_invoice")]
        public bool HasInvoice { get; set; }

        /// <summary>
        /// The invoice number attached to this transaction if available.
        /// </summary>
        [JsonProperty("invoice_number")]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Standard description attached to the transaction.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Metadata attributes attached along with payment instantiation. <see cref="PaystackMetadata"/>
        /// </summary>
        [JsonProperty("metadata")]
        public PaystackMetadata Metadata { get; set; }

        /// <summary>
        /// Available code when receiving a webhook for payment requests.
        /// </summary>
        [JsonProperty("request_code")]
        public string RequestCode { get; set; }

        /// <summary>
        /// Indicates the status string (e.g. 'success').
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Specifies if the transaction was completely paid for.
        /// </summary>
        [JsonProperty("paid")]
        public bool Paid { get; set; }

        /// <summary>
        /// Expected timestamp when the webhook event's payment was resolved as paid.
        /// </summary>
        [JsonProperty("paid_at")]
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// Extracted generic offline reference used for cash deposits etc.
        /// </summary>
        [JsonProperty("offline_reference")]
        public string OfflineReference { get; set; }

        /// <summary>
        /// The customer internal Paystack ID.
        /// </summary>
        [JsonProperty("customer")]
        public long Customer { get; set; }

        /// <summary>
        /// Date when this transaction was created prior to success event trigger.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Metadata provided initially containing specific contextual information for your use.
    /// </summary>
    public class PaystackMetadata
    {
        /// <summary>
        /// Standard transaction reference string added again internally.
        /// </summary>
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// The UserId of the customer in your application.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// The customer's full name.
        /// </summary>
        [JsonProperty("customer_name")]
        public string CustomerName { get; set; }

        /// <summary>
        /// The email representation for the customer on your end.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Any internal ID associated with generating this transaction payload structure.
        /// </summary>
        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

    }

    /// <summary>
    /// Supplementary model to carry event notification specific payload parts if available.
    /// </summary>
    public class PaystackNotification
    {
        /// <summary>
        /// Time that standard notification channels triggered alert.
        /// </summary>
        [JsonProperty("sent_at")]
        public DateTime SentAt { get; set; }

        /// <summary>
        /// Event channel delivery medium.
        /// </summary>
        [JsonProperty("channel")]
        public string Channel { get; set; }
    }

}

