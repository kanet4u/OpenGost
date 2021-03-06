﻿using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace OpenGost.Security.Cryptography
{
    using static Buffer;
    using static CryptoUtils;
    using static Math;
    using static SecurityCryptographyStrings;

    /// <summary>
    /// Provides a managed implementation of the <see cref="GostECDsa512"/> algorithm. 
    /// </summary>
    [ComVisible(true)]
    public sealed class GostECDsa512Managed : GostECDsa512
    {
        private static readonly BigInteger s_modulus = BigInteger.One << 512;

        private ECCurve _curve;
        private ECPoint _publicKey;
        private byte[] _privateKey;
        private bool
            _parametersSet = false,
            _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GostECDsa512Managed" /> class
        /// with a random key pair.
        /// </summary>
        public GostECDsa512Managed()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GostECDsa512Managed" /> class
        /// with a specified <see cref="ECParameters"/>.
        /// </summary>
        /// <param name="parameters">
        /// The elliptic curve parameters. Valid key size is 512 bits.
        /// </param>
        /// <exception cref="CryptographicException">
        /// <paramref name="parameters"/> specifies an invalid key length.
        /// </exception>
        public GostECDsa512Managed(ECParameters parameters)
        {
            ImportParameters(parameters);
        }

        /// <summary>
        /// Generates a new public/private key pair for the specified curve.
        /// </summary>
        /// <param name="curve">
        /// The curve to use.
        /// </param>
        /// <exception cref="CryptographicException">
        /// <paramref name="curve"/> is invalid.
        /// </exception>
        [ComVisible(false)]
        public override void GenerateKey(ECCurve curve)
        {
            curve.Validate();
            int keySizeInByted = curve.Prime.Length;
            KeySize = keySizeInByted * 8;

            BigInteger
                prime = Normalize(new BigInteger(curve.Prime), s_modulus),
                subgroupOrder = Normalize(new BigInteger(curve.Order), s_modulus) / Normalize(new BigInteger(curve.Cofactor), s_modulus),
                a = Normalize(new BigInteger(curve.A), s_modulus);

            byte[] privateKey = new byte[keySizeInByted];
            BigInteger key;
            do
            {
                StaticRandomNumberGenerator.GetBytes(privateKey);
                key = Normalize(new BigInteger(privateKey), s_modulus);
            } while (BigInteger.Zero >= key || key >= subgroupOrder);

            var basePoint = new BigIntegerPoint(curve.G, s_modulus);

            ECPoint publicKey = BigIntegerPoint.Multiply(basePoint, key, prime, a).ToECPoint(KeySize);

            EraseData(ref _privateKey);
            _curve = curve.Clone();
            _publicKey = publicKey;
            _privateKey = privateKey;
            _parametersSet = true;
        }

        /// <summary>
        /// Exports the <see cref="ECParameters"/> for an <see cref="ECCurve"/>.
        /// </summary>
        /// <param name="includePrivateParameters">
        /// <c>true</c> to include private parameters;
        /// otherwise, <c>false</c>.</param>
        /// <returns>
        /// An <see cref="ECParameters"/>.
        /// </returns>
        /// <exception cref="CryptographicException">
        /// The key cannot be exported. 
        /// </exception>
        [ComVisible(false)]
        public override ECParameters ExportParameters(bool includePrivateParameters)
        {
            ThrowIfDisposed();

            if (!_parametersSet)
                GenerateKey(GetDefaultCurve());

            return new ECParameters
            {
                Curve = _curve.Clone(),
                Q = _publicKey.Clone(),
                D = includePrivateParameters ? CloneArray(_privateKey) : null,
            };
        }

        /// <summary>
        /// Imports the specified <see cref="ECParameters"/>.
        /// </summary>
        /// <param name="parameters">
        /// The curve parameters.
        /// </param>
        /// <exception cref="CryptographicException">
        /// <paramref name="parameters"/> are invalid.
        /// </exception>
        [ComVisible(false)]
        public override void ImportParameters(ECParameters parameters)
        {
            ThrowIfDisposed();

            parameters.Validate();
            KeySize = parameters.Q.X.Length * 8;

            _curve = parameters.Curve.Clone();
            _publicKey = parameters.Q.Clone();
            _privateKey = CloneArray(parameters.D);
            _parametersSet = true;
        }

        /// <summary>
        /// Generates a digital signature for the specified hash value.
        /// </summary>
        /// <param name="hash">
        /// The hash value of the data that is being signed.
        /// </param>
        /// <returns>
        /// A digital signature that consists of the given hash value encrypted with the private key.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="hash"/> parameter is <c>null</c>.
        /// </exception>
        public override byte[] SignHash(byte[] hash)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));

            ThrowIfDisposed();

            if (KeySize / 8 != hash.Length)
                throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, CryptographicInvalidHashSize, KeySize / 8));

            int keySizeInByted = KeySize / 8;

            if (!_parametersSet)
                GenerateKey(GetDefaultCurve());

            BigInteger
                subgroupOrder = Normalize(new BigInteger(_curve.Order), s_modulus) / Normalize(new BigInteger(_curve.Cofactor), s_modulus);

            BigInteger e = Normalize(new BigInteger(hash), s_modulus) % subgroupOrder;

            if (e == BigInteger.Zero)
                e = BigInteger.One;

            BigInteger
                prime = Normalize(new BigInteger(_curve.Prime), s_modulus),
                a = Normalize(new BigInteger(_curve.A), s_modulus),
                d = Normalize(new BigInteger(_privateKey), s_modulus),
                k, r, s;

            var rgb = new byte[keySizeInByted];

            do
            {
                do
                {
                    do
                    {
                        StaticRandomNumberGenerator.GetBytes(rgb);
                        k = Normalize(new BigInteger(rgb), s_modulus);
                    } while (k <= BigInteger.Zero || k >= subgroupOrder);

                    r = BigIntegerPoint.Multiply(new BigIntegerPoint(_curve.G, s_modulus), k, prime, a).X;
                } while (r == BigInteger.Zero);

                s = (r * d + k * e) % subgroupOrder;
            } while (s == BigInteger.Zero);

            byte[]
                signature = new byte[keySizeInByted * 2],
                array = s.ToByteArray();

            BlockCopy(array, 0, signature, 0, Min(array.Length, keySizeInByted));
            array = r.ToByteArray();
            BlockCopy(array, 0, signature, keySizeInByted, Min(array.Length, keySizeInByted));

            return signature;
        }

        /// <summary>
        /// Verifies a digital signature against the specified hash value. 
        /// </summary>
        /// <param name="hash">
        /// The hash value of a block of data.
        /// </param>
        /// <param name="signature">
        /// The digital signature to be verified.
        /// </param>
        /// <returns>
        /// <c>true</c> if the hash value equals the decrypted signature;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="hash"/> parameter is <c>null</c>.
        /// -or-
        /// The <paramref name="signature"/> parameter is <c>null</c>.
        /// </exception>
        public override bool VerifyHash(byte[] hash, byte[] signature)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            if (signature == null) throw new ArgumentNullException(nameof(signature));

            ThrowIfDisposed();

            if (KeySize / 8 != hash.Length)
                throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, CryptographicInvalidHashSize, KeySize / 8));
            if (KeySize / 4 != signature.Length)
                throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, CryptographicInvalidSignatureSize, KeySize / 4));

            // There is no necessity to generate new parameter, just return false
            if (!_parametersSet)
                return false;

            int keySizeInByted = KeySize / 8;

            BigInteger
                subgroupOrder = Normalize(new BigInteger(_curve.Order), s_modulus) / Normalize(new BigInteger(_curve.Cofactor), s_modulus);

            byte[] array = new byte[keySizeInByted];

            BlockCopy(signature, 0, array, 0, keySizeInByted);
            BigInteger s = Normalize(new BigInteger(array), s_modulus);
            if (s < BigInteger.One || s > subgroupOrder)
                return false;

            BlockCopy(signature, keySizeInByted, array, 0, keySizeInByted);
            BigInteger r = Normalize(new BigInteger(array), s_modulus);
            if (r < BigInteger.One || r > subgroupOrder)
                return false;

            BigInteger e = Normalize(new BigInteger(hash), s_modulus) % subgroupOrder;

            if (e == BigInteger.Zero)
                e = BigInteger.One;

            BigInteger
                v = BigInteger.ModPow(e, subgroupOrder - 2, subgroupOrder),
                z1 = (s * v) % subgroupOrder,
                z2 = (subgroupOrder - r) * v % subgroupOrder,
                prime = Normalize(new BigInteger(_curve.Prime), s_modulus),
                a = Normalize(new BigInteger(_curve.A), s_modulus);

            BigIntegerPoint c = BigIntegerPoint.Add(
                BigIntegerPoint.Multiply(new BigIntegerPoint(_curve.G, s_modulus), z1, prime, a),
                BigIntegerPoint.Multiply(new BigIntegerPoint(_publicKey, s_modulus), z2, prime, a),
                prime);

            return c.X == r;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="GostECDsa512Managed"/> class
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c>true to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                EraseData(ref _privateKey);

                if (disposing)
                {
                    EraseData(ref _curve.Prime);
                    EraseData(ref _curve.A);
                    EraseData(ref _curve.B);
                    EraseData(ref _curve.Order);
                    EraseData(ref _curve.Cofactor);
                    EraseData(ref _publicKey.X);
                    EraseData(ref _publicKey.Y);
                    ECPoint g = _curve.G;
                    EraseData(ref g.X);
                    EraseData(ref g.Y);
                }
            }

            base.Dispose(disposing);
            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        private static ECCurve GetDefaultCurve()
            => ECCurveOidMap.GetExplicitCurveByOid("1.2.643.7.1.2.1.2.1");
    }
}
