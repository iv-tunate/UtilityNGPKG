using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.Sanitizer
{
    /// <summary>
    /// This interface defines the contract for a sanitization service that provides methods to sanitize HTML and URL inputs. Implementing this interface allows for consistent sanitization of user-generated content, helping to prevent security vulnerabilities such as cross-site scripting (XSS) attacks and ensuring that URLs are safe for use in web applications.
    /// Might not work properly or at all with .NET 6.0, consider using .NET 8.0 for better performance and security features related to sanitization.
    /// </summary>
    public interface ISanitizationService
    {
        /// <summary>
        /// Removes potentially unsafe or unwanted HTML elements and attributes from the specified string.
        /// </summary>
        /// <remarks>Use this method to prevent cross-site scripting (XSS) and other injection attacks
        /// when displaying user-supplied HTML content. The specific elements and attributes that are removed depend on
        /// the implementation.</remarks>
        /// <param name="value">The HTML content to sanitize. Cannot be null.</param>
        /// <returns>A sanitized string containing only safe HTML. Returns an empty string if the input is null or empty.</returns>
        string SanitizeHtml(string value);
        
        /// <summary>
        /// Returns a sanitized version of the specified URL string, removing or encoding potentially unsafe characters.
        /// </summary>
        /// <param name="value">The URL string to sanitize. Cannot be null.</param>
        /// <returns>A sanitized URL string that is safe for use in web contexts. Returns an empty string if the input is null or
        /// empty.</returns>
        string SanitizeUrl(string value);
    }
}
