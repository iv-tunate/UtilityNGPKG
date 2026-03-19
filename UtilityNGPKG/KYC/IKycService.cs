using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityNGPKG.KYC.YouVerify;

namespace UtilityNGPKG.KYC
{
    /// <summary>
    /// Defines methods for performing Know Your Customer (KYC) verification operations, including identification number
    /// 
    /// </summary>
    /// <remarks>Implementations of this interface provide asynchronous methods for initiating and validating
    /// KYC checks. These methods are typically used to verify user identity and document authenticity as part of
    /// onboarding or compliance workflows.</remarks>
    public interface IKycService
    {
        /// <summary>
        /// Verifies the identification number asynchronously using the provided KYC details and credentials. 
        /// This method is typically used to initiate a KYC verification process with an external service, such as YouVerify, by sending the necessary information and credentials to perform the verification. The response from the external service will indicate whether the verification was successful and may include additional details about the verification status.
        /// 
        /// </summary>
        /// <param name="baseUrl">The base URL of the external KYC verification service (e.g., YouVerify) to which the verification request will be sent. This URL is used to construct the full endpoint for the verification process.</param>
        /// <param name="details">The KYC details containing information about the user or entity being verified. This may include personal information, identification numbers, and other relevant data required for the verification process.</param>
        /// <param name="apiKey">The API key or credentials required to authenticate the request with the external KYC verification service. This key is used to authorize the request and ensure that it is coming from a valid source.</param>
        /// <returns></returns>
        Task<YouVerifyResponse> VerifyIdentificationNumberAsync(YouVerifyKycDto details, string baseUrl, string apiKey);

        /// <summary>
        /// Retries the KYC verification process for a given set of KYC details. This method is typically used when a previous verification attempt has failed or needs to be re-initiated due to issues such as incorrect information, document problems, or other errors encountered during the initial verification process. By calling this method with the appropriate KYC details, the system can attempt to perform the verification again, potentially allowing for corrections or updates to the information provided in the previous attempt.
        /// 
        /// </summary>
        /// <param name="payload">The KYC details containing information about the user or entity for which the verification process needs to be retried. This may include updated personal information, identification numbers, or other relevant data that may have been corrected or modified since the previous verification attempt.</param>
        /// <returns></returns>
        Task RetryVerification(YouVerifyKycDto payload, string baseUrl);
    }
}
