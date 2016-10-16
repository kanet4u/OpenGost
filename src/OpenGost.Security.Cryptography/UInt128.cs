using System;

namespace OpenGost.Security.Cryptography
{
    using static CryptoUtils;
    using static SecurityCryptographyStrings;

    internal struct UInt128 : IComparable, IComparable<UInt128>, IEquatable<UInt128>, IFormattable
    {
        private static readonly UInt128 s_minValue = new UInt128(0, 0);
        private static readonly UInt128 s_one = new UInt128(0, 1);
        private static readonly UInt128 s_maxValue = new UInt128(ulong.MaxValue, ulong.MaxValue);

        private ulong _low;
        private ulong _high;

        public static UInt128 MinValue => s_minValue;

        public static UInt128 One => s_one;

        public static UInt128 MaxValue => s_maxValue;

        public UInt128(int value)
        {
            throw new NotImplementedException();
        }

        public UInt128(uint value)
        {
            _low = value;
            _high = 0;
        }

        public UInt128(long value)
        {
            throw new NotImplementedException();
        }

        public UInt128(ulong value)
        {
            _low = value;
            _high = 0;
        }

        public UInt128(float value)
        {
            throw new NotImplementedException();
        }

        public UInt128(double value)
        {
            throw new NotImplementedException();
        }

        public UInt128(decimal value)
        {
            throw new NotImplementedException();
        }

        public UInt128(byte[] value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            throw new NotImplementedException();
        }

        private UInt128(ulong high, ulong low)
        {
            _low = low;
            _high = high;
        }

        public override bool Equals(object obj) => obj is UInt128 && Equals((UInt128)obj);

        public override int GetHashCode() => CombineHash(_high.GetHashCode(), _low.GetHashCode());

        public bool Equals(UInt128 other) => this == other;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (!(obj is UInt128))
                throw new ArgumentException(
                    string.Format(Culture, ArgumentMustBeOfType, typeof(UInt128).Name),
                    nameof(obj));

            return Compare(this, (UInt128)obj);
        }

        public int CompareTo(UInt128 other) => Compare(this, other);

        public static int Compare(UInt128 left, UInt128 right)
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

        public static UInt128 Add(UInt128 left, UInt128 right)
        {
            ulong low = unchecked(left._high + right._high);
            ulong high = left._high + right._high;

            if (low < left._low)
                high++;

            return new UInt128(high, low);
        }

        public static UInt128 Subtract(UInt128 left, UInt128 right)
        {
            ulong low = unchecked(left._high - right._high);
            ulong high = left._high - right._high;

            if (left._low < right._low)
                high--;

            return new UInt128(high, low);
        }

        public static UInt128 Multiply(UInt128 left, UInt128 right)
        {
            UInt128 result = Multiply(left._low, right._low);
            result._high += left._high * right._low + left._low * right._high;
            return result;
        }

        private static UInt128 Multiply(ulong left, ulong right)
        {
            throw new NotImplementedException();
        }

        public static UInt128 Divide(UInt128 dividend, UInt128 divisor)
        {
            UInt128 remainder;
            return DivRem(dividend, divisor, out remainder);
        }

        public static UInt128 Remainder(UInt128 dividend, UInt128 divisor)
        {
            UInt128 remainder;
            DivRem(dividend, divisor, out remainder);
            return remainder;
        }

        public static UInt128 DivRem(UInt128 dividend, UInt128 divisor, out UInt128 remainder)
        {
            throw new NotImplementedException();
        }

        #region Operators

        public static implicit operator UInt128(byte value) => new UInt128(value);

        public static implicit operator UInt128(sbyte value) => new UInt128(value);

        public static implicit operator UInt128(short value) => new UInt128(value);

        public static implicit operator UInt128(ushort value) => new UInt128(value);

        public static implicit operator UInt128(int value) => new UInt128(value);

        public static implicit operator UInt128(uint value) => new UInt128(value);

        public static implicit operator UInt128(long value) => new UInt128(value);

        public static implicit operator UInt128(ulong value) => new UInt128(value);

        public static explicit operator UInt128(float value) => new UInt128(value);

        public static explicit operator UInt128(double value) => new UInt128(value);

        public static explicit operator UInt128(decimal value) => new UInt128(value);

        public static explicit operator byte(UInt128 value) => checked((byte)(int)value);

        public static explicit operator sbyte(UInt128 value) => checked((sbyte)(int)value);

        public static explicit operator short(UInt128 value) => checked((short)(int)value);

        public static explicit operator ushort(UInt128 value) => checked((ushort)(int)value);

        public static explicit operator int(UInt128 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator uint(UInt128 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator long(UInt128 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator ulong(UInt128 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator float(UInt128 value) => (float)(double)value;

        public static explicit operator double(UInt128 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator decimal(UInt128 value)
        {
            throw new NotImplementedException();
        }

        public static UInt128 operator &(UInt128 left, UInt128 right)
            => new UInt128(left._high & right._high, left._low & right._low);

        public static UInt128 operator |(UInt128 left, UInt128 right)
            => new UInt128(left._high | right._high, left._low | right._low);

        public static UInt128 operator ^(UInt128 left, UInt128 right)
            => new UInt128(left._high ^ right._high, left._low ^ right._low);

        public static UInt128 operator <<(UInt128 value, int shift)
        {
            if (shift == 0)
                return value;

            if (shift < 0)
                return value >> -shift;

            shift %= 128;

            ulong lowShiftedToHigh = value._low << (shift - 64);

            return (shift > 63) ?
                new UInt128(lowShiftedToHigh, 0) :
                new UInt128(lowShiftedToHigh | (value._high << shift), value._low << shift);
        }

        public static UInt128 operator >>(UInt128 value, int shift)
        {
            if (shift == 0)
                return value;

            if (shift < 0)
                return value << -shift;

            shift %= 128;

            ulong highShiftedToLow = value._high >> (shift - 64);

            return (shift > 63) ?
                new UInt128(0, highShiftedToLow) :
                new UInt128(value._high >> shift, highShiftedToLow | (value._low >> shift));
        }

        public static UInt128 operator ~(UInt128 value) => new UInt128(~value._high, ~value._high);

        public static bool operator ==(UInt128 left, UInt128 right)
            => left._low == right._low && left._high == right._high;

        public static bool operator !=(UInt128 left, UInt128 right)
            => left._low != right._low || left._high != right._high;

        public static UInt128 operator ++(UInt128 value) => Add(value, s_one);

        public static UInt128 operator --(UInt128 value) => Subtract(value, s_one);

        public static UInt128 operator *(UInt128 left, UInt128 right) => Multiply(left, right);

        public static UInt128 operator /(UInt128 dividend, UInt128 divisor) => Divide(dividend, divisor);

        public static UInt128 operator %(UInt128 dividend, UInt128 divisor) => Remainder(dividend, divisor);

        public static bool operator >(UInt128 left, UInt128 right) => left.CompareTo(right) > 0;

        public static bool operator <(UInt128 left, UInt128 right) => left.CompareTo(right) < 0;

        public static bool operator >=(UInt128 left, UInt128 right) => left.CompareTo(right) >= 0;

        public static bool operator <=(UInt128 left, UInt128 right) => left.CompareTo(right) <= 0;

        public static UInt128 operator +(UInt128 left, UInt128 right) => Add(left, right);

        public static UInt128 operator -(UInt128 left, UInt128 right) => Subtract(left, right);

        #endregion
    }
}
