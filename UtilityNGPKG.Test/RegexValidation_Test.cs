using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.Test
{
    public class RegexValidation_Test
    {
        [Fact]
        public void IsValidMail_EvaluatesCorrectly()
        {
            Assert.True(RegexValidations.IsValidMail("test@example.com"));
            Assert.True(RegexValidations.IsValidMail("user.name+tag@sub.domain.org"));

            Assert.False(RegexValidations.IsValidMail("plainaddress"));
            Assert.False(RegexValidations.IsValidMail("@missingadmin.com"));
            Assert.False(RegexValidations.IsValidMail("missingat.com"));
            Assert.False(RegexValidations.IsValidMail("space in@email.com"));
            Assert.False(RegexValidations.IsValidMail(""));
        }

        [Fact]
        public void IsValidName_EvaluatesCorrectly()
        {
            Assert.True(RegexValidations.IsValidName("John"));
            Assert.True(RegexValidations.IsValidName("John", "Doe"));
            Assert.True(RegexValidations.IsValidName("John", "Doe", "Smith"));

            Assert.False(RegexValidations.IsValidName("John123"));
            Assert.False(RegexValidations.IsValidName("John", "Doe!"));
            Assert.False(RegexValidations.IsValidName("John", "Doe", "Smith "));
            Assert.False(RegexValidations.IsValidName(""));
        }

        [Fact]
        public void IsValidPhoneNumber_EvaluatesCorrectly()
        {
            Assert.True(RegexValidations.IsValidPhoneNumber("+1234567890"));   
            Assert.True(RegexValidations.IsValidPhoneNumber("+2348123456789")); 
            Assert.True(RegexValidations.IsValidPhoneNumber("+14155552671"));  

            Assert.True(RegexValidations.IsValidPhoneNumber("2348123456789")); 
            Assert.True(RegexValidations.IsValidPhoneNumber("1234567"));    

            Assert.False(RegexValidations.IsValidPhoneNumber("12345"));
            Assert.False(RegexValidations.IsValidPhoneNumber("123456"));

            Assert.False(RegexValidations.IsValidPhoneNumber("+0123456789"));
            Assert.False(RegexValidations.IsValidPhoneNumber("0802123456"));

            Assert.False(RegexValidations.IsValidPhoneNumber("phone"));
            Assert.False(RegexValidations.IsValidPhoneNumber("+234 812 345 678"));
            Assert.False(RegexValidations.IsValidPhoneNumber("(234)812-3456"));

            Assert.False(RegexValidations.IsValidPhoneNumber(""));
        }

        [Fact]
        public void IsAcceptablePasswordFormat_EvaluatesCorrectly()
        {
            Assert.True(RegexValidations.IsAcceptablePasswordFormat("StrongPass1!"));
            Assert.True(RegexValidations.IsAcceptablePasswordFormat("vErysec#re99"));

            Assert.False(RegexValidations.IsAcceptablePasswordFormat("weakpass"));
            Assert.False(RegexValidations.IsAcceptablePasswordFormat("StrongPass!")); 
            Assert.False(RegexValidations.IsAcceptablePasswordFormat("StrongPass1")); 
            Assert.False(RegexValidations.IsAcceptablePasswordFormat("str1!")); 
            Assert.False(RegexValidations.IsAcceptablePasswordFormat(""));
        }
    }
}
