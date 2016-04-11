﻿using System;
using System.Security.Cryptography;

namespace Gost.Security.Cryptography
{
    using System.Runtime.CompilerServices;
    using static Buffer;
    using static CryptoUtils;

    internal sealed class GrasshopperManagedTransform : SymmetricTransform
    {
        #region Constants

        private static readonly byte[]
            s_multiplicationTableConstants = { 148, 32, 133, 16, 194, 192, 1, 251, 1, 192, 194, 16, 133, 32, 148, 1 },
            s_forwardSubstitutionBox =
            {
                0xFC, 0xEE, 0xDD, 0x11, 0xCF, 0x6E, 0x31, 0x16, 0xFB, 0xC4, 0xFA, 0xDA, 0x23, 0xC5, 0x04, 0x4D,
                0xE9, 0x77, 0xF0, 0xDB, 0x93, 0x2E, 0x99, 0xBA, 0x17, 0x36, 0xF1, 0xBB, 0x14, 0xCD, 0x5F, 0xC1,
                0xF9, 0x18, 0x65, 0x5A, 0xE2, 0x5C, 0xEF, 0x21, 0x81, 0x1C, 0x3C, 0x42, 0x8B, 0x01, 0x8E, 0x4F,
                0x05, 0x84, 0x02, 0xAE, 0xE3, 0x6A, 0x8F, 0xA0, 0x06, 0x0B, 0xED, 0x98, 0x7F, 0xD4, 0xD3, 0x1F,
                0xEB, 0x34, 0x2C, 0x51, 0xEA, 0xC8, 0x48, 0xAB, 0xF2, 0x2A, 0x68, 0xA2, 0xFD, 0x3A, 0xCE, 0xCC,
                0xB5, 0x70, 0x0E, 0x56, 0x08, 0x0C, 0x76, 0x12, 0xBF, 0x72, 0x13, 0x47, 0x9C, 0xB7, 0x5D, 0x87,
                0x15, 0xA1, 0x96, 0x29, 0x10, 0x7B, 0x9A, 0xC7, 0xF3, 0x91, 0x78, 0x6F, 0x9D, 0x9E, 0xB2, 0xB1,
                0x32, 0x75, 0x19, 0x3D, 0xFF, 0x35, 0x8A, 0x7E, 0x6D, 0x54, 0xC6, 0x80, 0xC3, 0xBD, 0x0D, 0x57,
                0xDF, 0xF5, 0x24, 0xA9, 0x3E, 0xA8, 0x43, 0xC9, 0xD7, 0x79, 0xD6, 0xF6, 0x7C, 0x22, 0xB9, 0x03,
                0xE0, 0x0F, 0xEC, 0xDE, 0x7A, 0x94, 0xB0, 0xBC, 0xDC, 0xE8, 0x28, 0x50, 0x4E, 0x33, 0x0A, 0x4A,
                0xA7, 0x97, 0x60, 0x73, 0x1E, 0x00, 0x62, 0x44, 0x1A, 0xB8, 0x38, 0x82, 0x64, 0x9F, 0x26, 0x41,
                0xAD, 0x45, 0x46, 0x92, 0x27, 0x5E, 0x55, 0x2F, 0x8C, 0xA3, 0xA5, 0x7D, 0x69, 0xD5, 0x95, 0x3B,
                0x07, 0x58, 0xB3, 0x40, 0x86, 0xAC, 0x1D, 0xF7, 0x30, 0x37, 0x6B, 0xE4, 0x88, 0xD9, 0xE7, 0x89,
                0xE1, 0x1B, 0x83, 0x49, 0x4C, 0x3F, 0xF8, 0xFE, 0x8D, 0x53, 0xAA, 0x90, 0xCA, 0xD8, 0x85, 0x61,
                0x20, 0x71, 0x67, 0xA4, 0x2D, 0x2B, 0x09, 0x5B, 0xCB, 0x9B, 0x25, 0xD0, 0xBE, 0xE5, 0x6C, 0x52,
                0x59, 0xA6, 0x74, 0xD2, 0xE6, 0xF4, 0xB4, 0xC0, 0xD1, 0x66, 0xAF, 0xC2, 0x39, 0x4B, 0x63, 0xB6,
            },
            s_backwardSubstitutionBox =
            {
                0xA5, 0x2D, 0x32, 0x8F, 0x0E, 0x30, 0x38, 0xC0, 0x54, 0xE6, 0x9E, 0x39, 0x55, 0x7E, 0x52, 0x91,
                0x64, 0x03, 0x57, 0x5A, 0x1C, 0x60, 0x07, 0x18, 0x21, 0x72, 0xA8, 0xD1, 0x29, 0xC6, 0xA4, 0x3F,
                0xE0, 0x27, 0x8D, 0x0C, 0x82, 0xEA, 0xAE, 0xB4, 0x9A, 0x63, 0x49, 0xE5, 0x42, 0xE4, 0x15, 0xB7,
                0xC8, 0x06, 0x70, 0x9D, 0x41, 0x75, 0x19, 0xC9, 0xAA, 0xFC, 0x4D, 0xBF, 0x2A, 0x73, 0x84, 0xD5,
                0xC3, 0xAF, 0x2B, 0x86, 0xA7, 0xB1, 0xB2, 0x5B, 0x46, 0xD3, 0x9F, 0xFD, 0xD4, 0x0F, 0x9C, 0x2F,
                0x9B, 0x43, 0xEF, 0xD9, 0x79, 0xB6, 0x53, 0x7F, 0xC1, 0xF0, 0x23, 0xE7, 0x25, 0x5E, 0xB5, 0x1E,
                0xA2, 0xDF, 0xA6, 0xFE, 0xAC, 0x22, 0xF9, 0xE2, 0x4A, 0xBC, 0x35, 0xCA, 0xEE, 0x78, 0x05, 0x6B,
                0x51, 0xE1, 0x59, 0xA3, 0xF2, 0x71, 0x56, 0x11, 0x6A, 0x89, 0x94, 0x65, 0x8C, 0xBB, 0x77, 0x3C,
                0x7B, 0x28, 0xAB, 0xD2, 0x31, 0xDE, 0xC4, 0x5F, 0xCC, 0xCF, 0x76, 0x2C, 0xB8, 0xD8, 0x2E, 0x36,
                0xDB, 0x69, 0xB3, 0x14, 0x95, 0xBE, 0x62, 0xA1, 0x3B, 0x16, 0x66, 0xE9, 0x5C, 0x6C, 0x6D, 0xAD,
                0x37, 0x61, 0x4B, 0xB9, 0xE3, 0xBA, 0xF1, 0xA0, 0x85, 0x83, 0xDA, 0x47, 0xC5, 0xB0, 0x33, 0xFA,
                0x96, 0x6F, 0x6E, 0xC2, 0xF6, 0x50, 0xFF, 0x5D, 0xA9, 0x8E, 0x17, 0x1B, 0x97, 0x7D, 0xEC, 0x58,
                0xF7, 0x1F, 0xFB, 0x7C, 0x09, 0x0D, 0x7A, 0x67, 0x45, 0x87, 0xDC, 0xE8, 0x4F, 0x1D, 0x4E, 0x04,
                0xEB, 0xF8, 0xF3, 0x3E, 0x3D, 0xBD, 0x8A, 0x88, 0xDD, 0xCD, 0x0B, 0x13, 0x98, 0x02, 0x93, 0x80,
                0x90, 0xD0, 0x24, 0x34, 0xCB, 0xED, 0xF4, 0xCE, 0x99, 0x10, 0x44, 0x40, 0x92, 0x3A, 0x01, 0x26,
                0x12, 0x1A, 0x48, 0x68, 0xF5, 0x81, 0x8B, 0xC7, 0xD6, 0x20, 0x0A, 0x08, 0x00, 0x4C, 0xD7, 0x74,
            };

        #endregion

        private static readonly byte[][] s_multiplicationTable = InitializeMultiplicationTable();

        private static readonly byte[][][] s_iterationConstants = InitializeIterationConstants();

        private byte[] _keyExpansion;

        public GrasshopperManagedTransform(
            byte[] rgbKey,
            byte[] rgbIV,
            int blockSize,
            int feedbackSize,
            CipherMode cipherMode,
            PaddingMode paddingMode,
            SymmetricTransformMode transformMode)
            : base(rgbKey, rgbIV, blockSize, cipherMode, paddingMode, transformMode)
        { }

        protected override void GenerateKeyExpansion(byte[] rgbKey)
        {
            _keyExpansion = new byte[160];

            BlockCopy(rgbKey, 0, _keyExpansion, 0, 16);
            BlockCopy(rgbKey, 16, _keyExpansion, 16, 16);

            byte[] temp = new byte[16];

            for (int i = 0; i < 4; i++)
            {
                int expansionPartOffset = (i + 1) * 32;
                BlockCopy(_keyExpansion, i * 32, _keyExpansion, expansionPartOffset, 32);

                for (int j = 0; j < 8; j++)
                {
                    Xor(s_iterationConstants[i][j], 0, _keyExpansion, expansionPartOffset, temp, 0);
                    Substitute(s_forwardSubstitutionBox, temp, 0);
                    DoLinearTransformForward(temp, 0);
                    Xor(temp, 0, _keyExpansion, expansionPartOffset + 16, temp, 0);

                    BlockCopy(_keyExpansion, expansionPartOffset, _keyExpansion, expansionPartOffset + 16, 16);
                    BlockCopy(temp, 0, _keyExpansion, expansionPartOffset, 16);
                }
            }
            Array.Clear(temp, 0, temp.Length);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                EraseData(ref _keyExpansion);
            }

            base.Dispose(disposing);
        }

        protected override void EncryptBlock(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, 16);

            for (int i = 0; i < 9; i++)
            {
                Xor(outputBuffer, outputOffset, _keyExpansion, 16 * i, outputBuffer, outputOffset);
                Substitute(s_forwardSubstitutionBox, outputBuffer, outputOffset);
                DoLinearTransformForward(outputBuffer, outputOffset);
            }
            Xor(outputBuffer, outputOffset, _keyExpansion, 16 * 9, outputBuffer, outputOffset);
        }

        protected override void DecryptBlock(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
        {
            BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, 16);

            for (int i = 0; i < 9; i++)
            {
                Xor(outputBuffer, outputOffset, _keyExpansion, (9 - i) * 16, outputBuffer, outputOffset);
                DoLinearTransformBackward(outputBuffer, outputOffset);
                Substitute(s_backwardSubstitutionBox, outputBuffer, outputOffset);
            }
            Xor(outputBuffer, outputOffset, _keyExpansion, 0, outputBuffer, outputOffset);
        }

        private static void Xor(byte[] left, int leftOffset, byte[] right, int rightOffset, byte[] output, int outputOffset)
            => CryptoUtils.Xor(left, leftOffset, right, rightOffset, output, outputOffset, 16);

        private static void Substitute(byte[] substTable, byte[] data, int dataOffset)
        {
            for (int i = 0; i < 16; i++)
                data[dataOffset + i] = substTable[data[dataOffset + i]];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DoLinearTransformForward(byte[] data, int dataOffset)
        {
            for (int i = 0; i < 16; i++)
            {
                byte sum = 0;

                for (int j = 0; j < 16; j++)
                    sum ^= s_multiplicationTable[j][data[dataOffset + j]];

                BlockCopy(data, dataOffset, data, dataOffset + 1, 15);
                data[dataOffset] = sum;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DoLinearTransformBackward(byte[] data, int dataOffset)
        {
            for (int i = 0; i < 16; i++)
            {
                byte indata0 = data[dataOffset];

                BlockCopy(data, dataOffset + 1, data, dataOffset, 15);
                data[dataOffset + 15] = indata0;

                byte sum = 0;

                for (int j = 0; j < 16; j++)
                    sum ^= s_multiplicationTable[j][data[dataOffset + j]];

                data[dataOffset + 15] = sum;
            }
        }

        private static byte[][][] InitializeIterationConstants()
        {
            byte[][][] retval = new byte[4][][];
            for (int i = 0; i < 4; i++)
            {
                byte[][] row = new byte[8][];

                for (int j = 0; j < 8; j++)
                {
                    byte[] iterConst = new byte[16];
                    iterConst[15] = (byte)(i * 8 + j + 1); ;
                    DoLinearTransformForward(iterConst, 0);
                    row[j] = iterConst;
                }

                retval[i] = row;
            }
            return retval;
        }

        private static byte[][] InitializeMultiplicationTable()
        {
            byte[][] table = new byte[16][];
            for (int i = 0; i < 16; i++)
            {
                byte[] row = new byte[256];
                for (int j = 0; j < 256; j++)
                {
                    int x = j;
                    int z = 0;
                    int y = s_multiplicationTableConstants[i];

                    while (y != 0)
                    {
                        if ((y & 1) != 0)
                            z ^= x;
                        x = x << 1 ^ (((x & 0x80) != 0) ? 0xC3 : 0x00);
                        y >>= 1;
                    }

                    row[j] = (byte)z;
                }
                table[i] = row;
            }
            return table;
        }
    }
}
