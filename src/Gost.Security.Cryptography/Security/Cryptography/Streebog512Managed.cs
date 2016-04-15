﻿using System;

namespace Gost.Security.Cryptography
{
    using static Buffer;
    using static CryptoUtils;

    /// <summary>
    /// Computes the <see cref="Streebog512"/> hash for the input data using the managed implementation. 
    /// </summary>
    public class Streebog512Managed : Streebog512
    {
        #region Constants

        private static readonly byte[][] s_keyExpansionTable =
        {
            new byte[]
            {
                0x07, 0x45, 0xa6, 0xf2, 0x59, 0x65, 0x80, 0xdd, 0x23, 0x4d, 0x74, 0xcc, 0x36, 0x74, 0x76, 0x05,
                0x15, 0xd3, 0x60, 0xa4, 0x08, 0x2a, 0x42, 0xa2, 0x01, 0x69, 0x67, 0x92, 0x91, 0xe0, 0x7c, 0x4b,
                0xfc, 0xc4, 0x85, 0x75, 0x8d, 0xb8, 0x4e, 0x71, 0x16, 0xd0, 0x45, 0x2e, 0x43, 0x76, 0x6a, 0x2f,
                0x1f, 0x7c, 0x65, 0xc0, 0x81, 0x2f, 0xcb, 0xeb, 0xe9, 0xda, 0xca, 0x1e, 0xda, 0x5b, 0x08, 0xb1,
            },
            new byte[]
            {
                0xb7, 0x9b, 0xb1, 0x21, 0x70, 0x04, 0x79, 0xe6, 0x56, 0xcd, 0xcb, 0xd7, 0x1b, 0xa2, 0xdd, 0x55,
                0xca, 0xa7, 0x0a, 0xdb, 0xc2, 0x61, 0xb5, 0x5c, 0x58, 0x99, 0xd6, 0x12, 0x6b, 0x17, 0xb5, 0x9a,
                0x31, 0x01, 0xb5, 0x16, 0x0f, 0x5e, 0xd5, 0x61, 0x98, 0x2b, 0x23, 0x0a, 0x72, 0xea, 0xfe, 0xf3,
                0xd7, 0xb5, 0x70, 0x0f, 0x46, 0x9d, 0xe3, 0x4f, 0x1a, 0x2f, 0x9d, 0xa9, 0x8a, 0xb5, 0xa3, 0x6f,
            },
            new byte[]
            {
                0xb2, 0x0a, 0xba, 0x0a, 0xf5, 0x96, 0x1e, 0x99, 0x31, 0xdb, 0x7a, 0x86, 0x43, 0xf4, 0xb6, 0xc2,
                0x09, 0xdb, 0x62, 0x60, 0x37, 0x3a, 0xc9, 0xc1, 0xb1, 0x9e, 0x35, 0x90, 0xe4, 0x0f, 0xe2, 0xd3,
                0x7b, 0x7b, 0x29, 0xb1, 0x14, 0x75, 0xea, 0xf2, 0x8b, 0x1f, 0x9c, 0x52, 0x5f, 0x5e, 0xf1, 0x06,
                0x35, 0x84, 0x3d, 0x6a, 0x28, 0xfc, 0x39, 0x0a, 0xc7, 0x2f, 0xce, 0x2b, 0xac, 0xdc, 0x74, 0xf5,
            },
            new byte[]
            {
                0x2e, 0xd1, 0xe3, 0x84, 0xbc, 0xbe, 0x0c, 0x22, 0xf1, 0x37, 0xe8, 0x93, 0xa1, 0xea, 0x53, 0x34,
                0xbe, 0x03, 0x52, 0x93, 0x33, 0x13, 0xb7, 0xd8, 0x75, 0xd6, 0x03, 0xed, 0x82, 0x2c, 0xd7, 0xa9,
                0x3f, 0x35, 0x5e, 0x68, 0xad, 0x1c, 0x72, 0x9d, 0x7d, 0x3c, 0x5c, 0x33, 0x7e, 0x85, 0x8e, 0x48,
                0xdd, 0xe4, 0x71, 0x5d, 0xa0, 0xe1, 0x48, 0xf9, 0xd2, 0x66, 0x15, 0xe8, 0xb3, 0xdf, 0x1f, 0xef,
            },
            new byte[]
            {
                0x57, 0xfe, 0x6c, 0x7c, 0xfd, 0x58, 0x17, 0x60, 0xf5, 0x63, 0xea, 0xa9, 0x7e, 0xa2, 0x56, 0x7a,
                0x16, 0x1a, 0x27, 0x23, 0xb7, 0x00, 0xff, 0xdf, 0xa3, 0xf5, 0x3a, 0x25, 0x47, 0x17, 0xcd, 0xbf,
                0xbd, 0xff, 0x0f, 0x80, 0xd7, 0x35, 0x9e, 0x35, 0x4a, 0x10, 0x86, 0x16, 0x1f, 0x1c, 0x15, 0x7f,
                0x63, 0x23, 0xa9, 0x6c, 0x0c, 0x41, 0x3f, 0x9a, 0x99, 0x47, 0x47, 0xad, 0xac, 0x6b, 0xea, 0x4b,
            },
            new byte[]
            {
                0x6e, 0x7d, 0x64, 0x46, 0x7a, 0x40, 0x68, 0xfa, 0x35, 0x4f, 0x90, 0x36, 0x72, 0xc5, 0x71, 0xbf,
                0xb6, 0xc6, 0xbe, 0xc2, 0x66, 0x1f, 0xf2, 0x0a, 0xb4, 0xb7, 0x9a, 0x1c, 0xb7, 0xa6, 0xfa, 0xcf,
                0xc6, 0x8e, 0xf0, 0x9a, 0xb4, 0x9a, 0x7f, 0x18, 0x6c, 0xa4, 0x42, 0x51, 0xf9, 0xc4, 0x66, 0x2d,
                0xc0, 0x39, 0x30, 0x7a, 0x3b, 0xc3, 0xa4, 0x6f, 0xd9, 0xd3, 0x3a, 0x1d, 0xae, 0xae, 0x4f, 0xae,
            },
            new byte[]
            {
                0x93, 0xd4, 0x14, 0x3a, 0x4d, 0x56, 0x86, 0x88, 0xf3, 0x4a, 0x3c, 0xa2, 0x4c, 0x45, 0x17, 0x35,
                0x04, 0x05, 0x4a, 0x28, 0x83, 0x69, 0x47, 0x06, 0x37, 0x2c, 0x82, 0x2d, 0xc5, 0xab, 0x92, 0x09,
                0xc9, 0x93, 0x7a, 0x19, 0x33, 0x3e, 0x47, 0xd3, 0xc9, 0x87, 0xbf, 0xe6, 0xc7, 0xc6, 0x9e, 0x39,
                0x54, 0x09, 0x24, 0xbf, 0xfe, 0x86, 0xac, 0x51, 0xec, 0xc5, 0xaa, 0xee, 0x16, 0x0e, 0xc7, 0xf4,
            },
            new byte[]
            {
                0x1e, 0xe7, 0x02, 0xbf, 0xd4, 0x0d, 0x7f, 0xa4, 0xd9, 0xa8, 0x51, 0x59, 0x35, 0xc2, 0xac, 0x36,
                0x2f, 0xc4, 0xa5, 0xd1, 0x2b, 0x8d, 0xd1, 0x69, 0x90, 0x06, 0x9b, 0x92, 0xcb, 0x2b, 0x89, 0xf4,
                0x9a, 0xc4, 0xdb, 0x4d, 0x3b, 0x44, 0xb4, 0x89, 0x1e, 0xde, 0x36, 0x9c, 0x71, 0xf8, 0xb7, 0x4e,
                0x41, 0x41, 0x6e, 0x0c, 0x02, 0xaa, 0xe7, 0x03, 0xa7, 0xc9, 0x93, 0x4d, 0x42, 0x5b, 0x1f, 0x9b,
            },
            new byte[]
            {
                0xdb, 0x5a, 0x23, 0x83, 0x51, 0x44, 0x61, 0x72, 0x60, 0x2a, 0x1f, 0xcb, 0x92, 0xdc, 0x38, 0x0e,
                0x54, 0x9c, 0x07, 0xa6, 0x9a, 0x8a, 0x2b, 0x7b, 0xb1, 0xce, 0xb2, 0xdb, 0x0b, 0x44, 0x0a, 0x80,
                0x84, 0x09, 0x0d, 0xe0, 0xb7, 0x55, 0xd9, 0x3c, 0x24, 0x42, 0x89, 0x25, 0x1b, 0x3a, 0x7d, 0x3a,
                0xde, 0x5f, 0x16, 0xec, 0xd8, 0x9a, 0x4c, 0x94, 0x9b, 0x22, 0x31, 0x16, 0x54, 0x5a, 0x8f, 0x37,
            },
            new byte[]
            {
                0xed, 0x9c, 0x45, 0x98, 0xfb, 0xc7, 0xb4, 0x74, 0xc3, 0xb6, 0x3b, 0x15, 0xd1, 0xfa, 0x98, 0x36,
                0xf4, 0x52, 0x76, 0x3b, 0x30, 0x6c, 0x1e, 0x7a, 0x4b, 0x33, 0x69, 0xaf, 0x02, 0x67, 0xe7, 0x9f,
                0x03, 0x61, 0x33, 0x1b, 0x8a, 0xe1, 0xff, 0x1f, 0xdb, 0x78, 0x8a, 0xff, 0x1c, 0xe7, 0x41, 0x89,
                0xf3, 0xf3, 0xe4, 0xb2, 0x48, 0xe5, 0x2a, 0x38, 0x52, 0x6f, 0x05, 0x80, 0xa6, 0xde, 0xbe, 0xab,
            },
            new byte[]
            {
                0x1b, 0x2d, 0xf3, 0x81, 0xcd, 0xa4, 0xca, 0x6b, 0x5d, 0xd8, 0x6f, 0xc0, 0x4a, 0x59, 0xa2, 0xde,
                0x98, 0x6e, 0x47, 0x7d, 0x1d, 0xcd, 0xba, 0xef, 0xca, 0xb9, 0x48, 0xea, 0xef, 0x71, 0x1d, 0x8a,
                0x79, 0x66, 0x84, 0x14, 0x21, 0x80, 0x01, 0x20, 0x61, 0x07, 0xab, 0xeb, 0xbb, 0x6b, 0xfa, 0xd8,
                0x94, 0xfe, 0x5a, 0x63, 0xcd, 0xc6, 0x02, 0x30, 0xfb, 0x89, 0xc8, 0xef, 0xd0, 0x9e, 0xcd, 0x7b,
            },
            new byte[]
            {
                0x20, 0xd7, 0x1b, 0xf1, 0x4a, 0x92, 0xbc, 0x48, 0x99, 0x1b, 0xb2, 0xd9, 0xd5, 0x17, 0xf4, 0xfa,
                0x52, 0x28, 0xe1, 0x88, 0xaa, 0xa4, 0x1d, 0xe7, 0x86, 0xcc, 0x91, 0x18, 0x9d, 0xef, 0x80, 0x5d,
                0x9b, 0x9f, 0x21, 0x30, 0xd4, 0x12, 0x20, 0xf8, 0x77, 0x1d, 0xdf, 0xbc, 0x32, 0x3c, 0xa4, 0xcd,
                0x7a, 0xb1, 0x49, 0x04, 0xb0, 0x80, 0x13, 0xd2, 0xba, 0x31, 0x16, 0xf1, 0x67, 0xe7, 0x8e, 0x37,
            },

        };

        private static readonly byte[]
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
            },
            s_iv = new byte[64];

        private static readonly ulong[] s_linearTransformTable =
        {
            0x8e20faa72ba0b470, 0x47107ddd9b505a38, 0xad08b0e0c3282d1c, 0xd8045870ef14980e,
            0x6c022c38f90a4c07, 0x3601161cf205268d, 0x1b8e0b0e798c13c8, 0x83478b07b2468764,
            0xa011d380818e8f40, 0x5086e740ce47c920, 0x2843fd2067adea10, 0x14aff010bdd87508,
            0x0ad97808d06cb404, 0x05e23c0468365a02, 0x8c711e02341b2d01, 0x46b60f011a83988e,
            0x90dab52a387ae76f, 0x486dd4151c3dfdb9, 0x24b86a840e90f0d2, 0x125c354207487869,
            0x092e94218d243cba, 0x8a174a9ec8121e5d, 0x4585254f64090fa0, 0xaccc9ca9328a8950,
            0x9d4df05d5f661451, 0xc0a878a0a1330aa6, 0x60543c50de970553, 0x302a1e286fc58ca7,
            0x18150f14b9ec46dd, 0x0c84890ad27623e0, 0x0642ca05693b9f70, 0x0321658cba93c138,
            0x86275df09ce8aaa8, 0x439da0784e745554, 0xafc0503c273aa42a, 0xd960281e9d1d5215,
            0xe230140fc0802984, 0x71180a8960409a42, 0xb60c05ca30204d21, 0x5b068c651810a89e,
            0x456c34887a3805b9, 0xac361a443d1c8cd2, 0x561b0d22900e4669, 0x2b838811480723ba,
            0x9bcf4486248d9f5d, 0xc3e9224312c8c1a0, 0xeffa11af0964ee50, 0xf97d86d98a327728,
            0xe4fa2054a80b329c, 0x727d102a548b194e, 0x39b008152acb8227, 0x9258048415eb419d,
            0x492c024284fbaec0, 0xaa16012142f35760, 0x550b8e9e21f7a530, 0xa48b474f9ef5dc18,
            0x70a6a56e2440598e, 0x3853dc371220a247, 0x1ca76e95091051ad, 0x0edd37c48a08a6d8,
            0x07e095624504536c, 0x8d70c431ac02a736, 0xc83862965601dd1b, 0x641c314b2b8ee083,
        };

        #endregion

        private static readonly ulong[][] s_LookupTable = InitializeLookupTable();

        private byte[]
            _sigma,
            _buffer,
            _tempKey,
            _tempBuffer,
            _iv;

        private ulong _n;
        private long _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="Streebog512Managed"/> class.
        /// </summary>
        public Streebog512Managed()
            : this(s_iv)
        { }

        internal Streebog512Managed(byte[] iv)
        {
            _iv = iv;
            HashValue = new byte[64];
            BlockCopy(_iv, 0, HashValue, 0, 64);
            _sigma = new byte[64];
            _buffer = new byte[64];
            _tempKey = new byte[64];
            _tempBuffer = new byte[64];
        }

        /// <summary>
        /// Initializes an instance of <see cref="Streebog512Managed"/>.
        /// </summary>
        public override void Initialize()
        {
            _count = 0L;
            _n = 0uL;

            BlockCopy(_iv, 0, HashValue, 0, 64);
            Array.Clear(_sigma, 0, 64);
            Array.Clear(_buffer, 0, 64);
            Array.Clear(_tempKey, 0, 64);
            Array.Clear(_tempBuffer, 0, 64);
        }

        /// <summary>
        /// Routes data written to the object into the <see cref="Streebog512"/> hash algorithm for computing the hash.
        /// </summary>
        /// <param name="data">
        /// The input data. 
        /// </param>
        /// <param name="dataOffset">
        /// The offset into the byte array from which to begin using data. 
        /// </param>
        /// <param name="dataSize">
        /// The number of bytes in the array to use as data. 
        /// </param>
        protected override void HashCore(byte[] data, int dataOffset, int dataSize)
        {
            // Compute length of buffer 
            int bufferLen = (int)(_count & 0x3f);

            // Update number of bytes
            _count += dataSize;

            if ((bufferLen > 0) && (bufferLen + dataSize >= 64))
            {
                int bytesToCopy = 64 - bufferLen;
                BlockCopy(data, dataOffset, _buffer, bufferLen, bytesToCopy);
                dataOffset += bytesToCopy;
                dataSize -= bytesToCopy;
                DoTransform(_buffer, 512);
                bufferLen = 0;
            }

            // Copy input to temporary buffer and hash
            while (dataSize >= 64)
            {
                BlockCopy(data, dataOffset, _buffer, 0, 64);
                dataOffset += 64;
                dataSize -= 64;
                DoTransform(_buffer, 512);
            }

            if (dataSize > 0)
                BlockCopy(data, dataOffset, _buffer, bufferLen, dataSize);
        }

        /// <summary>
        /// Returns the computed <see cref="Streebog512"/> hash value after all data has been written to the object.
        /// </summary>
        /// <returns>
        /// The computed hash code.
        /// </returns>
        protected override byte[] HashFinal()
        {
            // Compute length of buffer 
            int bufferLen = (int)(_count & 0x3f);

            byte[] lastBlock = new byte[64];
            Array.Copy(_buffer, 0, lastBlock, 0, bufferLen);
            lastBlock[bufferLen] = 1;

            DoTransform(lastBlock, (uint)(bufferLen * 8));

            DoFinalTransform(_n, _sigma);

            return HashValue;
        }

        private void DoTransform(byte[] block, uint blockSize)
        {
            Xor(HashValue, _n, _tempKey);
            Transform(_tempKey);
            Encrypt(_tempKey, block, _tempBuffer);
            Xor(_tempBuffer, HashValue, _tempBuffer);
            Xor(_tempBuffer, block, HashValue);

            AddModuloLittleEndian(_sigma, block, _sigma);
            _n += blockSize;
        }

        private void DoFinalTransform(ulong sizeInBits, byte[] sigma)
        {
            byte[] n = new byte[64];
            AddModuloLittleEndian(n, sizeInBits, n);

            BlockCopy(HashValue, 0, _tempKey, 0, 64);
            Transform(_tempKey);
            Encrypt(_tempKey, n, _tempBuffer);
            Xor(_tempBuffer, HashValue, _tempBuffer);
            Xor(_tempBuffer, n, HashValue);

            BlockCopy(HashValue, 0, _tempKey, 0, 64);
            Transform(_tempKey);
            Encrypt(_tempKey, sigma, _tempBuffer);
            Xor(_tempBuffer, HashValue, _tempBuffer);
            Xor(_tempBuffer, sigma, HashValue);
        }

        private static void Encrypt(byte[] key, byte[] block, byte[] result)
        {
            Xor(key, block, result);

            for (int i = 0; i < 12; i++)
            {
                Transform(result);
                Xor(key, s_keyExpansionTable[i], key);
                Transform(key);
                Xor(result, key, result);
            }
        }

        private static void Transform(byte[] data)
        {
            // Transposition function
            for (int i = 0; i < 7; i++)
                for (int j = i + 1; j < 8; j++)
                {
                    int
                        i1 = i * 8 + j,
                        i2 = j * 8 + i;
                    byte tmp = data[i1];
                    data[i1] = data[i2];
                    data[i2] = tmp;
                }

            // Linear transform with precomputed substitution
            for (int shift = 0; shift < 64; shift += 8)
            {
                ulong t = 0;

                for (int j = 0; j < 8; j++)
                    t ^= s_LookupTable[j][data[shift + (7 - j)]];

                UInt64ToLittleEndian(t, data, shift);
            }
        }

        private static void Xor(byte[] left, byte[] right, byte[] result)
            => CryptoUtils.Xor(left, 0, right, 0, result, 0, 64);

        private static void Xor(byte[] left, ulong right, byte[] result)
        {
            for (int i = 0; i < 8; i++)
                result[i] = (byte)(left[i] ^ (byte)(right >> (i * 8)));
            BlockCopy(left, 8, result, 8, 56);
        }

        private static void AddModuloLittleEndian(byte[] left, byte[] right, byte[] result)
        {
            int t = 0;
            for (int i = 0; i < 64; i++)
            {
                t = left[i] + right[i] + (t >> 8);
                result[i] = (byte)t;
            }
        }

        private static void AddModuloLittleEndian(byte[] left, ulong right, byte[] result)
        {
            int i = 0, t = 0;
            while (i < 8)
            {
                byte rightByte = (byte)(right >> (i * 8));
                t = left[i] + rightByte + (t >> 8);
                result[i] = (byte)t;
                i++;
            }
            while (i < 64)
            {
                t = left[i] + (t >> 8);
                result[i] = (byte)t;
                i++;
            }
        }

        private static ulong[][] InitializeLookupTable()
        {
            var result = new ulong[8][];

            for (int j = 0; j < 8; j++)
            {
                var row = new ulong[256];

                for (int b = 0; b < 256; b++)
                {
                    ulong t = 0;
                    for (int k = 0; k < 8; k++)
                        if (((b >> (7 - k)) & 1) == 1)
                            t = t ^ s_linearTransformTable[j * 8 + k];
                    row[s_backwardSubstitutionBox[b]] = t;
                }

                result[j] = row;
            }

            return result;
        }
    }
}