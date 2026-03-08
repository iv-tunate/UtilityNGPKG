using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace UtilityNGPKG.Tokenomics
{
    /// <summary>
    /// Provides methods for encrypting and decrypting data using various cryptographic algorithms, as well as
    /// generating random numbers and tokens.
    /// </summary>
    /// <remarks>The TokenBuilder class supports multiple symmetric and asymmetric encryption algorithms,
    /// including AES, DES, HMACSHA256, and RSA. It also offers utility methods for generating random numbers and tokens
    /// suitable for use in authentication or security scenarios. All cryptographic operations are intended for use in
    /// security-sensitive contexts and should be used with appropriate key management practices. This class is not
    /// thread-safe.</remarks>
    internal class TokenBuilder : ITokenBuilder
    {
        private readonly ILogger<TokenBuilder> logger;
        public TokenBuilder(ILogger<TokenBuilder> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Verifies HMAC-SHA256 signature for data authenticity
        /// </summary>
        public (string error, bool isValid) VerifyHMACSHA256(string data, string expectedSignature, string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(expectedSignature) || string.IsNullOrWhiteSpace(key))
                    return ("Invalid input parameters", false);

                var (generatedSignature, success) = GenerateHMACSHA256(data, key);

                if (!success)
                    return ("Failed to generate signature for verification", false);

                var isValid = ConstantTimeComparison(expectedSignature, generatedSignature);

                if (!isValid)
                    return ("Signature verification failed", false);

                return (string.Empty, true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error verifying HMAC-SHA256 signature");
                return ($"Verification error: {ex.Message}", false);
            }
        }

        /// <summary>
        /// Encrypts plain text using RSA public key
        /// </summary>
        public (byte[] encryptedData, bool success) EncryptRSA(string plainText, RSAParameters publicKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plainText))
                    return (Array.Empty<byte>(), false);

                using (var rsa = RSA.Create())
                {
                    rsa.ImportParameters(publicKey);
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    var encryptedBytes = rsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA256);
                    return (encryptedBytes, true);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error encrypting RSA plain text");
                return (Array.Empty<byte>(), false);
            }
        }

        /// <summary>
        /// Decrypts RSA encrypted data using private key
        /// </summary>
        public (string decryptedData, bool success) DecryptRSA(string encryptedText, RSAParameters privateKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(encryptedText))
                    return (string.Empty, false);

                using (var rsa = RSA.Create())
                {
                    rsa.ImportParameters(privateKey);
                    var encryptedBytes = Convert.FromBase64String(encryptedText);
                    var decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
                    var decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                    return (decryptedText, true);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error decrypting RSA encrypted text");
                return (string.Empty, false);
            }
        }

        /// <summary>
        /// Generates an RSA key pair, returning the private and public keys as RSAParameters.
        /// </summary>
        /// <returns>A tuple containing the private key and public key as RSAParameters.</returns>
        public (RSAParameters privateKey, RSAParameters publicKey) GenerateRSAParameters()
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider(2048);
                var privateKey = rsa.ExportParameters(true);
                var publicKey = rsa.ExportParameters(false);
                return (privateKey, publicKey);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating RSA parameters");
                return (default, default);
            }
        }

        /// <summary>
        /// Encrypts plain text using AES-256-CBC
        /// </summary>
        public (string encryptedText, bool success, string errorMessage) EncryptAES(string plainText, byte[] key, byte[] iv)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plainText) || key == null || iv == null)
                    return (string.Empty, false, "invalid or an empty parameter was passed");

                if (key.Length < 16 || iv.Length != 16)
                {
                    logger.LogWarning("Invalid AES key length ({KeyLength}) or IV length ({IVLength})", key.Length, iv.Length);
                    return (string.Empty, false, "Invalid AES key length or IV length. At least 16 bytes is required for both the key and iv. The requires 16 bytes while the key can be more that 16 bytes but not less");
                }

                using (var aesAlgo = Aes.Create())
                {
                    aesAlgo.Key = key;
                    aesAlgo.IV = iv;
                    aesAlgo.Mode = CipherMode.CBC;
                    aesAlgo.Padding = PaddingMode.PKCS7;

                    using (var encryptor = aesAlgo.CreateEncryptor(aesAlgo.Key, aesAlgo.IV))
                    {
                        var plainBytes = Encoding.UTF8.GetBytes(plainText);
                        var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                        var encryptedText = Convert.ToBase64String(encryptedBytes);
                        return (encryptedText, true, "");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error encrypting AES plain text");
                return (string.Empty, false, $"An exception was thrown:\n{ex}");
            }
        }

        public (string decryptedText, bool success, string errorMessage) DecryptAES(string cipherText, byte[] key, byte[] iv)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cipherText) || key == null || iv == null)
                    return (string.Empty, false, "invalid or an empty parameter was passed");
                else if (key.Length < 16 || iv.Length != 16)
                    return (string.Empty, false, "Invalid AES key length or IV length. At least 16 bytes is required for both the key and iv. The requires 16 bytes while the key can be more that 16 bytes but not less");

                using Aes aesAlgo = Aes.Create();
                aesAlgo.Key = key;
                aesAlgo.IV = iv;
                aesAlgo.Mode = CipherMode.CBC;
                aesAlgo.Padding = PaddingMode.PKCS7;

                using var decryptor = aesAlgo.CreateDecryptor(aesAlgo.Key, aesAlgo.IV);
                var cipherBytes = Convert.FromBase64String(cipherText);
                var decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                var decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                return (decryptedText, true, "");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error decrypting AES cipher text");
                return (string.Empty, false, $"An exception was thrown:\n{ex}");
            }
        }

        /// <summary>
        /// Generates HMAC-SHA256 signature for data authentication
        /// </summary>
        public (string signature, bool success) GenerateHMACSHA256(string plainText, string key)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plainText) || string.IsNullOrWhiteSpace(key))
                    return (string.Empty, false);

                var keyBytes = Encoding.UTF8.GetBytes(key);
                var plainBytes = Encoding.UTF8.GetBytes(plainText);

                using (var hmac = new HMACSHA256(keyBytes))
                {
                    var hashBytes = hmac.ComputeHash(plainBytes);
                    var signature = Convert.ToBase64String(hashBytes);
                    return (signature, true);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating HMAC-SHA256 signature");
                return (string.Empty, false);
            }
        }

        public (string randomNumber, bool success) GenerateRandomNumber(uint length)
        {
            try
            {
                var randomNumber = RandomNumberGenerator.GetInt32((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length));
                return (randomNumber.ToString(), true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating random number");
                return (string.Empty, false);
            }
        }

        /// <summary>
        /// Generates a random token of specified length (in bytes)
        /// </summary>
        public (string randomToken, bool success) GenerateRandomToken(uint length)
        {
            try
            {
                if (length == 0)
                    return (string.Empty, false);

                var randomBytes = new byte[length];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomBytes);
                }

                var token = Convert.ToBase64String(randomBytes);
                return (token, true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating random token");
                return (string.Empty, false);
            }
        }

        public (string token, bool success) GenerateJWTToken(JwtSettings request)
        {
            try
            {
                var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, request.UserId.ToString()),
                new(ClaimTypes.Role, request.Role),
                new("VerificationStatus", request.VerificationStatus),
                new(ClaimTypes.Email, request.Email)
            };

                if (request.CustomClaims != null)
                {
                    claims.AddRange(request.CustomClaims);
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(request.SecretKey));
                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

                var token = new JwtSecurityToken(
                    issuer: request.Issuer,
                    audience: request.Audience != null ? string.Join(", ", request.Audience) : " ",
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(request.ExpirationMinutes),
                    signingCredentials: signingCredentials
                );

                var writtenToken = new JwtSecurityTokenHandler().WriteToken(token);

                return (writtenToken, true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating JWT token");
                return (string.Empty, false);
            }
        }

        /// <summary>
        /// Constant-time string comparison to prevent timing attacks... This method compares two strings in a way that takes the same amount of time regardless of how many characters match, 
        /// making it resistant to timing attacks that could reveal information about the contents of the strings. It returns true if the strings are equal and false otherwise.
        /// </summary>
        public bool ConstantTimeComparison(string a, string b)
        {
            if (a.Length != b.Length)
                return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }

            return result == 0;
        }

        public (string hashedToken, bool success) Hash_Argon2id(string value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Password cannot be empty or whitespace.");

                var config = new Argon2Config
                {
                    Type = Argon2Type.HybridAddressing,
                    Version = Argon2Version.Nineteen,
                    TimeCost = 3, 
                    MemoryCost = 1 << 16,
                    Lanes = 4,
                    Threads = Math.Min(2, Environment.ProcessorCount),
                    Salt = RandomNumberGenerator.GetBytes(16),
                    Password = Encoding.UTF8.GetBytes(value),
                    HashLength = 32 
                };

                using var argon2 = new Argon2(config);
                var hash = config.EncodeString(argon2.Hash().Buffer).ToString();
                return (hash, true);  
            }
            catch (Exception ex)
            {
                logger.LogCritical("{ex}", ex);
                return ("", false);
            }
        }

        public bool VerifyArgon2idHash(string value, string encodedHash)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(encodedHash))
                    return false;

                return Argon2.Verify(encodedHash, value);
            }
            catch(Exception ex)
            {
                logger.LogCritical("{ex}", ex);
                return false;
            }
        }
    }
}
