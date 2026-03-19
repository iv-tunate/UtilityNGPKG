using System;
using System.Threading.Tasks;
using UtilityNGPKG.PaymentGateway.Paystack.DTOs;
using UtilityNGPKG.Test.config;

namespace UtilityNGPKG.Test
{
    public class PaymentService_Test : BaseTestFeature
    {
        #region Paystack

        [Fact]
        public void GenerateReference_ReturnsNonEmptyString()
        {
            var transactionId = Guid.NewGuid();
            var reference = paystackService.GenerateReference(transactionId);

            Assert.NotNull(reference);
            Assert.NotEmpty(reference);
            Assert.Contains(transactionId.ToString(), reference);
        }

        [SkippableFact]
        public async Task InitializePayment_ReturnsAuthorizationUrl()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var secret = config.Value.PaystackTestSecret;

            var request = new PaymentInitRequest
            {
                UserId = Guid.NewGuid(),
                CustomerName = "Test User",
                Amount = 500,
                Email = "testuser@example.com",
                Currency = "NGN",
                CallbackUrl = "https://example.com/callback",
                TransactionId = Guid.NewGuid()
            };

            var result = await paystackService.InitializePaymentAsync(request, secret);

            Assert.NotNull(result);
            Assert.True(result.Status);
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data.AuthorizationUrl);
            Assert.NotEmpty(result.Data.Reference);
        }

        [SkippableFact]
        public async Task GetBanks_ReturnsNonEmptyList()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var secret = config.Value.PaystackTestSecret;
            var banks = await paystackService.GetBanksAsync(secret);

            Assert.NotNull(banks);
            Assert.NotEmpty(banks);
        }

        [SkippableFact]
        public async Task ResolveAccountNumber_ReturnsSuccess()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var secret = config.Value.PaystackTestSecret;

            var result = await paystackService.ResolveAccountNumber("0001234567", "058", secret);

            Assert.NotNull(result);
            Assert.True(result.Status);
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data.AccountName);
        }

        [SkippableFact]
        public async Task CreateRecipient_ReturnsRecipientCode()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var secret = config.Value.PaystackTestSecret;

            var request = new CreateRecipientRequest
            {
                Name = "Test User",
                AccountNumber = "0001234567",
                BankCode = "058",
                Currency = "NGN"
            };

            var result = await paystackService.CreateRecipientAsync(request, secret);

            Assert.NotNull(result);
            Assert.NotEmpty(result.RecipientCode);
        }

        [SkippableFact]
        public async Task VerifyPayment_ReturnsExpectedResponse()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var secret = config.Value.PaystackTestSecret;
            var result = await paystackService.VerifyPaymentAsync("dummy_ref_123", secret);

            Assert.NotNull(result);
            Assert.False(result.Status);
        }

        [SkippableFact]
        public async Task VerifyTransfer_ReturnsExpectedResponse()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var secret = config.Value.PaystackTestSecret;
            var result = await paystackService.VerifyTransferAsync("dummy_transfer_ref", secret);

            Assert.NotNull(result);
            Assert.False(result.Status);
        }

        [SkippableFact]
        public async Task InitiateWithdrawal_ReturnsExpectedResponse()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");

            var secret = config.Value.PaystackTestSecret;
            var request = new WithdrawalRequest
            {
                Amount = 1000m,
                RecipientCode = "RCP_dummy",
                Reason = "Test withdrawal"
            };

            var result = await paystackService.InitiateWithdrawalAsync(request, secret);

            Assert.NotNull(result);
            Assert.False(result.Status); 
        }

        #endregion
    }
}
