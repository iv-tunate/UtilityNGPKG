namespace UtilityNGPKG.KYC.YouVerify
{
    /// <summary>
    /// Represents the KYC data required for a YouVerify verification request.
    /// </summary>
    public record YouVerifyKycDto
    {
        /// <summary>
        /// Represents the Identification number that needs to be verified (e.g., BVN, NIN, etc.).
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Represents the type of identification document being verified (e.g., BVN, NIN, International Passport, Driver's License).
        /// It is an enum that specifies the category of the identification document, allowing for clear differentiation between different types of documents during the verification process.
        /// </summary>
        public DocumentType Type { get; init; }

        /// <summary>
        /// The last name of the subject for additional identity matching.
        /// </summary>
        public string LastName { get; init; } = default!;

        /// <summary>
        /// Indicates whether the subject has consented to the verification.
        /// </summary>
        public bool IsSubjectConsent { get; init; } = true;

        public Guid UserId { get; init; }
    }
}
