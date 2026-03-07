using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UtilityNGPKG.ExternalApiIntegration;
using UtilityNGPKG.KYC.YouVerify;

namespace UtilityNGPKG.KYC
{
    /// <summary>
    /// Represents a service for performing Know Your Customer (KYC) operations, specifically for verifying the presence of document numbers within PDF documents. The KycService class provides methods to extract text from PDF files and to verify if a specified document number (such as BVN, NIN, International Passport, or Driver's License) is present in the extracted text. This service is designed to assist in the KYC process by enabling the validation of identity documents against their corresponding numbers.
    /// </summary>
    public class KycService : IKycService
    {
        private readonly IApiIntegrationService apiService;
        private readonly ILogger<KycService> logger;
        public KycService(IApiIntegrationService apiIntegration, ILogger<KycService> logger)
        {
            this.apiService = apiIntegration;
            this.logger = logger;
        }
        /// <summary>
        /// Verifies whether the specified document number is present in the text extracted from the provided PDF document. The method takes a verification number, an uploaded PDF document, and the type of document as parameters. It uses regular expressions to search for the document number in the extracted text based on the expected format for the given document type. The method returns true if a match is found, indicating that the document number is present in the PDF, and false otherwise. If any exceptions occur during processing (such as issues with reading the PDF), the method will catch them and return false to indicate that verification was unsuccessful.
        /// </summary>
        /// <param name="verificationNumber">The verification number to check against the document. Cannot be null or empty.</param>
        /// <param name="document">The document file to be verified. Must be a valid, non-null file containing the document data.</param>
        ///<param name="documentType">The type of document being verified, which determines the expected format of the verification number. Must be a valid value from the DocumentType enum</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// document is successfully verified; otherwise, <see langword="false"/>.</returns>
        public static async Task<bool> VerifyDocumentNumber(string verificationNumber, IFormFile document, DocumentType documentType)
        {
            try
            {
                using var stream = document.OpenReadStream();
                var pdfText = ExtractTextFromPdf(stream);
                verificationNumber = verificationNumber?.Trim();
               
                var numberPattern = documentType switch
                {
                    DocumentType.BVN => @"\b\d{11}\b",
                    DocumentType.NIN => @"\b\d{11}\b",
                    DocumentType.InternationalPassport => @"\b[A-Z]\d{8}\b",
                    DocumentType.DriversLicense => @"\b[A-Z0-9\-]+\b",
                    _ => @"\b[\w\-]+\b"
                };

                var regex = new Regex(
                    numberPattern,
                    RegexOptions.IgnoreCase
                );

                var matches = regex.Matches(pdfText);

                var foundMatch = matches
                    .Cast<Match>()
                    .Any(m => string.Equals(m.Value.Trim(), verificationNumber,
                        StringComparison.OrdinalIgnoreCase));

                return foundMatch;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extracts all text content from a PDF document provided as a stream.
        /// </summary>
        /// <remarks>The method reads the entire PDF from the provided stream. The caller is responsible
        /// for managing the lifetime of the input stream. The extracted text preserves the page order but does not
        /// retain formatting or layout information.</remarks>
        /// <param name="stream">A readable stream containing the PDF document from which to extract text. The stream must be positioned at
        /// the beginning of the PDF content.</param>
        /// <returns>A string containing the concatenated text from all pages of the PDF document. Returns an empty string if the
        /// document contains no text.</returns>
        public static string ExtractTextFromPdf(Stream stream)
        {
            using var pdf = PdfDocument.Open(stream);

            var builder = new StringBuilder();

            foreach (Page page in pdf.GetPages())
            {
                builder.AppendLine(page.Text);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Defines an asynchronous method to verify the presence of a specified identification number (such as BVN, NIN, International Passport, or Driver's License) using the YouVerify API. The method constructs the appropriate API endpoint based on the type of document being verified and sends a POST request with the necessary details. It handles the response from the API, checking for success or failure, and returns a YouVerifyResponse object indicating the outcome of the verification process. In case of errors, it logs the error details and provides user-friendly messages based on the type of error encountered (e.g., temporary issues or problems with provided information).
        /// </summary>
        /// <param name="details"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public async Task<YouVerifyResponse> VerifyIdentificationNumberAsync(YouVerifyKycDto details, string baseUrl)
        {
            try
            {
                var url = details.Type switch
                {
                    DocumentType.BVN => $"{baseUrl}/v2/api/identity/ng/bvn",
                    DocumentType.NIN => $"{baseUrl}/v2/api/identity/ng/nin",
                    DocumentType.InternationalPassport => $"{baseUrl}/v2/api/identity/ng/passport",
                    DocumentType.DriversLicense => $"{baseUrl}/v2/api/identity/ng/drivers-license",
                    _ => throw new ArgumentOutOfRangeException(nameof(details.Type))
                };

                object body = details.Type switch
                {
                    DocumentType.BVN or DocumentType.NIN or DocumentType.DriversLicense => new
                    {
                        id = details.Id,
                        isSubjectConsent = true
                    },

                    DocumentType.InternationalPassport => new
                    {
                        id = details.Id,
                        isSubjectConsent = true,
                        lastName = details.LastName
                    },

                    _ => throw new ArgumentOutOfRangeException(nameof(details.Type))
                };

                var reqBody = apiService.SerializeReqBody(body);

                var request = await apiService.PostRequest(reqBody, url, null, "YouVerify");

                if (!request.IsSuccessStatusCode)
                {
                    var statusCode = (int)request.StatusCode;

                    var errorResponse = await request.Content.ReadAsStringAsync();

                    var error = JsonConvert.DeserializeObject<YouVerifyErrorResponse>(errorResponse)
                                ?? new YouVerifyErrorResponse { Message = "Unknown error" };

                    if (statusCode >= 500)
                    {
                        return new YouVerifyResponse
                        {
                            Success = false,
                            Status = "retry",
                            Message = "Temporary issue. Please retry after 10 minutes."
                        };
                    }

                    if (statusCode >= 400)
                    {
                        return new YouVerifyResponse
                        {
                            Success = false,
                            Status = "error",
                            Message = "There was a problem with the information provided."
                        };
                    }

                    logger.LogError($"Verification failed for {details.Id} with status code {statusCode}: {error.Message}");
                }

                return new YouVerifyResponse
                {
                    Success = true,
                    Status = "completed",
                    Message = "Operation initiated successfully"
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Verification process failed");

                return new YouVerifyResponse
                {
                    Success = false,
                    Status = "error",
                    Message = "An unexpected error occurred during verification."
                };
            }
        }
        public Task RetryVerification(YouVerifyKycDto payload, string baseUrl)
        {
            throw new NotImplementedException();
        }
    }
}
