using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.Test
{
    using UtilityNGPKG.Test.config;

    public class SanitizationService_Test : BaseTestFeature
    {
        [SkippableFact]
        public void SanitizeHtml_RemovesMaliciousTags()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");
            string safeHtml = "<p>This is a <b>safe</b> text.</p>";
            string sanitizedSafe = sanitizationService.SanitizeHtml(safeHtml);
            Assert.Equal("This is a safe text.", sanitizedSafe); 
    
            string maliciousHtml = "<script>alert('xss');</script><p onload='bad()'>Test</p>";
            string sanitizedMalicious = sanitizationService.SanitizeHtml(maliciousHtml);
            
            Assert.DoesNotContain("<script>", sanitizedMalicious);
            Assert.DoesNotContain("onload", sanitizedMalicious);
        }

        [SkippableFact]
        public void SanitizeUrl_AllowsOnlyHttpAndHttps()
        {
            Skip.If(Environment.GetEnvironmentVariable("CI") == "true");
            string httpUrl = "http://example.com/page?q=1";
            string httpsUrl = "https://secure.example.com";
            
            Assert.Equal(httpUrl, sanitizationService.SanitizeUrl(httpUrl));
            Assert.Equal(httpsUrl, sanitizationService.SanitizeUrl(httpsUrl));

            Assert.Empty(sanitizationService.SanitizeUrl("javascript:alert(1)"));
            Assert.Empty(sanitizationService.SanitizeUrl("data:text/html,<script>alert(1)</script>"));
            Assert.Empty(sanitizationService.SanitizeUrl("ftp://files.example.com"));
            Assert.Empty(sanitizationService.SanitizeUrl("not-a-url"));
            Assert.Empty(sanitizationService.SanitizeUrl(""));
        }
    }
}
