using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityNGPKG.Test.config;

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
    }
}