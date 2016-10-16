using System;

namespace OpenGost.Security.Cryptography
{
    using static CryptoUtils;
    using static SecurityCryptographyStrings;

    internal struct UInt512 : IComparable, IComparable<UInt512>, IEquatable<UInt512>, IFormattable
    {
        private static readonly UInt512 s_minValue = new UInt512(UInt256.MinValue, UInt256.MinValue);
        private static readonly UInt512 s_one = new UInt512(UInt256.MinValue, UInt256.One);
        private static readonly UInt512 s_maxValue = new UInt512(UInt256.MaxValue, UInt256.MaxValue);

        private UInt256 _low;
        private UInt256 _high;

        public static UInt512 MinValue => s_minValue;

        public static UInt512 One => s_one;

        public static UInt512 MaxValue => s_maxValue;

        public UInt512(int value)
        {
            throw new NotImplementedException();
        }

        public UInt512(uint value)
        {
            _low = value;
            _high = 0;
        }

        public UInt512(long value)
        {
            throw new NotImplementedException();
        }

        public UInt512(ulong value)
        {
            _low = value;
            _high = 0;
        }

        public UInt512(UInt128 value)
        {
            _low = value;
            _high = 0;
        }

        public UInt512(UInt256 value)
        {
            _low = value;
            _high = 0;
        }

        public UInt512(float value)
        {
            throw new NotImplementedException();
        }

        public UInt512(double value)
        {
            throw new NotImplementedException();
        }

        public UInt512(decimal value)
        {
            throw new NotImplementedException();
        }

        public UInt512(byte[] value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            throw new NotImplementedException();
        }

        private UInt512(UInt256 high, UInt256 low)
        {
            _low = low;
            _high = high;
        }

        public override bool Equals(object obj) => obj is UInt512 && Equals((UInt512)obj);

        public override int GetHashCode() => CombineHash(_high.GetHashCode(), _low.GetHashCode());

        public bool Equals(UInt512 other) => this == other;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (!(obj is UInt512))
                throw new ArgumentException(
                    string.Format(Culture, ArgumentMustBeOfType, typeof(UInt512).Name),
                    nameof(obj));

            return Compare(this, (UInt512)obj);
        }

        public int CompareTo(UInt512 other) => Compare(this, other);

        public static int Compare(UInt512 left, UInt512 right)
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

        public static UInt512 Add(UInt512 left, UInt512 right)
        {
            UInt256 low = unchecked(left._high + right._high);
            UInt256 high = left._high + right._high;

            if (low < left._low)
                high++;

            return new UInt512(high, low);
        }

        public static UInt512 Subtract(UInt512 left, UInt512 right)
        {
            UInt256 low = unchecked(left._high - right._high);
            UInt256 high = left._high - right._high;

            if (left._low < right._low)
                high--;

            return new UInt512(high, low);
        }

        public static UInt512 Multiply(UInt512 left, UInt512 right)
        {
            UInt512 result = Multiply(left._low, right._low);
            result._high += left._high * right._low + left._low * right._high;
            return result;
        }

        private static UInt512 Multiply(UInt256 left, UInt256 right)
        {
            throw new NotImplementedException();
        }

        public static UInt512 Divide(UInt512 dividend, UInt512 divisor)
        {
            UInt512 remainder;
            return DivRem(dividend, divisor, out remainder);
        }

        public static UInt512 Remainder(UInt512 dividend, UInt512 divisor)
        {
            UInt512 remainder;
            DivRem(dividend, divisor, out remainder);
            return remainder;
        }

        public static UInt512 DivRem(UInt512 dividend, UInt512 divisor, out UInt512 remainder)
        {
            throw new NotImplementedException();
        }

        #region Operators

        public static implicit operator UInt512(byte value) => new UInt512(value);

        public static implicit operator UInt512(sbyte value) => new UInt512(value);

        public static implicit operator UInt512(short value) => new UInt512(value);

        public static implicit operator UInt512(ushort value) => new UInt512(value);

        public static implicit operator UInt512(int value) => new UInt512(value);

        public static implicit operator UInt512(uint value) => new UInt512(value);

        public static implicit operator UInt512(long value) => new UInt512(value);

        public static implicit operator UInt512(ulong value) => new UInt512(value);

        public static implicit operator UInt512(UInt128 value) => new UInt512(value);

        public static implicit operator UInt512(UInt256 value) => new UInt512(value);

        public static explicit operator UInt512(float value) => new UInt512(value);

        public static explicit operator UInt512(double value) => new UInt512(value);

        public static explicit operator UInt512(decimal value) => new UInt512(value);

        public static explicit operator byte(UInt512 value) => checked((byte)(int)value);

        public static explicit operator sbyte(UInt512 value) => checked((sbyte)(int)value);

        public static explicit operator short(UInt512 value) => checked((short)(int)value);

        public static explicit operator ushort(UInt512 value) => checked((ushort)(int)value);

        public static explicit operator int(UInt512 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator uint(UInt512 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator long(UInt512 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator ulong(UInt512 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator UInt128(UInt512 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator UInt256(UInt512 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator float(UInt512 value) => (float)(double)value;

        public static explicit operator double(UInt512 value)
        {
            throw new NotImplementedException();
        }

        public static explicit operator decimal(UInt512 value)
        {
            throw new NotImplementedException();
        }

        public static UInt512 operator &(UInt512 left, UInt512 right)
            => new UInt512(left._high & right._high, left._low & right._low);

        public static UInt512 operator |(UInt512 left, UInt512 right)
            => new UInt512(left._high | right._high, left._low | right._low);

        public static UInt512 operator ^(UInt512 left, UInt512 right)
            => new UInt512(left._high ^ right._high, left._low ^ right._low);

        public static UInt512 operator <<(UInt512 value, int shift)
        {
            if (shift == 0)
                return value;

            if (shift < 0)
                return value >> -shift;

            shift %= 512;

            UInt256 lowShiftedToHigh = value._low << (shift - 256);

            return (shift > 255) ?
                new UInt512(lowShiftedToHigh, 0) :
                new UInt512(lowShiftedToHigh | (value._high << shift), value._low << shift);
        }

        public static UInt512 operator >>(UInt512 value, int shift)
        {
            if (shift == 0)
                return value;

            if (shift < 0)
                return value << -shift;

            shift %= 512;

            UInt256 highShiftedToLow = value._high >> (shift - 256);

            return (shift > 255) ?
                new UInt512(0, highShiftedToLow) :
                new UInt512(value._high >> shift, highShiftedToLow | (value._low >> shift));
        }

        public static UInt512 operator ~(UInt512 value) => new UInt512(~value._high, ~value._high);

        public static bool operator ==(UInt512 left, UInt512 right)
            => left._low == right._low && left._high == right._high;

        public static bool operator !=(UInt512 left, UInt512 right)
            => left._low != right._low || left._high != right._high;

        public static UInt512 operator ++(UInt512 value) => Add(value, s_one);

        public static UInt512 operator --(UInt512 value) => Subtract(value, s_one);

        public static UInt512 operator *(UInt512 left, UInt512 right) => Multiply(left, right);

        public static UInt512 operator /(UInt512 dividend, UInt512 divisor) => Divide(dividend, divisor);

        public static UInt512 operator %(UInt512 dividend, UInt512 divisor) => Remainder(dividend, divisor);

        public static bool operator >(UInt512 left, UInt512 right) => left.CompareTo(right) > 0;

        public static bool operator <(UInt512 left, UInt512 right) => left.CompareTo(right) < 0;

        public static bool operator >=(UInt512 left, UInt512 right) => left.CompareTo(right) >= 0;

        public static bool operator <=(UInt512 left, UInt512 right) => left.CompareTo(right) <= 0;

        public static UInt512 operator +(UInt512 left, UInt512 right) => Add(left, right);

        public static UInt512 operator -(UInt512 left, UInt512 right) => Subtract(left, right);

        #endregion
    }
}
