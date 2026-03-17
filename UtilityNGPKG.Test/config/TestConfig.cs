using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace UtilityNGPKG.Test.config
{
    public class TestConfig
    {
        public string YouVerifyAPIKEY { get; set; }
        public string CloudinaryAPISecret { get; set; }
        public string CloudinaryAPIKEY { get; set; }
        public string SendGridApiKey { get; set; }
        public string IpInfoKey { get; set; }
        public string MailTrapApiKey { get; set; }
        public string PaystackTestSecret { get; set; }
        public string ExchangeAPIKey { get; set; }
    }
}
