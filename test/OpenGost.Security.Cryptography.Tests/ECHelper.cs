﻿using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Xunit;

namespace OpenGost.Security.Cryptography
{
#if NET45
    [ExcludeFromCodeCoverage] 
#endif
    internal static class ECHelper
    {
        internal static void AssertEqual(ECPoint expected, ECPoint actual)
        {
            Assert.Equal(expected.X, actual.X);
            Assert.Equal(expected.Y, actual.Y);
        }

        internal static void AssertEqual(ECCurve expected, ECCurve actual)
        {
            Assert.Equal(expected.A, actual.A);
            Assert.Equal(expected.B, actual.B);
            AssertEqual(expected.G, actual.G);
            Assert.Equal(expected.Order, actual.Order);
            Assert.Equal(expected.Cofactor, actual.Cofactor);
            Assert.Equal(expected.Seed, actual.Seed);
            Assert.Equal(expected.Hash, actual.Hash);
            Assert.Equal(expected.Polynomial, actual.Polynomial);
            Assert.Equal(expected.Prime, actual.Prime);
            if (expected.IsNamed)
                AssertEqual(expected.Oid, actual.Oid);
        }

        internal static void AssertEqual(ECParameters expected, ECParameters actual, bool shouldComparePrivateData)
        {
            AssertEqual(expected.Curve, actual.Curve);
            AssertEqual(expected.Q, actual.Q);
            if (shouldComparePrivateData)
                Assert.Equal(expected.D, actual.D);
        }

        internal static void AssertEqual(Oid expected, Oid actual)
        {
            Assert.Equal(expected.Value, actual.Value);
            Assert.Equal(expected.FriendlyName, actual.FriendlyName);
        }
    }
}
