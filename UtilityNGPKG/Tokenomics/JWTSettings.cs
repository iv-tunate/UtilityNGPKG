using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.Tokenomics
{
    /// <summary>
    /// Represents a request for generating a JSON Web Token (JWT) for a user, including user identity and custom
    /// claims.
    /// </summary>
    /// <remarks>This class is typically used to supply the necessary information when requesting a JWT from
    /// an authentication service. It includes standard user identification fields as well as support for additional
    /// custom claims. All properties should be populated with valid values to ensure correct token
    /// generation.</remarks>
    public class JwtTokenRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Gets or sets the email address associated with the user.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the role associated with the user or entity. It's important to set this property correctly as it can be used in the JWT claims to determine the user's permissions and access levels within the application. The role can be a string representing different user roles (e.g., "Admin", "User", "Manager") depending on your application's requirements. This information can be utilized by downstream services to enforce authorization rules based on the user's role when processing requests that include the JWT.
        /// </summary>
        public string Role { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the current verification status of the user or entity for which the JWT is being generated. This property can be used to indicate whether the user has completed necessary verification steps (such as email verification, phone verification, or identity verification) required by the application. The value of this property can be a string representing different verification states (e.g., "Verified", "Unverified", "Pending") depending on the application's requirements. This information can be included in the JWT claims to allow downstream services to make authorization decisions based on the user's verification status.
        /// </summary>
        public string VerificationStatus { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the collection of custom claims associated with the user or entity. If there are other claims needed to be included in the JWT beyond the standard ones (like UserId, Email, Role, etc.), they can be added to this list. Each claim consists of a type and a value, allowing for flexible inclusion of additional information in the token as needed by the application. This property can be null or empty if no custom claims are required.
        /// </summary>
        public List<Claim>? CustomClaims { get; set; }
        /// <summary>
        /// Gets or sets the secret key used for authentication or encryption purposes.
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// Represents the Issuer of the Token. This is typically the entity that creates and signs the JWT. It can be a URL, a name, or any identifier that represents the issuer of the token. The Issuer claim is used to identify the principal that issued the JWT and can be used by the recipient to validate the token's authenticity and trustworthiness.
        /// It is important to set this property correctly to ensure that the generated JWT can be properly validated by the intended recipients. Ensure it matches the issuer on your config file 
        /// </summary>
        public string Issuer { get; set; } = "";
        /// <summary>
        /// Gets or sets the list of intended audiences for the token. If you need to validate the audience of the token, you can specify one or more audience values that represent the recipients or services that are allowed to accept the token. The Audience claim is used to identify the intended recipients of the JWT and can be used by the recipient to verify that the token was issued for them. 
        /// If this property is not set or is empty, it may indicate that the token is intended for any audience, depending on your validation logic. Make sure to set this property according to your application's requirements to ensure proper token validation.
        /// </summary>
        public string[]? Audience { get; set; }
        /// <summary>
        /// Gets or sets the expiration time, in minutes of the token. If no value is passed, it defaults to 60 minutes. This property determines how long the generated JWT will be valid before it expires. The expiration time is typically included in the "exp" claim of the JWT and is used by recipients to determine whether the token is still valid or has expired. Setting an appropriate expiration time is important for security reasons, as it limits the window of opportunity for an attacker to use a stolen token. Adjust this value based on your application's security requirements and user experience considerations.
        /// </summary>
        public int ExpirationMinutes { get; set; } = 60;
    }
}
