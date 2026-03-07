using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityNGPKG.KYC.YouVerify
{
    /// <summary>
    /// Represents the different types of identification documents that can be used for KYC verification with YouVerify. This enumeration defines the supported document types, including BVN (Bank Verification Number), NIN (National Identification Number), International Passport, and Driver's License.
    /// Each member of the DocumentType enum corresponds to a specific type of identification document that may be required during the KYC verification process, allowing for clear and standardized representation of document types in the codebase when performing verifications or handling related logic.
    /// </summary>
    public enum DocumentType
    {
        BVN,
        NIN,
        InternationalPassport,
        DriversLicense
    }
}
