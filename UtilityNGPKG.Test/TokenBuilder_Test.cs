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
        private readonly string key = "823f2032mfy29204jfu2840tn4204822feetj7440347f40";
        [Fact]
        public void Generate_and_VerifyHMACSHA256()
        {
            string data = "TestText";
            var (token, success) = tokenBuilder.GenerateHMACSHA256(data, key);
            Assert.True(success);
            Assert.NotNull(token);

            var (error, isValid) = tokenBuilder.VerifyHMACSHA256(data, token, key);
            Assert.Empty(error);
            Assert.True(isValid);
        }
    }
}
