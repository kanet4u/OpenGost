using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security;

namespace OpenGost.Security.Cryptography
{
    internal static class NumericsHelpers
    {
        internal static void Xor(byte[] left, int leftOffset, byte[] right, int rightOffset, byte[] result, int resultOffset, int count)
        {
            for (int i = 0; i < count; i++)
                result[resultOffset + i] = (byte)(left[leftOffset + i] ^ right[rightOffset + i]);
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void UInt64ToLittleEndian(byte* block, ulong* x, int digits)
        {
            for (int i = 0, j = 0; i < digits; i++, j += sizeof(ulong))
                UInt64ToLittleEndian(block + j, x[i]);
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void UInt64ToLittleEndian(byte* block, ulong value)
        {
            *block = (byte)value;
            block[1] = (byte)(value >> 8);
            block[2] = (byte)(value >> 16);
            block[3] = (byte)(value >> 24);
            block[4] = (byte)(value >> 32);
            block[5] = (byte)(value >> 40);
            block[6] = (byte)(value >> 48);
            block[7] = (byte)(value >> 56);
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void UInt32ToBigEndian(byte* block, uint* x, int digits)
        {
            for (int i = 0, j = 0; i < digits; i++, j += sizeof(uint))
                UInt32ToBigEndian(block + j, x[i]);
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void UInt32ToBigEndian(byte* block, uint value)
        {
            *block = (byte)(value >> 24);
            block[1] = (byte)(value >> 16);
            block[2] = (byte)(value >> 8);
            block[3] = (byte)value;
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe static void UInt32FromBigEndian(uint* x, int digits, byte* block)
        {
            for (int i = 0, j = 0; i < digits; i++, j += sizeof(uint))
                x[i] = UInt32FromBigEndian(block + j);
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe uint UInt32FromBigEndian(byte* block)
            => (uint)(*block << 24) | (uint)(block[1] << 16) | (uint)(block[2] << 8) | (block[3]);

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ulong UInt64FromLittleEndian(byte[] input, int offset)
        {
            unsafe
            {
                fixed (byte* block = input)
                    return UInt64FromLittleEndian(block + offset);
            }
        }

        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe ulong UInt64FromLittleEndian(byte* block)
            => *block | (uint)(block[1] << 8) | (uint)(block[2] << 16) | (uint)(block[3] << 24) |
                (uint)(block[4] << 32) | (uint)(block[5] << 40) | (uint)(block[6] << 48) | (uint)(block[7] << 56);

        internal static byte[] ToNormalizedByteArray(BigInteger value, int size)
        {
            if (value < BigInteger.Zero)
                value += (BigInteger.One << size * 8);

            byte[] result = new byte[size];
            for (int i = 0; i < size; i++)
            {
                if (value == BigInteger.Zero)
                    break;
                result[i] = (byte)(value % 256);
                value >>= 8;
            }

            return result;
        }
    }
}
