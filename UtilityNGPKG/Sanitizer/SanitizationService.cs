using System.Net;
using Ganss.Xss;
using Microsoft.Extensions.Logging;
namespace UtilityNGPKG.Sanitizer
{
    /// <summary>
    /// Provides methods for sanitizing HTML and URL input to help prevent injection attacks and ensure safe content
    /// handling.
    /// </summary>
    /// <remarks>This service is intended for use in scenarios where user-supplied HTML or URLs need to be
    /// cleaned before being rendered or processed. It removes all HTML tags and attributes from input and validates
    /// URLs to allow only HTTP and HTTPS schemes. The service logs errors encountered during sanitization and applies
    /// fallback encoding or filtering as appropriate.</remarks>
    internal class SanitizationService : ISanitizationService
    {
        private readonly HtmlSanitizer sanitizer;
        private readonly ILogger<SanitizationService> logger;
        public SanitizationService(ILogger<SanitizationService> logger)
        {
            this.logger = logger;
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Clear();
            this.sanitizer.AllowedAttributes.Clear();
        }

        public string SanitizeHtml(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            try
            {
                var sanitized = sanitizer.Sanitize(input);
                return sanitized;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sanitizing HTML content");
                return WebUtility.HtmlEncode(input);
            }
        }

        public string SanitizeUrl(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            try
            {
                if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
                    return string.Empty;

                if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                    return string.Empty;

                return uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sanitizing URL");
                return string.Empty;
            }
        }
    }
}
