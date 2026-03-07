using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityNGPKG.PaymentGateway.Paystack.DTOs;

namespace UtilityNGPKG.PaymentGateway.Paystack
{
    public interface IPaystackService
    {
        /// <summary>
        /// Initializes a payment transaction with the Paystack gateway.
        /// This method prepares the payment and returns the authorization URL that the user should be redirected to.
        /// </summary>
        /// <param name="request">The payment initialization payload. <see cref="PaymentInitRequest"/></param>
        /// <param name="paystackSecret">The Paystack secret key associated with the environment or merchant.</param>
        /// <remarks>This is typically the first step in the checkout process.</remarks>
        /// <returns>A response containing the authorization URL and access code. <see cref="PaymentInitResponse"/></returns>
        Task<PaymentInitResponse> InitializePaymentAsync(PaymentInitRequest request, string paystackSecret);

        /// <summary>
        /// Verifies the status of a payment transaction using the reference generated during initialization.
        /// </summary>
        /// <param name="reference">The unique transaction reference provided to Paystack during payment initialization.</param>
        /// <param name="paystackSecret">The Paystack secret key for authenticating the request.</param>
        /// <remarks>You should call this method after receiving a callback or webhook from Paystack to confirm the payment was successful before giving value to the user.</remarks>
        /// <returns>A response containing the verified transaction details. <see cref="PaymentVerifyResponse"/></returns>
        Task<PaymentVerifyResponse> VerifyPaymentAsync(string reference, string paystackSecret);

        /// <summary>
        /// Verifies the status of a transfer or payout transaction using the transfer reference.
        /// </summary>
        /// <param name="reference">The unique transfer reference.</param>
        /// <param name="paystackSecret">The Paystack secret key for authenticating the request.</param>
        /// <remarks>Use this to check the final status of a transfer initiated via the Paystack Transfers API.</remarks>
        /// <returns>A response containing the verified transfer details. <see cref="PaymentVerifyResponse"/></returns>
        Task<PaymentVerifyResponse> VerifyTransferAsync(string reference, string paystackSecret);

        /// <summary>
        /// Initiates a transfer (withdrawal) request to a previously created transfer recipient on Paystack.
        /// </summary>
        /// <param name="request">The payload containing withdrawal details like amount and recipient code. <see cref="WithdrawalRequest"/></param>
        /// <param name="paystackSecret">The Paystack secret key.</param>
        /// <remarks>The amount specified will be transferred to the bank account associated with the recipient code.</remarks>
        /// <returns>A response indicating if the transfer was successfully queued. <see cref="WithdrawalResponse"/></returns>
        Task<WithdrawalResponse> InitiateWithdrawalAsync(WithdrawalRequest request, string paystackSecret);

        /// <summary>
        /// Creates a transfer recipient on Paystack. A recipient must be created before a transfer (payout) can be initiated.
        /// </summary>
        /// <param name="request">The payload containing the recipient's bank details and personal information. <see cref="CreateRecipientRequest"/></param>
        /// <param name="paystackSecret">The Paystack secret key.</param>
        /// <remarks>This method validates the account number and bank code, and returns a recipient code that will be used for transferring funds.</remarks>
        /// <returns>The generated transfer recipient data. <see cref="RecipientData"/></returns>
        Task<RecipientData> CreateRecipientAsync(CreateRecipientRequest request, string paystackSecret);

        /// <summary>
        /// Retrieves a list of supported banks in Nigeria from Paystack.
        /// </summary>
        /// <param name="paystackSecret">The Paystack secret key.</param>
        /// <remarks>This is useful for populating dropdowns when users need to select their bank to receive payouts.</remarks>
        /// <returns>A list of banks supported by Paystack. <see cref="BankData"/></returns>
        Task<List<BankData>> GetBanksAsync(string paystackSecret);

        /// <summary>
        /// Resolves a bank account number to verify its validity and return the account name.
        /// </summary>
        /// <param name="accountNumber">The 10-digit NUBAN account number to resolve.</param>
        /// <param name="bankCode">The Paystack bank code of the associated bank.</param>
        /// <param name="paystackSecret">The Paystack secret key.</param>
        /// <remarks>Use this method to confirm that the supplied bank details belong to the correct user before creating a recipient or transferring funds.</remarks>
        /// <returns>A response containing the resolved account details. <see cref="AccountResolveResponse"/></returns>
        Task<AccountResolveResponse> ResolveAccountNumber(string accountNumber, string bankCode, string paystackSecret);

        /// <summary>
        /// Generates a unique transaction reference using the provided identifier.
        /// </summary>
        /// <param name="uniqueIdentifier">The unique identifier (e.g., a Guid) for the local transaction recording.</param>
        /// <remarks>This method combines the supplied Guid, current timestamp, and a random string to ensure the generated reference is highly unique across Paystack transactions.</remarks>
        /// <returns>A uniquely generated standard reference string.</returns>
        string GenerateReference(Guid uniqueIdentifier);     
    }
}
