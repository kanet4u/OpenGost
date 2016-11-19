using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading;

namespace OpenGost.Security.Cryptography
{
    using static SecurityCryptographyStrings;

    internal static class CryptoUtils
    {
        private static RandomNumberGenerator s_randomNumberGenerator;

        internal static RandomNumberGenerator StaticRandomNumberGenerator
#if NET45
            => LazyInitializer.EnsureInitialized(ref s_randomNumberGenerator, () => new RNGCryptoServiceProvider());
#elif NETCOREAPP1_0
            => LazyInitializer.EnsureInitialized(ref s_randomNumberGenerator, RandomNumberGenerator.Create);
#endif

        internal static byte[] GenerateRandomBytes(int size)
        {
            byte[] array = new byte[size];
            StaticRandomNumberGenerator.GetBytes(array);
            return array;
        }

        internal static T[] CloneArray<T>(T[] source)
            => source == null ? null : (T[])source.Clone();

        internal static T[] Subarray<T>(this T[] value, int startIndex)
            => value.Subarray(startIndex, value.Length - startIndex);

        internal static T[] Subarray<T>(this T[] value, int startIndex, int length)
        {
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ArgumentOutOfRangeStartIndex);
            if (startIndex > value.Length)
                throw new ArgumentOutOfRangeException(nameof(startIndex), ArgumentOutOfRangeStartIndexLargerThanLength);
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), ArgumentOutOfRangeNegativeLength);
            if (startIndex > value.Length - length)
                throw new ArgumentOutOfRangeException(nameof(length), ArgumentOutOfRangeIndexLength);

            if (length == 0)
                return EmptyArray<T>.Value;
            if (startIndex == 0 && length == value.Length)
                return CloneArray(value);

            T[] result = new T[length];
            Array.Copy(value, startIndex, result, 0, length);
            return result;
        }

        internal static void EraseData<T>(ref T[] data)
            where T : struct
        {
            if (data != null)
            {
                Array.Clear(data, 0, data.Length);
                data = null;
            }
        }

        internal static void EraseData<T>(ref T[][] data)
            where T : struct
        {
            if (data != null)
            {
                int length = data.Length;
                for (int i = 0; i < length; i++)
                    EraseData(ref data[i]);

                data = null;
            }
        }

        internal static ECCurve Clone(this ECCurve curve)
        {
            if (curve.IsNamed)
                return ECCurve.CreateFromOid(curve.Oid);

            return new ECCurve
            {
                A = CloneArray(curve.A),
                B = CloneArray(curve.B),
                G = Clone(curve.G),
                Order = CloneArray(curve.Order),
                Cofactor = CloneArray(curve.Cofactor),
                Seed = CloneArray(curve.Seed),
                CurveType = curve.CurveType,
                Hash = curve.Hash,
                Prime = CloneArray(curve.Prime),
                Polynomial = CloneArray(curve.Polynomial),
            };
        }

        internal static ECPoint Clone(this ECPoint point)
        {
            return new ECPoint
            {
                X = CloneArray(point.X),
                Y = CloneArray(point.Y)
            };
        }

        internal static BigInteger Normalize(BigInteger value, BigInteger modulus)
            => value >= BigInteger.Zero ? value : value + modulus;

        internal static int CombineHash(int hash1, int hash2) => (hash1 << 7 | hash1 >> 25) ^ hash2;
    }
}