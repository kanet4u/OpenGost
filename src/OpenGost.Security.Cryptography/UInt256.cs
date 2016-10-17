using System;

namespace OpenGost.Security.Cryptography
{
    using static CryptoUtils;
    using static SecurityCryptographyStrings;

    internal struct UInt256 : IComparable, IComparable<UInt256>, IEquatable<UInt256>, IFormattable
    {
        private static readonly UInt256 s_minValue = new UInt256(UInt128.MinValue, UInt128.MinValue);
        private static readonly UInt256 s_one = new UInt256(UInt128.MinValue, UInt128.One);
        private static readonly UInt256 s_maxValue = new UInt256(UInt128.MaxValue, UInt128.MaxValue);

        private UInt128 _low;
        private UInt128 _high;

        public static UInt256 MinValue => s_minValue;

        public static UInt256 One => s_one;

        public static UInt256 MaxValue => s_maxValue;

        public UInt256(int value)
        {
            throw new NotImplementedException();
        }

        public UInt256(uint value)
        {
            _low = value;
            _high = 0;
        }

        public UInt256(long value)
        {
            throw new NotImplementedException();
        }

        public UInt256(ulong value)
        {
            _low = value;
            _high = 0;
        }

        public UInt256(UInt128 value)
        {
            _low = value;
            _high = 0;
        }

        public UInt256(float value)
        {
            throw new NotImplementedException();
        }

        public UInt256(double value)
        {
            throw new NotImplementedException();
        }

        public UInt256(decimal value)
        {
            throw new NotImplementedException();
        }

        public UInt256(byte[] value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            throw new NotImplementedException();
        }

        private UInt256(UInt128 high, UInt128 low)
        {
            _low = low;
            _high = high;
        }

        public override bool Equals(object obj) => obj is UInt256 && Equals((UInt256)obj);

        public override int GetHashCode() => CombineHash(_high.GetHashCode(), _low.GetHashCode());

        public bool Equals(UInt256 other) => this == other;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (!(obj is UInt256))
                throw new ArgumentException(
                    string.Format(Culture, ArgumentMustBeOfType, typeof(UInt256).Name),
                    nameof(obj));

            return Compare(this, (UInt256)obj);
        }

        public int CompareTo(UInt256 other) => Compare(this, other);

        public static int Compare(UInt256 left, UInt256 right)
        {
            if (left._high != right._high)
                return left._high.CompareTo(right._high);

            return left._low.CompareTo(right._low);
        }

        public override string ToString() => ToString(null, null);

        public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

        public string ToString(string format) => ToString(format, null);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public static UInt256 Add(UInt256 left, UInt256 right)
        {
            UInt128 low = unchecked(left._high + right._high);
            UInt128 high = left._high + right._high;

            if (low < left._low)
                high++;

            return new UInt256(high, low);
        }

        public static UInt256 Subtract(UInt256 left, UInt256 right)
        {
            UInt128 low = unchecked(left._high - right._high);
            UInt128 high = left._high - right._high;

            if (left._low < right._low)
                high--;

            return new UInt256(high, low);
        }

        public static UInt256 Multiply(UInt256 left, UInt256 right)
        {
            UInt256 result = Multiply(left._low, right._low);
            result._high += left._high * right._low + left._low * right._high;
            return result;
        }

        private static UInt256 Multiply(UInt128 left, UInt128 right)
        {
            UInt128 u1 = (ulong)left;
            UInt128 v1 = (ulong)right;
            UInt128 t = u1 * v1;
            UInt128 w3 = (ulong)t;
            UInt128 k = (t >> 64);

            left >>= 64;
            t = (left * v1) + k;
            k = (ulong)t;
            UInt128 w1 = (t >> 64);

            right >>= 64;
            t = (u1 * right) + k;
            k = (t >> 64);

            return new UInt256((left * right) + w1 + k, (t << 64) + w3);
        }

        public static UInt256 Divide(UInt256 dividend, UInt256 divisor)
        {
            UInt256 remainder;
            return DivRem(dividend, divisor, out remainder);
        }

        public static UInt256 Remainder(UInt256 dividend, UInt256 divisor)
        {
            UInt256 remainder;
            DivRem(dividend, divisor, out remainder);
            return remainder;
        }

        public static UInt256 DivRem(UInt256 dividend, UInt256 divisor, out UInt256 remainder)
        {
            throw new NotImplementedException();
        }

        #region Operators

        public static implicit operator UInt256(byte value) => new UInt256(value);

        public static implicit operator UInt256(sbyte value) => new UInt256(value);

        public static implicit operator UInt256(short value) => new UInt256(value);

        public static implicit operator UInt256(ushort value) => new UInt256(value);

        public static implicit operator UInt256(int value) => new UInt256(value);

        public static implicit operator UInt256(uint value) => new UInt256(value);

        public static implicit operator UInt256(long value) => new UInt256(value);

        public static implicit operator UInt256(ulong value) => new UInt256(value);

        public static implicit operator UInt256(UInt128 value) => new UInt256(value);

        public static explicit operator UInt256(float value) => new UInt256(value);

        public static explicit operator UInt256(double value) => new UInt256(value);

        public static explicit operator UInt256(decimal value) => new UInt256(value);

        public static explicit operator byte(UInt256 value) => checked((byte)(int)value);

        public static explicit operator sbyte(UInt256 value) => checked((sbyte)(int)value);

        public static explicit operator short(UInt256 value) => checked((short)(int)value);

        public static explicit operator ushort(UInt256 value) => checked((ushort)(int)value);

        public static explicit operator int(UInt256 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator uint(UInt256 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator long(UInt256 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator ulong(UInt256 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator UInt128(UInt256 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator float(UInt256 value) => (float)(double)value;

        public static explicit operator double(UInt256 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator decimal(UInt256 value)
        {
            throw new NotImplementedException();
        }

        public static UInt256 operator &(UInt256 left, UInt256 right)
            => new UInt256(left._high & right._high, left._low & right._low);

        public static UInt256 operator |(UInt256 left, UInt256 right)
            => new UInt256(left._high | right._high, left._low | right._low);

        public static UInt256 operator ^(UInt256 left, UInt256 right)
            => new UInt256(left._high ^ right._high, left._low ^ right._low);

        public static UInt256 operator <<(UInt256 value, int shift)
        {
            if (shift == 0)
                return value;

            if (shift < 0)
                return value >> -shift;

            shift %= 256;

            UInt128 lowShiftedToHigh = value._low << (shift - 128);

            return (shift > 127) ?
                new UInt256(lowShiftedToHigh, 0) :
                new UInt256(lowShiftedToHigh | (value._high << shift), value._low << shift);
        }

        public static UInt256 operator >>(UInt256 value, int shift)
        {
            if (shift == 0)
                return value;

            if (shift < 0)
                return value << -shift;

            shift %= 256;

            UInt128 highShiftedToLow = value._high >> (shift - 128);

            return (shift > 127) ?
                new UInt256(0, highShiftedToLow) :
                new UInt256(value._high >> shift, highShiftedToLow | (value._low >> shift));
        }

        public static UInt256 operator ~(UInt256 value) => new UInt256(~value._high, ~value._high);

        public static bool operator ==(UInt256 left, UInt256 right)
            => left._low == right._low && left._high == right._high;

        public static bool operator !=(UInt256 left, UInt256 right)
            => left._low != right._low || left._high != right._high;

        public static UInt256 operator ++(UInt256 value) => Add(value, s_one);

        public static UInt256 operator --(UInt256 value) => Subtract(value, s_one);

        public static UInt256 operator *(UInt256 left, UInt256 right) => Multiply(left, right);

        public static UInt256 operator /(UInt256 dividend, UInt256 divisor) => Divide(dividend, divisor);

        public static UInt256 operator %(UInt256 dividend, UInt256 divisor) => Remainder(dividend, divisor);

        public static bool operator >(UInt256 left, UInt256 right) => left.CompareTo(right) > 0;

        public static bool operator <(UInt256 left, UInt256 right) => left.CompareTo(right) < 0;

        public static bool operator >=(UInt256 left, UInt256 right) => left.CompareTo(right) >= 0;

        public static bool operator <=(UInt256 left, UInt256 right) => left.CompareTo(right) <= 0;

        public static UInt256 operator +(UInt256 left, UInt256 right) => Add(left, right);

        public static UInt256 operator -(UInt256 left, UInt256 right) => Subtract(left, right);

        #endregion
    }
}
