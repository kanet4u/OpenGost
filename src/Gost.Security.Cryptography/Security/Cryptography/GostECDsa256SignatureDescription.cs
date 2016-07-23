﻿using System.Security.Cryptography;

namespace Gost.Security.Cryptography
{
    using static CryptoConstants;

    /// <summary>
    /// Contains information about the properties of the 256-bit 
    /// GOST Elliptic Curve Digital Signature (GOST R 34.10-2012).
    /// </summary>
    public class GostECDsa256SignatureDescription : SignatureDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GostECDsa256SignatureDescription"/> class.
        /// </summary>
        public GostECDsa256SignatureDescription()
        {
            KeyAlgorithm = GostECDsa256AlgorithmFullName;
            DigestAlgorithm = Streebog256AlgorithmFullName;
            FormatterAlgorithm = GostECDsaSignatureFormatterFullName;
            DeformatterAlgorithm = GostECDsaSignatureDeformatterFullName;
        }
    }
}
