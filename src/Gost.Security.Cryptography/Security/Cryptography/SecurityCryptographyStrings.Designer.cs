﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gost.Security.Cryptography {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SecurityCryptographyStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SecurityCryptographyStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gost.Security.Cryptography.SecurityCryptographyStrings", typeof(SecurityCryptographyStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection..
        /// </summary>
        internal static string ArgumentInvalidOffLen {
            get {
                return ResourceManager.GetString("ArgumentInvalidOffLen", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Non-negative number required..
        /// </summary>
        internal static string ArgumentOutOfRangeNeedNonNegNum {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeNeedNonNegNum", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Positive number required..
        /// </summary>
        internal static string ArgumentOutOfRangeNeedPositiveNum {
            get {
                return ResourceManager.GetString("ArgumentOutOfRangeNeedPositiveNum", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified curve &apos;{0}&apos; or its parameters are not valid for this platform..
        /// </summary>
        internal static string CryptographicCurveNotSupported {
            get {
                return ResourceManager.GetString("CryptographicCurveNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Input buffer contains insufficient data..
        /// </summary>
        internal static string CryptographicInsufficientBuffer {
            get {
                return ResourceManager.GetString("CryptographicInsufficientBuffer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Specified block size is not valid for this algorithm..
        /// </summary>
        internal static string CryptographicInvalidBlockSize {
            get {
                return ResourceManager.GetString("CryptographicInvalidBlockSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified Characteristic2 curve parameters are not valid. Polynomial, A, B, G.X, G.Y, and Order are required. A, B, G.X, G.Y must be the same length, and the same length as Q.X, Q.Y and D if those are specified. Seed, Cofactor and Hash are optional. Other parameters are not allowed..
        /// </summary>
        internal static string CryptographicInvalidCharacteristic2Curve {
            get {
                return ResourceManager.GetString("CryptographicInvalidCharacteristic2Curve", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Specified cipher mode is not valid for this algorithm..
        /// </summary>
        internal static string CryptographicInvalidCipherMode {
            get {
                return ResourceManager.GetString("CryptographicInvalidCipherMode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified key or curve parameters are not valid..
        /// </summary>
        internal static string CryptographicInvalidCurveKeyParameters {
            get {
                return ResourceManager.GetString("CryptographicInvalidCurveKeyParameters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified OID is not valid. The friendly name or value must be set.
        /// </summary>
        internal static string CryptographicInvalidCurveOid {
            get {
                return ResourceManager.GetString("CryptographicInvalidCurveOid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Length of the data to transform is invalid..
        /// </summary>
        internal static string CryptographicInvalidDataSize {
            get {
                return ResourceManager.GetString("CryptographicInvalidDataSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Specified feedback is not a valid for this algorithm..
        /// </summary>
        internal static string CryptographicInvalidFeedbackSize {
            get {
                return ResourceManager.GetString("CryptographicInvalidFeedbackSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hash size must be {0} bytes..
        /// </summary>
        internal static string CryptographicInvalidHashSize {
            get {
                return ResourceManager.GetString("CryptographicInvalidHashSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Specified initialization vector (IV) does not match allowed size for this algorithm..
        /// </summary>
        internal static string CryptographicInvalidIVSize {
            get {
                return ResourceManager.GetString("CryptographicInvalidIVSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Specified key does not match allowed size for this algorithm..
        /// </summary>
        internal static string CryptographicInvalidKeySize {
            get {
                return ResourceManager.GetString("CryptographicInvalidKeySize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified named curve parameters are not valid. Only the OID parameter must be set..
        /// </summary>
        internal static string CryptographicInvalidNamedCurve {
            get {
                return ResourceManager.GetString("CryptographicInvalidNamedCurve", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This operation is not supported for this class..
        /// </summary>
        internal static string CryptographicInvalidOperation {
            get {
                return ResourceManager.GetString("CryptographicInvalidOperation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Padding is invalid and cannot be removed..
        /// </summary>
        internal static string CryptographicInvalidPadding {
            get {
                return ResourceManager.GetString("CryptographicInvalidPadding", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified prime curve parameters are not valid. Prime, A, B, G.X, G.Y and Order are required and must be the same length, and the same length as Q.X, Q.Y and D if those are specified. Seed, Cofactor and Hash are optional. Other parameters are not allowed..
        /// </summary>
        internal static string CryptographicInvalidPrimeCurve {
            get {
                return ResourceManager.GetString("CryptographicInvalidPrimeCurve", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Digital signature size must be {0} bytes..
        /// </summary>
        internal static string CryptographicInvalidSignatureSize {
            get {
                return ResourceManager.GetString("CryptographicInvalidSignatureSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not read the domain parameters from the XML string..
        /// </summary>
        internal static string CryptographicMissingDomainParameters {
            get {
                return ResourceManager.GetString("CryptographicMissingDomainParameters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not read the elliptic curve key value parameters root element from the XML string..
        /// </summary>
        internal static string CryptographicMissingECDsaKeyValue {
            get {
                return ResourceManager.GetString("CryptographicMissingECDsaKeyValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No asymmetric key object has been associated with this formatter object..
        /// </summary>
        internal static string CryptographicMissingKey {
            get {
                return ResourceManager.GetString("CryptographicMissingKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required object identifier (OID) cannot be found..
        /// </summary>
        internal static string CryptographicMissingOid {
            get {
                return ResourceManager.GetString("CryptographicMissingOid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not read the public key from the XML string..
        /// </summary>
        internal static string CryptographicMissingPublicKey {
            get {
                return ResourceManager.GetString("CryptographicMissingPublicKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The key cannot be changed after the first write to the stream..
        /// </summary>
        internal static string CryptographicSymmetricAlgorithmKeySet {
            get {
                return ResourceManager.GetString("CryptographicSymmetricAlgorithmKeySet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The symmetric algorithm name cannot be null or empty..
        /// </summary>
        internal static string CryptographicSymmetricAlgorithmNameNullOrEmpty {
            get {
                return ResourceManager.GetString("CryptographicSymmetricAlgorithmNameNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Symmetric algorithm name cannot be changed after the first write to the stream..
        /// </summary>
        internal static string CryptographicSymmetricAlgorithmNameSet {
            get {
                return ResourceManager.GetString("CryptographicSymmetricAlgorithmNameSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The XML namespace &apos;{0}&apos; was unexpected, expected &apos;{1}&apos;..
        /// </summary>
        internal static string CryptographicUnexpectedXmlNamespace {
            get {
                return ResourceManager.GetString("CryptographicUnexpectedXmlNamespace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown elliptic curve algorithm..
        /// </summary>
        internal static string CryptographicUnknownEllipticCurveAlgorithm {
            get {
                return ResourceManager.GetString("CryptographicUnknownEllipticCurveAlgorithm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is not a known symmetric algorithm..
        /// </summary>
        internal static string CryptographicUnknownSymmetricAlgorithm {
            get {
                return ResourceManager.GetString("CryptographicUnknownSymmetricAlgorithm", resourceCulture);
            }
        }
    }
}
