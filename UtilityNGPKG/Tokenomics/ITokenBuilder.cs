using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.Tokenomics
{
    /// <summary>
    /// This interface defines the cryptography contracts. Encryption, decryption, token generation such as: JWT token generation, refresh tokens, random tokens, etc are defined within this contract
    /// </summary>
    public interface ITokenBuilder
    {
        /// <summary>
        /// This Encrypts a plain text using HMACSHA256 algorithm and a secret key. 
        /// The resulting encrypted string is typically represented in Base64 format for safe transmission and storage. 
        /// This method is commonly used for securely hashing sensitive data, such as passwords or tokens, to ensure confidentiality and integrity.
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <returns> The encrypted data along with the success status</returns>
        (string signature, bool success) GenerateHMACSHA256(string plainText, string key);

        /// <summary>
        /// Chec
        /// </summary>
        /// <param name="data">The base64-encoded string to decrypt. Must not be null or empty.</param>
        /// <param name="expectedSignature">The expected HMACSHA256 signature to compare against the decrypted data. Must not be null or empty.</param>
        /// <param name="key">The secret key used for HMACSHA256 decryption. Must not be null or empty.</param>
        /// <returns>A string containing the decrypted plain text and true or false if the operation was successful. Returns null if decryption fails or the input is invalid.</returns>
        (string error, bool isValid) VerifyHMACSHA256(string data, string expectedSignature, string key);

        /// <summary>
        /// Encrypts the specified plain text using the AES symmetric encryption algorithm.
        /// </summary>
        /// <remarks>The caller is responsible for providing a secure key and initialization vector. The
        /// same key and IV must be used for decryption. This method does not validate the strength or randomness of the
        /// key and IV.</remarks>
        /// <param name="plainText">The text to encrypt. Cannot be null or empty.</param>
        /// <param name="key">The secret key to use for encryption. Must be a valid AES key size (typically 16, 24, or 32 bytes). Cannot
        /// be null.</param>
        /// <param name="iv">The initialization vector to use for encryption. Must be the correct length for the AES algorithm (typically
        /// 16 bytes). Cannot be null.</param>
        /// <returns>A Base64-encoded string containing the encrypted representation of the input text and true or false indicating whether the operation was successful along with the error message.</returns>
        (string encryptedText, bool success, string errorMessage) EncryptAES(string plainText, byte[] key, byte[] iv);

        /// <summary>
        /// Decrypts the specified AES-encrypted text using the provided key and initialization vector (IV).
        /// </summary>
        /// <remarks>The caller is responsible for ensuring that the key and IV match those used during
        /// encryption. If the input parameters are invalid or the decryption fails, an exception may be
        /// thrown.</remarks>
        /// <param name="cipherText">The base64-encoded string containing the AES-encrypted data to decrypt. Cannot be null or empty.</param>
        /// <param name="key">The secret key used for AES decryption. Must be a valid length for the AES algorithm (typically 16, 24, or
        /// 32 bytes... This method expects at least 16 bytes). Cannot be null.</param>
        /// <param name="iv">The initialization vector (IV) used for AES decryption. Must be the correct length for the AES algorithm
        /// (typically 16 bytes). Cannot be null.</param>
        /// <returns>The decrypted plain text as a string and true or false statement indicating the success status of the operation along with an error message if any.</returns>
        (string decryptedText, bool success, string errorMessage) DecryptAES(string cipherText, byte[] key, byte[] iv);

        /// <summary>
        /// Encrypts the specified plain text using RSA encryption with the provided public key.
        /// </summary>
        /// <param name="plainText">The text to encrypt. Cannot be null or empty.</param>
        /// <param name="publicKey">The RSA public key parameters used to encrypt the data.</param>
        /// <returns>A byte array containing the encrypted data true or false indicating whether the operation was successful.</returns>
        (byte[] encryptedData, bool success) EncryptRSA(string plainText, RSAParameters publicKey);

        /// <summary>
        /// Decrypts the specified RSA-encrypted text using the provided private key.
        /// </summary>
        /// <remarks>The encrypted text must have been encrypted using the corresponding public key. If
        /// the private key is invalid or does not match the encryption key, decryption will fail.</remarks>
        /// <param name="encryptedText">The base64-encoded string representing the data encrypted with RSA.</param>
        /// <param name="privateKey">The RSA private key parameters used to decrypt the encrypted text. Must contain the private exponent and
        /// modulus.</param>
        /// <returns>The decrypted plain text as a string.</returns>
        (string decryptedData, bool success) DecryptRSA(string encryptedText, RSAParameters privateKey);

        /// <summary>
        /// Generates an RSA key pair, returning the private and public keys as RSAParameters.
        /// </summary>
        /// <returns>A tuple containing the private key and public key as RSAParameters.</returns>
        (RSAParameters privateKey, RSAParameters publicKey) GenerateRSAParameters();

        /// <summary>
        /// Generates a random alphanumeric token of the specified length. 
        /// </summary>
        /// <param name="length">The number of characters in the generated token. Must be a positive integer.</param>
        /// <returns>A randomly generated string consisting of alphanumeric characters with the specified length along with the success status</returns>
        (string randomToken, bool success) GenerateRandomToken(uint length);

        /// <summary>
        /// Generates a random numeric string of the specified length.
        /// </summary>
        /// <param name="length">The number of digits to include in the generated string. Must be greater than 0.</param>
        /// <returns>A string consisting of random numeric digits with the specified length along with the success status.</returns>
        (string randomNumber, bool success) GenerateRandomNumber(uint length);

        /// <summary>
        /// Generates a JSON Web Token (JWT) based on the specified request parameters.
        /// </summary>
        /// <param name="request">An object containing the parameters required to generate the JWT, such as user claims, expiration, and
        /// signing credentials. Cannot be null. Please note that the JWT Secret key on the JWT settings class <see cref="JwtSettings.SecretKey"/> must be a 64 byte (512bit) secret key string</param>
        /// <returns>A tuple containing the generated JWT as a string and a Boolean value indicating whether the token generation
        /// was successful. If token generation fails, the string will be empty and the Boolean will be <see
        /// langword="false"/>.</returns>
        (string token, bool success) GenerateJWTToken(JwtSettings request);

        /// <summary>
        /// Compares two strings for equality using a constant-time algorithm to help prevent timing attacks.
        /// </summary>
        /// <remarks>This method is intended for scenarios where resistance to timing attacks is
        /// important, such as comparing cryptographic secrets. The comparison takes the same amount of time regardless
        /// of where the first difference occurs in the strings.</remarks>
        /// <param name="a">The first string to compare. Can be null.</param>
        /// <param name="b">The second string to compare. Can be null.</param>
        /// <returns>true if the strings are equal; otherwise, false.</returns>
        bool ConstantTimeComparison(string a, string b);

        /// <summary>
        /// Generates a secure Argon2id hash from the provided value.
        /// </summary>
        /// <remarks>
        /// The resulting hash contains the Argon2 parameters, salt, and hash
        /// encoded in a single string. This value is safe to store in a database
        /// and can later be verified using <see cref="VerifyArgon2idHash"/>.
        /// </remarks>
        /// <param name="value">The plain text value to hash.</param>
        /// <returns>
        /// A tuple containing the encoded Argon2id hash and a boolean indicating
        /// whether the hashing operation succeeded.
        /// </returns>
        (string hashedToken, bool success) Hash_Argon2id(string value);

        /// <summary>
        /// Verifies a plain text value against a previously generated Argon2id hash.
        /// </summary>
        /// <remarks>
        /// This method validates the supplied value by recomputing the Argon2id hash
        /// using the parameters embedded in the stored hash string.
        /// </remarks>
        /// <param name="value">The plain text value to verify.</param>
        /// <param name="hashedToken">The stored Argon2id encoded hash.</param>
        /// <returns>
        /// <c>true</c> if the value matches the stored hash; otherwise <c>false</c>.
        /// </returns>
        bool VerifyArgon2idHash(string value, string hashedToken);
    }
}
