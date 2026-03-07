using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace UtilityNGPKG.KYC.YouVerify
{
    /// <summary>
    /// Represents a utility class for verifying the authenticity of webhook requests. The WebhookVerifier class provides methods to validate the signature of incoming webhook requests by comparing the computed hash of the request body with the signature provided in the request headers.
    /// This verification process ensures that the webhook requests are genuinely sent by the expected service provider and have not been tampered with during transmission. 
    /// The class includes methods that can be used in ASP.NET Core applications to validate webhook signatures in a secure and efficient manner.
    /// </summary>
    public static class WebhookVerifier
    {
        /// <summary>
        /// Represents a method that validates the signature of an incoming YouVerify webhook request by computing the HMAC SHA256 hash of the request body using a provided signing key and comparing it to the signature included in the request headers. 
        /// The method reads the raw body of the request, computes the hash, and performs a fixed-time comparison to ensure security against timing attacks. 
        /// It returns true if the computed signature matches the header signature, indicating that the request is valid and has not been tampered with, and false otherwise.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="signingKey"></param>
        /// <returns></returns>
        public static bool IsValidYouVerifySignature(HttpRequest request, string signingKey)
        {
            var headerSignature = request.Headers["x-youverify-signature"].ToString();
            if (string.IsNullOrEmpty(headerSignature) || !headerSignature.StartsWith("sha256="))
                return false;

            var reqSignature = headerSignature["sha256=".Length..];

            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var rawBody = reader.ReadToEnd();
            request.Body.Position = 0;

            var secretBytes = Encoding.UTF8.GetBytes(signingKey);
            var payloadBytes = Encoding.UTF8.GetBytes(rawBody);

            using var hmac = new HMACSHA256(secretBytes);
            var hashBytes = hmac.ComputeHash(payloadBytes);
            var computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedSignature),
                Encoding.UTF8.GetBytes(reqSignature));
        }

        /// <summary>
        /// Represents a method that validates the signature of an incoming YouVerify webhook request by computing the HMAC SHA256 hash of the raw request body using a provided signing key and comparing it to the signature included in the request headers. 
        /// This method is useful in scenarios where the raw body of the request has already been read or is available as a string, allowing for signature validation without needing to access the HttpRequest object directly. 
        /// It returns true if the computed signature matches the header signature, indicating that the request is valid and has not been tampered with, and false otherwise.
        /// </summary>
        /// <param name="rawBody"></param>
        /// <param name="signatureHeader"></param>
        /// <param name="signingKey"></param>
        /// <returns></returns>
        public static bool IsValidYouVerifySignature(string rawBody, string signatureHeader, string signingKey)
        {
            if (string.IsNullOrEmpty(signatureHeader))
                return false;

            var reqSignature = signatureHeader;

            var secretBytes = Encoding.UTF8.GetBytes(signingKey);
            var payloadBytes = Encoding.UTF8.GetBytes(rawBody);

            using var hmac = new HMACSHA256(secretBytes);
            var hashBytes = hmac.ComputeHash(payloadBytes);
            var computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedSignature),
                Encoding.UTF8.GetBytes(reqSignature));
        }
    }
}
