using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UtilityNGPKG.Test.config;
using UtilityNGPKG.Tokenomics;

namespace UtilityNGPKG.Test
{
    public class TokenBuilder_Test : BaseTestFeature
    {
        private readonly string data = "With everyday that goes by, i begin to see reasons getting married to the right person is as important  as eating";
        private readonly string string_key = "823f2032mfy29204jfu2840tn4204822feetj7440347f40";
        //AES Key size is typically 16, 24 or 32bytes
        private readonly byte[] byte_key = new byte[16] { 0, 1, 3, 5, 7, 9, 255, 16, 8, 2, 16, 32, 74, 96, 38, 4 };
        private readonly byte[] ten_byte_key = new byte[10] { 0, 1, 3, 5, 7, 9, 255, 16, 8, 2 };

        //The AES vector used for encryption is typically 16bytes
        private readonly byte[] vector = new byte[16] { 2,9,45,9,12,37,79,90,12,17,3,7,8,6,0,1 };
        private readonly byte[] ten_byte_vector = new byte[10] { 2,9,45,9,12,37,79,90,12,17};

        [Fact]
        public void Generate_and_VerifyHMACSHA256()
        {
            var (token, success) = tokenBuilder.GenerateHMACSHA256(data, string_key);
            Assert.True(success);
            Assert.NotNull(token);

            var (error, isValid) = tokenBuilder.VerifyHMACSHA256(data, token, string_key);
            Assert.Empty(error);
            Assert.True(isValid);

            (error, isValid) = tokenBuilder.VerifyHMACSHA256("WrongData", token, string_key);
            Assert.NotEmpty(error);
            Assert.False(isValid);

            (error, isValid) = tokenBuilder.VerifyHMACSHA256(data, token, "uiwiuefbwefyu82789rh4n34342f23r2r2r2-232uiwfw");
            Assert.NotEmpty(error);
            Assert.False(isValid);
        }

        [Fact]
        public void Encrypt_and_DecryptAES()
        {
            var (encryptedText, success, errorMessage) = tokenBuilder.EncryptAES(data, byte_key, vector);
            Assert.True(success);
            Assert.NotEmpty(encryptedText);
            Assert.Empty(errorMessage);

            var (decryptedText, isSuccessful, error) = tokenBuilder.DecryptAES(encryptedText, byte_key, vector); 
            Assert.True(isSuccessful);
            Assert.NotEmpty(decryptedText);
            Assert.Empty(error);
            Assert.Equal(decryptedText, data);
            
            (encryptedText, success, errorMessage) = tokenBuilder.EncryptAES(data, ten_byte_key, ten_byte_vector);
            Assert.False(success);
            Assert.Empty(encryptedText);
            Assert.NotEmpty(errorMessage);
        }

        [Fact]
        public void Encrypt_and_DecryptRSA()
        {
            var (privateKey, publicKey) = tokenBuilder.GenerateRSAParameters();

            // Check that the keys generated are not null empty structs (RSAParameters is a struct, so we check modulus)
            Assert.NotNull(privateKey.Modulus);
            Assert.NotNull(publicKey.Modulus);

            var (encryptedData, encryptSuccess) = tokenBuilder.EncryptRSA(data, publicKey);
            Assert.True(encryptSuccess);
            Assert.NotEmpty(encryptedData);

            string encryptedText = Convert.ToBase64String(encryptedData);

            var (decryptedData, decryptSuccess) = tokenBuilder.DecryptRSA(encryptedText, privateKey);
            Assert.True(decryptSuccess);
            Assert.Equal(data, decryptedData);

            var (failData, failSuccess) = tokenBuilder.EncryptRSA("", publicKey);
            Assert.False(failSuccess);
            Assert.Empty(failData);
        }

        [Fact]
        public void Hash_and_VerifyArgon2id()
        {
            var (hashedToken, hashSuccess) = tokenBuilder.Hash_Argon2id(string_key);
            Assert.True(hashSuccess);
            Assert.NotEmpty(hashedToken);
            Assert.Contains("$argon2id$", hashedToken);

            bool isValid = tokenBuilder.VerifyArgon2idHash(string_key, hashedToken);
            Assert.True(isValid);

            bool isInvalid = tokenBuilder.VerifyArgon2idHash("wrong_password_attempt", hashedToken);
            Assert.False(isInvalid);
            
            bool emptyInvalid = tokenBuilder.VerifyArgon2idHash("", hashedToken);
            Assert.False(emptyInvalid);
        }

        [Fact]
        public void GenerateJWTToken_CreatesValidString()
        {
            var request = new JwtSettings
            {
                UserId = Guid.NewGuid(),
                Email = "test@example.com",
                Role = "Admin",
                VerificationStatus = "Verified",
                SecretKey = tokenBuilder.GenerateRandomToken(64).randomToken,
                Issuer = "TestIssuer",
                ExpirationMinutes = 120
            };

            var (token, success) = tokenBuilder.GenerateJWTToken(request);
            
            Assert.True(success);
            Assert.NotEmpty(token);
            Assert.Equal(3, token.Split('.').Length);
        }

        [Fact]
        public void GenerateRandom_Number_and_Token()
        {
            uint numberLength = 6;
            var (randomNumber, numSuccess) = tokenBuilder.GenerateRandomNumber(numberLength);
            Assert.True(numSuccess);
            Assert.NotEmpty(randomNumber);
            Assert.Equal((int)numberLength, randomNumber.Length);
            Assert.True(int.TryParse(randomNumber, out _));

            uint tokenByteLength = 32;
            var (randomToken, tokenSuccess) = tokenBuilder.GenerateRandomToken(tokenByteLength);
            Assert.True(tokenSuccess);
            Assert.NotEmpty(randomToken);
            // Base64 string length for 32 bytes is 44 characters
            Assert.Equal(44, randomToken.Length); 

            var (failToken, failSuccess) = tokenBuilder.GenerateRandomToken(0);
            Assert.False(failSuccess);
            Assert.Empty(failToken);
        }

        [Fact]
        public void ConstantTimeComparison_EvaluatesCorrectly()
        {
            string stringA = "SuperSecretCompareString123";
            string stringB = "SuperSecretCompareString123";
            string stringC = "SuperSecretCompareString999";
            string stringD = "ShortString";

            bool isEqual = tokenBuilder.ConstantTimeComparison(stringA, stringB);
            Assert.True(isEqual);

            bool isNotEqual = tokenBuilder.ConstantTimeComparison(stringA, stringC);
            Assert.False(isNotEqual);

            bool isLengthNotEqual = tokenBuilder.ConstantTimeComparison(stringA, stringD);
            Assert.False(isLengthNotEqual);
        }
    }
}