using System.Text.RegularExpressions;

namespace UtilityNGPKG
{
    public partial class RegexValidations
    {
        /// <summary>
        /// Regular expression for validating names (alphabetic only).
        /// </summary>
    #if NET7_0_OR_GREATER
        [GeneratedRegex("^[a-zA-Z]+$")]
        private static partial Regex NameRegex();
    #else
        private static readonly Regex _nameRegex = new Regex("^[a-zA-Z]+$", RegexOptions.Compiled);
    #endif

        /// <summary>
        /// Regular expression for validating email addresses.
        /// This regex checks for a basic email format: local part followed by '@' and a domain part with at least one dot.
        /// </summary>
    #if NET7_0_OR_GREATER
        [GeneratedRegex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
        private static partial Regex EmailRegex();
    #else
        private static readonly Regex _emailRegex = new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", RegexOptions.Compiled);
    #endif
        /// <summary>
        /// Regular expression for validating phone numbers (E.164 format).
        /// </summary>
    #if NET7_0_OR_GREATER
        [GeneratedRegex(@"^\+?[1-9]\d{1,14}$")]
        private static partial Regex PhoneRegex();
    #else
        private static readonly Regex _phoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled);
    #endif

        /// <summary>
        /// Regular expression for validating password format.
        /// Must include at least one lowercase letter, one uppercase letter,
        /// one number, one special character, and be at least 8 characters long.
        /// </summary>
    #if NET7_0_OR_GREATER
        [GeneratedRegex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$")]
        private static partial Regex PasswordRegex();
    #else
        private static readonly Regex _passwordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$", RegexOptions.Compiled);
    #endif
        /// <summary>
        /// Validates that a given email address is in a proper format.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns><c>true</c> if the email is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidMail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

#if NET8_0_OR_GREATER
                return EmailRegex().IsMatch(email);
#else
                return _emailRegex.IsMatch(email);
#endif
        }

        /// <summary>
        /// Validates that the provided names contain only alphabetic characters.
        /// At least the first name must be provided and valid; last name and middle name are optional but must also be valid if provided.
        /// </summary>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name. (optional)</param>
        /// <param name="middleName">The user's middle name (optional).</param>
        /// <returns><c>true</c> if all provided names are valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidName(string firstName, string lastName = "", string middleName = "")
        {
#if NET7_0_OR_GREATER
            if (!string.IsNullOrWhiteSpace(lastName) && !NameRegex().IsMatch(lastName))
                return false;
            if (!string.IsNullOrWhiteSpace(middleName) && !NameRegex().IsMatch(middleName))
                return false;
            if (!NameRegex().IsMatch(firstName))
                return false;
#else
            if (!string.IsNullOrWhiteSpace(lastName) && !_nameRegex.IsMatch(lastName))
                return false;
            if (!string.IsNullOrWhiteSpace(middleName) && !_nameRegex.IsMatch(middleName))
                return false;
            if (!_nameRegex.IsMatch(firstName))
                return false;
#endif
            return true;
        }

        /// <summary>
        /// Validates that the provided phone number is exactly 11 digits.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <returns><c>true</c> if the phone number is valid; otherwise, <c>false</c>.</returns>
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

#if NET7_0_OR_GREATER
            return PhoneRegex().IsMatch(phoneNumber);
#else
            return _phoneRegex.IsMatch(phoneNumber);
#endif
        }

        /// <summary>
        /// Validates whether the provided password meets complexity requirements.
        /// The password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns><c>true</c> if the password format is acceptable; otherwise, <c>false</c>.</returns>
        public static bool IsAcceptablePasswordFormat(string password)
        {
#if NET7_0_OR_GREATER
            return PasswordRegex().IsMatch(password);
#else
            return _passwordRegex.IsMatch(password);
#endif
        }
    }
}