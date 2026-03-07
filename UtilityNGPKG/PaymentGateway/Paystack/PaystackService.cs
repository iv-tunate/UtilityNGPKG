using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using PayStack.Net;
using UtilityNGPKG.ExternalApiIntegration;
using UtilityNGPKG.PaymentGateway.Paystack.DTOs;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UtilityNGPKG.PaymentGateway.Paystack
{
    internal class PaystackService : IPaystackService
    {
        private readonly ILogger logger;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api.paystack.co";
        private readonly IApiIntegrationService integrationService;
        private readonly string clientName = "paystack";

        public PaystackService(ILogger<PaystackService> logger, HttpClient httpClient, IApiIntegrationService integrationService)
        {
            this.logger = logger;
            _httpClient = httpClient;
            this.integrationService = integrationService;
        }
        public async Task<PaymentInitResponse> InitializePaymentAsync(PaymentInitRequest request, string paystackSecret)
        {
            var paymentInitResponse = new PaymentInitResponse();
            try
            {
                var paystack = new PayStackApi(paystackSecret);
                var reference = !string.IsNullOrEmpty(request.Reference) ? request.Reference : GenerateReference(request.TransactionId);

                var transactionRequest = new TransactionInitializeRequest
                {
                    AmountInKobo = (int)(request.Amount * 100),
                    Email = request.Email,
                    Currency = request.Currency,
                    Reference = reference,
                    CallbackUrl = request.CallbackUrl,
                    Channels = request.Channels?.ToArray(),
                    Metadata = request.Metadata != null && request.Metadata.Count > 0
                               ? JsonConvert.SerializeObject(request.Metadata)
                               : JsonConvert.SerializeObject(new
                               {
                                   request.UserId,
                                   request.CustomerName,
                                   request.Email,
                                   request.TransactionId,
                                   reference
                               })
                };

                var res = paystack.Transactions.Initialize(transactionRequest);
                logger.LogInformation("Paystack Initialize Payment Response: {@Response}", res);
                if (res.Status)
                {
                    return new PaymentInitResponse
                    {
                        Status = true,
                        Message = res.Message,
                        Data = new PaymentData
                        {
                            AuthorizationUrl = res.Data.AuthorizationUrl,
                            AccessCode = res.Data.AccessCode,
                            Reference = res.Data.Reference
                        }
                    };
                }
                else
                {
                    return paymentInitResponse;
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical("An exception was thrown initializing a payment on paystack:\n{ex}", ex);
                return paymentInitResponse;
            }
        }

        public async Task<PaymentVerifyResponse> VerifyPaymentAsync(string reference, string paystackSecret)
        {
            var paystack = new PayStackApi(paystackSecret);
            var result = new PaymentVerifyResponse();

            try
            {
                var response = paystack.Transactions.Verify(reference);

                if (!response.Status || response.Data == null)
                {
                    return result;
                }

                result.Status = response.Status;
                result.Message = response.Message;
                result.Data = new PaymentVerificationData
                {
                    Domain = response.Data.Domain,
                    Status = response.Data.Status,
                    Reference = response.Data.Reference,
                    Amount = response.Data.Amount / 100m,
                    GatewayResponse = response.Data.GatewayResponse,
                    CreatedAt = response.Data.TransactionDate,
                    Channel = response.Data.Channel,
                    Currency = response.Data.Currency,
                    Fees = Convert.ToDecimal(response.Data.Fees) / 100m,
                    Authorization = response.Data.Authorization == null ? null : new AuthorizationData
                    {
                        AuthorizationCode = response.Data.Authorization.AuthorizationCode,
                        Bin = response.Data.Authorization.Bin,
                        Last4 = response.Data.Authorization.Last4,
                        ExpMonth = response.Data.Authorization.ExpMonth,
                        ExpYear = response.Data.Authorization.ExpYear,
                        CardType = response.Data.Authorization.CardType,
                        Bank = response.Data.Authorization.Bank,
                        CountryCode = response.Data.Authorization.CountryCode,
                        Reusable = response.Data.Authorization.Reusable ?? false
                    },

                    Customer = response.Data.Customer == null ? null : new CustomerData
                    {
                        Id = response.Data.Customer.Id,
                        Email = response.Data.Customer.Email,
                        CustomerCode = response.Data.Customer.CustomerCode
                    }
                };

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("An exception was thrown while verifying a payment with paystack: {ex}", ex);
                return result;
            }
        }

        public async Task<PaymentVerifyResponse> VerifyTransferAsync(string reference, string paystackSecret)
        {
            var result = new PaymentVerifyResponse();

            try
            {
                var header = new Dictionary<string, string>()
                {
                    { "Bearer", paystackSecret }
                };
                var req = await integrationService.GetRequest($"{_baseUrl}/transfer/{reference}", header, clientName);
                if (!req.IsSuccessStatusCode)
                {
                    logger.LogError("Failed to verify transfer with Paystack. Reference: {Reference}, StatusCode: {StatusCode}", reference, req.StatusCode);
                    return result;
                }

                var content = await req.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<PaymentVerifyResponse>(content);

                if (response == null || !response.Status || response.Data == null)
                {
                    return result;
                }

                result.Status = response.Status;
                result.Message = response.Message;

                result.Data = new PaymentVerificationData
                {
                    Domain = response.Data.Domain,
                    Status = response.Data.Status,
                    Reference = response.Data.Reference,
                    Amount = response.Data.Amount / 100m,
                    GatewayResponse = response.Data.GatewayResponse,
                    CreatedAt = response.Data.CreatedAt,
                    Currency = response.Data.Currency,
                    Fees = response.Data.Fees / 100m,
                    Reason = response.Data.Reason,
                    TransferCode = response.Data.TransferCode,
                    Recipient = response.Data.Recipient == null ? null : new RecipientTransferData
                    {
                        Id = response.Data.Recipient.Id,
                        Name = response.Data.Recipient.Name,
                        RecipientCode = response.Data.Recipient.RecipientCode,
                        Details = response.Data.Recipient.Details == null ? null : new RecipientDetails
                        {
                            BankName = response.Data.Recipient.Details.BankName,
                            AccountNumber = response.Data.Recipient.Details.AccountNumber
                        }
                    }
                };

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("An exception was thrown while verifying a transfer transaction with paystack.\n{ex}", ex);
                return result;
            }
        }

        public async Task<RecipientData> CreateRecipientAsync(CreateRecipientRequest data, string paystackSecret)
        {
            try
            {
                var paystack = new PayStackApi(paystackSecret);
                var req = paystack.Post<ApiResponse<RecipientData>, CreateRecipientRequest>($"{_baseUrl}/transferrecipient", data);

                if (req.Status && req.Data != null)
                {
                    return req.Data;
                }
                return new RecipientData();
            }
            catch (Exception ex)
            {
                logger.LogError( "An exception was thrown while creating transfer recipient with Paystack.\n{ex}", ex);
                return new RecipientData();
            }
        }

        public async Task<WithdrawalResponse> InitiateWithdrawalAsync(WithdrawalRequest request, string paystackSecret)
        {
            var paystack = new PayStackApi(paystackSecret);
            var amount = (int)request.Amount * 100;

            //var reqbody = JsonConvert.SerializeObject(request);
            //var serialIzedBody = new StringContent(reqbody, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PostAsync($"{_baseUrl}/transfer", serialIzedBody);
            //request.Reference = GenerateReference(request.TransactionId);
            var req = paystack.Transfers.InitiateTransfer(amount, request.RecipientCode, reason: request.Reason);
            if (req.RawJson != null)
            {
                var responseContent = req.RawJson;
                return JsonConvert.DeserializeObject<WithdrawalResponse>(responseContent);
            }
            return new WithdrawalResponse();
        }

        public async Task<List<BankData>> GetBanksAsync(string paystackSecret)
        {
            try
            {
                var header = new Dictionary<string, string>()
                {
                    { "Bearer", paystackSecret }
                };
                var response = await integrationService.GetRequest($"{_baseUrl}/bank?country=nigeria", header, clientName );
                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError("Failed to fetch banks from Paystack {}", response.StatusCode.ToString());
                    return new List<BankData>();
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var bankResponse = JsonSerializer.Deserialize<BankInfoResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return bankResponse?.Data ?? new List<BankData>();
            }
            catch (Exception ex)
            {
                logger.LogError("An exception was thronw while Fetching banks from Paystack.\n {ex}", ex);
                return new List<BankData>();
            }
        }

        public async Task<AccountResolveResponse> ResolveAccountNumber(string accountNumber, string bankCode, string paystackSecret)
        {
            try
            {
                var header = new Dictionary<string, string>()
                {
                    {"Bearer", paystackSecret }
                };
                var response = await integrationService.GetRequest($"{_baseUrl}/bank/resolve?account_number={accountNumber}&bank_code={bankCode}", header, clientName);
                var responseContent = await response.Content.ReadAsStringAsync();
                var accountResolveResponse = JsonConvert.DeserializeObject<AccountResolveResponse>(responseContent);
                if (!response.IsSuccessStatusCode)
                {
                    logger.LogError($"There was an error with trying to resolve account number {accountNumber} with paystack");
                    logger.LogInformation($"{accountResolveResponse?.Message}");
                    return new AccountResolveResponse
                    {
                        Status = false,
                        Message = "Failed to resolve account number."
                    };
                }
                return accountResolveResponse;
            }
            catch (Exception ex)
            {
                logger.LogCritical("An exception was thrown while resolving account number with Paystack\n{ex}", ex);
                return new AccountResolveResponse
                {
                    Status = false,
                    Message = "An error occurred while resolving the account number."
                };
            }
        }
        public string GenerateReference(Guid transactionId)
        {
            return $"TXN_{transactionId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString()[..6]}";
        }
    }
}
